using System.Buffers;
using System.Net.WebSockets;
using System.Text;

namespace Server.WebSocket_NS;

/// <summary>
/// Представляет одно WebSocket подключение клиента.
/// Обрабатывает прием и отправку сообщений, аутентификацию и управление жизненным циклом подключения.
/// </summary>
public class WebSocketConnection(
    WebSocket webSocket
    , ILogger<WebSocketConnection> logger
    , IConfiguration configuration
    , WebSocketConnectionHandler webSocketServer
    //, IServiceProvider serviceProvider
    ,Guid userId)
{
    /// <summary>
    /// Уникальный идентификатор подключения.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Размер буфера для приема сообщений из конфигурации.
    /// </summary>
    private readonly int _receiveBufferSize = configuration.GetValue<int>("WebSocketSettings:ReceiveBufferSize");

    /// <summary>
    /// Обрабатывает WebSocket подключение клиента.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для прерывания обработки.</param>
    /// <returns>Задача, представляющая асинхронную обработку подключения.</returns>
    /// <exception cref="OperationCanceledException">
    /// Возникает при отмене операции через cancellationToken.
    /// </exception>
    /// <exception cref="WebSocketException">
    /// Возникает при ошибках WebSocket соединения.
    /// </exception>
    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        // Используем ArrayPool для уменьшения нагрузки на GC
        byte[] buffer = ArrayPool<byte>.Shared.Rent(_receiveBufferSize);

        try
        {
            // Отправляем приветственное сообщение
            await SendMessageSafeAsync($"Добро пожаловать! Ваш userId: {userId}", cancellationToken).ConfigureAwait(false);

            // Основной цикл чтения
            while (webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                WebSocketReceiveResult result;
                try
                {
                    // Используем ArraySegment для работы с буфером из пула
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }


                // Обрабатываем запрос на закрытие соединения
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocketAsync(WebSocketCloseStatus.NormalClosure, "Закрытие по инициативе клиента", cancellationToken).ConfigureAwait(false);
                    break;
                }
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    if (logger.IsEnabled(LogLevel.Information))
                    {
                        logger.LogInformation("Клиент {ClientId} инициировал закрытие: {Status}", userId, result.CloseStatus);
                    }

                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Подтверждение закрытия", CancellationToken.None).ConfigureAwait(false);
                    break;
                }

                // Декодируем полученное сообщение
                //string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                //if (logger.IsEnabled(LogLevel.Information))
                //{
                //    logger.LogInformation("FROM {ClientId}: {Message}", userId, message);
                //}

                // Передаем команду менеджеру игроков для обработки
                //await _playerManager.Command(message);

                // Эхо-ответ (если нужно)
                //if (webSocket.State == WebSocketState.Open)
                //{
                //    await SendMessageSafeAsync($"Эхо: {message}", cancellationToken);
                //}
            }
        }
        catch (OperationCanceledException)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Обработка клиента {ClientId} прервана", Id);
            }

        }
        catch (Exception ex)
        {
            // Логируем только неожиданные ошибки, ожидаемые исключения игнорируем
            if (IsExpectedDisconnectException(ex))
            {
                Console.WriteLine($"Клиент {Id} разорвал соединение. Активных подключений: {webSocketServer.GetCount()}");
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, "Неожиданная ошибка при обработке клиента {ClientId}", Id);
                }
            }
        }
        finally
        {
            // Возвращаем буфер в пул и очищаем ресурсы
            ArrayPool<byte>.Shared.Return(buffer);
            await CleanupSocketAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Проверяет, является ли исключение ожидаемым при разрыве соединения.
    /// </summary>
    /// <param name="ex">Исключение для проверки.</param>
    /// <returns>
    /// true - если исключение связано с нормальным разрывом соединения;
    /// false - если это неожиданная ошибка.
    /// </returns>
    public static bool IsExpectedDisconnectException(Exception ex)
    {
        // Проверяем само исключение
        if (ex is ObjectDisposedException ||
            ex is InvalidOperationException ||
            (ex is System.Net.HttpListenerException hlex && hlex.ErrorCode == 995) ||
            (ex is System.IO.IOException ioex && ioex.HResult == -2146232800))
        {
            return true;
        }

        // Проверяем WebSocketException
        if (ex is WebSocketException wex)
        {
            if (wex.WebSocketErrorCode is WebSocketError.ConnectionClosedPrematurely or
                WebSocketError.InvalidState)
            {
                return true;
            }

            // Проверяем внутреннее исключение WebSocketException
            if (wex.InnerException != null)
            {
                return IsExpectedDisconnectException(wex.InnerException);
            }
        }

        // Рекурсивно проверяем внутренние исключения
        return ex.InnerException != null && IsExpectedDisconnectException(ex.InnerException);
    }

    /// <summary>
    /// Безопасно отправляет сообщение клиенту.
    /// </summary>
    /// <param name="message">Текст сообщения для отправки.</param>
    /// <param name="cancellationToken">Токен отмены операции отправки.</param>
    /// <returns>Задача, представляющая асинхронную отправку сообщения.</returns>
    private async Task SendMessageSafeAsync(string message, CancellationToken cancellationToken)
    {
        // Проверяем, что соединение еще открыто
        if (webSocket.State != WebSocketState.Open)
        {
            return;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,  // Отправляем полное сообщение
                cancellationToken
            ).ConfigureAwait(false);
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("сообщение клиенту {ClientId}: {Message}", Id, message);
            }
        }
        catch (Exception ex) when (IsExpectedDisconnectException(ex))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                // Игнорируем ошибки при закрытых соединениях
                logger.LogDebug("Не удалось отправить сообщение клиенту {ClientId} (соединение закрыто): {Message}", Id, ex.Message);
            }
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(ex, "Неожиданная ошибка при отправке сообщения клиенту {ClientId}", Id);
            }
        }
    }

    /// <summary>
    /// Корректно закрывает WebSocket соединение.
    /// </summary>
    /// <param name="closeStatus">Статус закрытия соединения.</param>
    /// <param name="statusDescription">Описание причины закрытия.</param>
    /// <param name="cancellationToken">Токен отмены операции закрытия.</param>
    /// <returns>Задача, представляющая асинхронное закрытие соединения.</returns>
    private async Task CloseSocketAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        if (webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
        {
            try
            {
                await webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken).ConfigureAwait(false);
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Отключение клиента {ClientId}", Id);
                }
            }
            catch (Exception ex) when (IsExpectedDisconnectException(ex))
            {
                // Игнорируем ожидаемые ошибки при закрытии
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Ошибка при закрытии сокета клиента {ClientId} (уже закрыт): {Message}",
                    Id, ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// Очищает ресурсы WebSocket соединения.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную очистку ресурсов.</returns>
    private async Task CleanupSocketAsync()// ТУТ НЕ НУЖЕН CancellationToken
    {
        try
        {
            // Закрываем соединение, если оно еще открыто
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Завершение работы", CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (IsExpectedDisconnectException(ex))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                // Игнорируем ожидаемые ошибки при очистке
                logger.LogDebug("Ошибка при очистке сокета клиента {ClientId} (уже закрыт): {Message}",
                Id, ex.Message);
            }
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(ex, "Неожиданная ошибка при очистке сокета клиента {ClientId}", Id);
            }
        }
        finally
        {
            try
            {
                webSocket.Dispose();  // Освобождаем ресурсы WebSocket
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning(ex, "Ошибка при освобождении сокета клиента {ClientId}", Id);
                }
            }
            try
            {
                // Удаляем подключение из списка активных
                webSocketServer.ActiveConnectionsRemove(Id);
            }
            catch (Exception)
            {
                // Игнорируем ошибки при удалении из списка подключений
            }
        }
    }
}
