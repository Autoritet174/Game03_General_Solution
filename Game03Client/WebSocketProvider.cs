using General.DTO;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

/// <summary>
/// Провайдер WebSocket: Zero-Allocation (по возможности), защита от утечек и Buffer Bloat.
/// </summary>
public class WebSocketProvider
{
    private static readonly Logger<WebSocketProvider> logger = new();
    private const string SERVER_URL = "wss://localhost:7227/ws/";

    private static ClientWebSocket _webSocket = new();
    private static readonly Uri _serverUri = new(SERVER_URL);

    private static CancellationTokenSource? _internalDisconnectCts;
    private static CancellationTokenSource? _linkedCts;

    // Структура для кэша ответов с меткой времени
    private readonly struct CachedResponse(DtoWSResponseS2C response)
    {
        public readonly DtoWSResponseS2C Response = response;
        public readonly long Timestamp = DateTime.UtcNow.Ticks; // Ticks
    }

    private static readonly ConcurrentDictionary<Guid, CachedResponse> _responseDictionary = new();

    // TaskCreationOptions.RunContinuationsAsynchronously критичен для избежания дедлоков и блокировок цикла приема
    private static readonly ConcurrentDictionary<Guid, TaskCompletionSource<DtoWSResponseS2C>> _pendingRequests = new();

    // Таймер для очистки "сиротских" ответов
    private static Timer? _cleanupTimer;
    private static readonly TimeSpan _responseTtl = TimeSpan.FromSeconds(30);

    // Порог, после которого мы пересоздаем буфер приема, чтобы освободить память (64 KB)
    private const int MAX_BUFFER_RETAIN_SIZE = 64 * 1024;
    private const int INITIAL_BUFFER_SIZE = 8192;

    /// <summary>
    /// Извлекает ответ из очереди.
    /// </summary>
    public static DtoWSResponseS2C? GetIfExists(Guid messageId)
    {
        return _responseDictionary.TryRemove(messageId, out CachedResponse wrapper) ? wrapper.Response : null;
    }

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    public static async Task<bool> ConnectAsync(CancellationToken ctOpen, CancellationToken ctReceive)
    {
        if (ctOpen.IsCancellationRequested || ctReceive.IsCancellationRequested)
        {
            return false;
        }

        try
        {
            _webSocket.Dispose();
            _webSocket = new ClientWebSocket();

            if (!string.IsNullOrWhiteSpace(Auth.AccessToken))
            {
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {Auth.AccessToken}");
            }

            await _webSocket.ConnectAsync(_serverUri, ctOpen).ConfigureAwait(false);

            if (_webSocket.State == WebSocketState.Open)
            {
                _internalDisconnectCts = new CancellationTokenSource();
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ctReceive, _internalDisconnectCts.Token);

                // Запускаем таймер очистки (раз в минуту)
                _ = (_cleanupTimer?.DisposeAsync());
                _cleanupTimer = new Timer(CleanupExpiredResponses, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

                _ = Task.Run(() => ReceiveMessagesAsync(_linkedCts.Token), _linkedCts.Token);
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        return false;
    }

    /// <summary>
    /// Удаление устаревших ответов.
    /// </summary>
    private static void CleanupExpiredResponses(object? state)
    {
        try
        {
            long threshold = DateTime.UtcNow.Ticks - _responseTtl.Ticks;
            foreach (KeyValuePair<Guid, CachedResponse> kvp in _responseDictionary)
            {
                if (kvp.Value.Timestamp < threshold)
                {
                    _ = _responseDictionary.TryRemove(kvp.Key, out _);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка очистки кэша ответов: {ex.Message}");
        }
    }

    /// <summary>
    /// Цикл приема сообщений с защитой от Buffer Bloat.
    /// </summary>
    private static async Task ReceiveMessagesAsync(CancellationToken ct)
    {
        // Буфер для чтения части сообщения из сокета
        byte[] chunkBuffer = ArrayPool<byte>.Shared.Rent(INITIAL_BUFFER_SIZE);

        // Буфер для накопления полного сообщения
        byte[] accumulatorBuffer = ArrayPool<byte>.Shared.Rent(INITIAL_BUFFER_SIZE);
        try
        {
            while (_webSocket.State == WebSocketState.Open && !ct.IsCancellationRequested)
            {
                int totalBytesReceived = 0;
                WebSocketReceiveResult result;

                do
                {
                    // 1. Читаем чанк
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(chunkBuffer), ct).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync().ConfigureAwait(false);
                        return;
                    }

                    // 2. Если чанк не влезает в аккумулятор -> расширяем аккумулятор
                    if (totalBytesReceived + result.Count > accumulatorBuffer.Length)
                    {
                        int newSize = Math.Max(accumulatorBuffer.Length * 2, totalBytesReceived + result.Count);
                        byte[] newBuffer = ArrayPool<byte>.Shared.Rent(newSize);

                        Buffer.BlockCopy(accumulatorBuffer, 0, newBuffer, 0, totalBytesReceived);

                        ArrayPool<byte>.Shared.Return(accumulatorBuffer);
                        accumulatorBuffer = newBuffer;
                    }

                    // 3. Копируем данные
                    Buffer.BlockCopy(chunkBuffer, 0, accumulatorBuffer, totalBytesReceived, result.Count);
                    totalBytesReceived += result.Count;

                } while (!result.EndOfMessage);

                // 4. Обрабатываем
                if (result.MessageType == WebSocketMessageType.Text && totalBytesReceived > 0)
                {
                    ProcessMessageSpan(new ReadOnlySpan<byte>(accumulatorBuffer, 0, totalBytesReceived));
                }

                // 5. PROTECTION AGAINST BUFFER BLOAT
                // Если буфер разросся (например, пришел 1MB), а обычно сообщения мелкие,
                // возвращаем гиганта в пул и берем стандартный размер.
                if (accumulatorBuffer.Length > MAX_BUFFER_RETAIN_SIZE)
                {
                    ArrayPool<byte>.Shared.Return(accumulatorBuffer);
                    accumulatorBuffer = ArrayPool<byte>.Shared.Rent(INITIAL_BUFFER_SIZE);
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(chunkBuffer);
            ArrayPool<byte>.Shared.Return(accumulatorBuffer);
        }
    }

    /// <summary>
    /// Десериализация из Span (Zero-Allocation строки).
    /// </summary>
    private static void ProcessMessageSpan(ReadOnlySpan<byte> data)
    {
        try
        {
            DtoWS? dtoWS = JsonSerializer.Deserialize<DtoWS>(data);

            if (dtoWS is DtoWSResponseS2C response)
            {
                OnMessageReceived(response);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка десериализации: {ex.Message}");
        }
    }

    private static void OnMessageReceived(DtoWSResponseS2C response)
    {
        // Убираем StartNew, так как TCS создан с RunContinuationsAsynchronously.
        // Это минимизирует Latency и убирает лишние аллокации Task.
        if (_pendingRequests.TryRemove(response.InReplyTo, out TaskCompletionSource<DtoWSResponseS2C>? tcs))
        {
            _ = tcs.TrySetResult(response);
        }
        else
        {
            // Сохраняем "сиротский" ответ с текущим Timestamp
            _ = _responseDictionary.TryAdd(response.InReplyTo, new CachedResponse(response));
        }
    }

    public static async Task<bool> SendMessageAsync(DtoWSEquipmentTakeOnC2S message, CancellationToken ct)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            return false;
        }

        try
        {
            // Сериализация напрямую в байты
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message);

            await _webSocket.SendAsync(
                new ArraySegment<byte>(jsonBytes),
                WebSocketMessageType.Text,
                true,
                ct
            ).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка отправки: {ex.Message}");
            return false;
        }
    }

    public static async Task<DtoWSResponseS2C?> WaitForResponseAsync(Guid messageId, TimeSpan timeout, CancellationToken ct = default)
    {
        if (_responseDictionary.TryRemove(messageId, out CachedResponse existing))
        {
            return existing.Response;
        }

        var tcs = new TaskCompletionSource<DtoWSResponseS2C>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_pendingRequests.TryAdd(messageId, tcs))
        {
            throw new InvalidOperationException($"Duplicate wait for {messageId}");
        }

        // Double-Check
        if (_responseDictionary.TryRemove(messageId, out CachedResponse missed))
        {
            _ = _pendingRequests.TryRemove(messageId, out _);
            return missed.Response;
        }

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var timeoutTask = Task.Delay(timeout, linkedCts.Token);

        Task completedTask = await Task.WhenAny(tcs.Task, timeoutTask).ConfigureAwait(false);

        _ = _pendingRequests.TryRemove(messageId, out _);

        if (completedTask == tcs.Task)
        {
            linkedCts.Cancel();
            return await tcs.Task.ConfigureAwait(false);
        }

        return null;
    }

    public static async Task DisconnectAsync()
    {
        try
        {
            _internalDisconnectCts?.Cancel();
            _ = (_cleanupTimer?.DisposeAsync());

            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Client disconnect",
                    timeoutCts.Token).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Disconnection error: {ex.Message}");
        }
        finally
        {
            _webSocket.Dispose();
            _linkedCts?.Dispose();
            _internalDisconnectCts?.Dispose();
            _linkedCts = null;
            _internalDisconnectCts = null;
        }
    }
}
