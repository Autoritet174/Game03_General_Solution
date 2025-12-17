using Server.Jwt_NS;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;

namespace Server.WebSocket_NS;

/// <summary>
/// Представляет одно WebSocket подключение клиента.
/// Обрабатывает прием и отправку сообщений, аутентификацию и управление жизненным циклом подключения.
/// </summary>
public class WebSocketConnection(WebSocket webSocket, ILogger<WebSocketConnection> logger, IConfiguration configuration, WebSocketConnectionHandler webSocketServer, JwtService jwtService)
{
    /// <summary>
    /// Уникальный идентификатор подключения.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// WebSocket клиента для обмена сообщениями.
    /// </summary>
    private readonly WebSocket _webSocket = webSocket;

    /// <summary>
    /// Логгер для записи событий подключения.
    /// </summary>
    private readonly ILogger<WebSocketConnection> _logger = logger;

    /// <summary>
    /// Размер буфера для приема сообщений из конфигурации.
    /// </summary>
    private readonly int _receiveBufferSize = configuration.GetValue<int>("WebSocketSettings:ReceiveBufferSize");

    /// <summary>
    /// Обработчик WebSocket соединений для управления подключениями.
    /// </summary>
    private readonly WebSocketConnectionHandler _webSocketServer = webSocketServer;

    /// <summary>
    /// Сервис для работы с JWT токенами аутентификации.
    /// </summary>
    private readonly JwtService _jwtService = jwtService;

    /// <summary>
    /// Флаг, указывающий на аутентификацию клиента.
    /// </summary>
    private readonly bool _isAuthenticated = false;

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
            await SendMessageSafeAsync($"Добро пожаловать! Ваш ID: {Id}", cancellationToken);

            while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                // Ожидаем сообщение от клиента
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                // Обрабатываем запрос на закрытие соединения
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocketAsync(WebSocketCloseStatus.NormalClosure, "Закрытие по инициативе клиента", cancellationToken);
                    break;
                }

                // Декодируем полученное сообщение
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logger.LogInformation("Принято сообщение от клиента {ClientId}: {Message}", Id, message);

                // Передаем команду менеджеру игроков для обработки
                //await _playerManager.Command(message);

                // Эхо-ответ (если нужно)
                if (_webSocket.State == WebSocketState.Open)
                {
                    await SendMessageSafeAsync($"Эхо: {message}", cancellationToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Обработка клиента {ClientId} прервана", Id);
        }
        catch (Exception ex)
        {
            // Логируем только неожиданные ошибки, ожидаемые исключения игнорируем
            if (IsExpectedDisconnectException(ex))
            {
                Console.WriteLine($"Клиент {Id} разорвал соединение. Активных подключений: {_webSocketServer.GetCount()}");
            }
            else
            {
                _logger.LogError(ex, "Неожиданная ошибка при обработке клиента {ClientId}", Id);
            }
        }
        finally
        {
            // Возвращаем буфер в пул и очищаем ресурсы
            ArrayPool<byte>.Shared.Return(buffer);
            await CleanupSocketAsync();
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
        if (_webSocket.State != WebSocketState.Open)
        {
            return;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,  // Отправляем полное сообщение
                cancellationToken
            );
            _logger.LogInformation("сообщение клиенту {ClientId}: {Message}", Id, message);
        }
        catch (Exception ex) when (IsExpectedDisconnectException(ex))
        {
            // Игнорируем ошибки при закрытых соединениях
            _logger.LogDebug("Не удалось отправить сообщение клиенту {ClientId} (соединение закрыто): {Message}", Id, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Неожиданная ошибка при отправке сообщения клиенту {ClientId}", Id);
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
        if (_webSocket.State == WebSocketState.Open)
        {
            try
            {
                await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
            }
            catch (Exception ex) when (IsExpectedDisconnectException(ex))
            {
                // Игнорируем ожидаемые ошибки при закрытии
                _logger.LogDebug("Ошибка при закрытии сокета клиента {ClientId} (уже закрыт): {Message}",
                    Id, ex.Message);
            }
        }
    }

    /// <summary>
    /// Очищает ресурсы WebSocket соединения.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную очистку ресурсов.</returns>
    private async Task CleanupSocketAsync()
    {
        try
        {
            // Закрываем соединение, если оно еще открыто
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Завершение работы", CancellationToken.None);
            }
        }
        catch (Exception ex) when (IsExpectedDisconnectException(ex))
        {
            // Игнорируем ожидаемые ошибки при очистке
            _logger.LogDebug("Ошибка при очистке сокета клиента {ClientId} (уже закрыт): {Message}",
                Id, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Неожиданная ошибка при очистке сокета клиента {ClientId}", Id);
        }
        finally
        {
            try
            {
                _webSocket.Dispose();  // Освобождаем ресурсы WebSocket
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка при освобождении сокета клиента {ClientId}", Id);
            }
            try
            {
                // Удаляем подключение из списка активных
                _webSocketServer.ActiveConnectionsRemove(Id);
            }
            catch (Exception)
            {
                // Игнорируем ошибки при удалении из списка подключений
            }
        }
    }
}
