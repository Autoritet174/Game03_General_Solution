using General.DTO;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

/// <summary>
/// Провайдер для работы с WebSocket соединением с поддержкой фрагментации и оптимизацией памяти.
/// </summary>
public partial class WebSocketProvider
{

    private static readonly Logger<WebSocketProvider> logger = new();
    private const string SERVER_URL = "wss://localhost:7227/ws/";

    private static ClientWebSocket _webSocket = new();
    private static readonly Uri _serverUri = new(SERVER_URL);

    // Внутренний источник для прерывания работы при вызове DisconnectAsync
    private static CancellationTokenSource? _internalDisconnectCts;
    // Комбинированный токен (внешний + внутренний)
    private static CancellationTokenSource? _linkedCts;

    private static readonly ConcurrentDictionary<Guid, DtoWSResponseS2C> _responseDictionary = [];
    private static readonly ConcurrentDictionary<Guid, TaskCompletionSource<DtoWSResponseS2C>> _pendingRequests = [];

    /// <summary>
    /// Извлекает ответ из очереди, если он уже был получен.
    /// </summary>
    /// <param name="messageId">ID сообщения.</param>
    /// <returns>Ответ сервера или null.</returns>
    public static DtoWSResponseS2C? GetIfExists(Guid messageId) =>
        _responseDictionary.TryRemove(messageId, out DtoWSResponseS2C? response) ? response : null;

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    /// <param name="ctOpen">Токен для отмены процесса установки соединения.</param>
    /// <param name="ctReceive">Токен, определяющий время жизни цикла приема сообщений.</param>
    /// <returns>True, если соединение установлено.</returns>
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
                // Инициализируем механизмы отмены
                _internalDisconnectCts = new CancellationTokenSource();
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ctReceive, _internalDisconnectCts.Token);

                // Запуск цикла приема
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
    /// Бесконечный цикл приема сообщений.
    /// </summary>
    private static async Task ReceiveMessagesAsync(CancellationToken ct)
    {
        // Арендуем буфер для снижения нагрузки на GC
        byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

        try
        {
            while (_webSocket.State == WebSocketState.Open && !ct.IsCancellationRequested)
            {
                using var ms = new MemoryStream();
                WebSocketReceiveResult result;

                do
                {
                    // Используем Memory для исключения лишних аллокаций при работе с потоком
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectAsync().ConfigureAwait(false);
                        return;
                    }

                    await ms.WriteAsync(buffer.AsMemory(0, result.Count), ct).ConfigureAwait(false);

                } while (!result.EndOfMessage);

                _ = ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await ProcessTextMessageAsync(ms, ct).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException) { /* Ожидаемое завершение */ }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Обработка текстового сообщения с использованием асинхронной десериализации.
    /// </summary>
    private static async Task ProcessTextMessageAsync(MemoryStream ms, CancellationToken ct)
    {
        try
        {
            // Десериализация напрямую из потока (Zero-string allocation)
            DtoWS? dtoWS = await JsonSerializer.DeserializeAsync<DtoWS>(ms, cancellationToken: ct).ConfigureAwait(false);

            if (dtoWS is DtoWSResponseS2C response)
            {
                OnMessageReceived(response);
            }
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }
    }

    /// <summary>
    /// Распределение полученного ответа.
    /// </summary>
    public static void OnMessageReceived(DtoWSResponseS2C response)
    {
        if (_pendingRequests.TryRemove(response.InReplyTo, out TaskCompletionSource<DtoWSResponseS2C>? tcs))
        {
            _ = tcs.TrySetResult(response);
        }
        else
        {
            _ = _responseDictionary.TryAdd(response.InReplyTo, response);
        }
    }

    /// <summary>
    /// Отправка данных на сервер.
    /// </summary>
    public static async Task<bool> SendMessageAsync(DtoWSEquipmentTakeOnC2S message, CancellationToken ct)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            return false;
        }

        try
        {
            // Прямая сериализация в байты
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

    /// <summary>
    /// Синхронное ожидание ответа по ID с защитой от гонки.
    /// </summary>
    public static async Task<DtoWSResponseS2C?> WaitForResponseAsync(Guid messageId, TimeSpan timeout, CancellationToken ct = default)
    {
        if (_responseDictionary.TryRemove(messageId, out DtoWSResponseS2C? existing))
        {
            return existing;
        }

        var tcs = new TaskCompletionSource<DtoWSResponseS2C>(TaskCreationOptions.RunContinuationsAsynchronously);

        if (!_pendingRequests.TryAdd(messageId, tcs))
        {
            throw new InvalidOperationException($"Duplicate wait for {messageId}");
        }

        // Double-check: вдруг сообщение пришло пока мы создавали TCS
        if (_responseDictionary.TryRemove(messageId, out DtoWSResponseS2C? missed))
        {
            _ = _pendingRequests.TryRemove(messageId, out _);
            return missed;
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

    /// <summary>
    /// Завершение соединения и освобождение ресурсов.
    /// </summary>
    public static async Task DisconnectAsync()
    {
        try
        {
            // Прерываем цикл приема через внутренний токен
            _internalDisconnectCts?.Cancel();

            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Закрытие клиентом",
                    timeoutCts.Token).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка при отключении: {ex.Message}");
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
