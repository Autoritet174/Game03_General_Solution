using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.WebSocket_NS
{
    public class WebSocketClient
    {
        private readonly WebSocket _webSocket;
        private readonly Guid _id;
        private readonly ILogger<WebSocketClient> _logger;
        private readonly int _receiveBufferSize;
        WebSocketServer _webSocketServer;

        public Guid Id => _id;

        public WebSocketClient(WebSocket webSocket, ILogger<WebSocketClient> logger, IConfiguration configuration, WebSocketServer webSocketServer)
        {
            _webSocket = webSocket;
            _id = Guid.NewGuid();
            _logger = logger;
            _webSocketServer = webSocketServer;
            _receiveBufferSize = configuration.GetValue<int>("WebSocketSettings:ReceiveBufferSize", 8192);
        }

        public async Task HandleAsync(CancellationToken cancellationToken)
        {


            //string m = $"Клиент {_id} подключён";
            //_logger.LogInformation("Клиент {_id} подключён", _id);
            _webSocketServer.ActiveConnectionsAdd(_id);
            
            //Console.WriteLine(m);

            // Используем ArrayPool для уменьшения нагрузки на GC
            var buffer = ArrayPool<byte>.Shared.Rent(_receiveBufferSize);

            try
            {
                // Отправляем приветственное сообщение
                await SendMessageSafeAsync($"Добро пожаловать! Ваш ID: {_id}", cancellationToken);

                while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await CloseSocketAsync(WebSocketCloseStatus.NormalClosure, "Закрытие по инициативе клиента", cancellationToken);
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    //_logger.LogDebug("Принято сообщение от клиента {ClientId}: {Message}", _id, message);
                    //Console.WriteLine($"Принято сообщение от клиента {_id}: {message}");

                    // Эхо-ответ (если нужно)
                    if (_webSocket.State == WebSocketState.Open)
                    {
                        await SendMessageSafeAsync($"Эхо: {message}", cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Обработка клиента {ClientId} прервана", _id);
            }
            catch (Exception ex)
            {
                if (IsExpectedDisconnectException(ex))
                {
                    //_logger.LogInformation("Клиент {ClientId} разорвал соединение", _id);
                    Console.WriteLine($"Клиент {_id} разорвал соединение. Активных подключений: {_webSocketServer.GetCount()}");
                }
                else
                {
                    _logger.LogError(ex, "Неожиданная ошибка при обработке клиента {ClientId}", _id);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                await CleanupSocketAsync();
                //_logger.LogInformation("Клиент {ClientId} отключён", _id);
            }
        }
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
                if (wex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely ||
                    wex.WebSocketErrorCode == WebSocketError.InvalidState)
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
            if (ex.InnerException != null)
            {
                return IsExpectedDisconnectException(ex.InnerException);
            }

            return false;
        }

        private async Task SendMessageSafeAsync(string message, CancellationToken cancellationToken)
        {
            if (_webSocket.State != WebSocketState.Open)
                return;

            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken
                );
            }
            catch (Exception ex) when (IsExpectedDisconnectException(ex))
            {
                _logger.LogDebug("Не удалось отправить сообщение клиенту {ClientId} (соединение закрыто): {Message}",
                    _id, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Неожиданная ошибка при отправке сообщения клиенту {ClientId}", _id);
            }
        }

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
                        _id, ex.Message);
                }
            }
        }

        private async Task CleanupSocketAsync()
        {
            try
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Завершение работы", CancellationToken.None);
                }
            }
            catch (Exception ex) when (IsExpectedDisconnectException(ex))
            {
                // Игнорируем ожидаемые ошибки при очистке
                _logger.LogDebug("Ошибка при очистке сокета клиента {ClientId} (уже закрыт): {Message}",
                    _id, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Неожиданная ошибка при очистке сокета клиента {ClientId}", _id);
            }
            finally
            {
                try
                {
                    _webSocket.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Ошибка при освобождении сокета клиента {ClientId}", _id);
                }
                try
                {
                    _webSocketServer.ActiveConnectionsRemove(_id);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}