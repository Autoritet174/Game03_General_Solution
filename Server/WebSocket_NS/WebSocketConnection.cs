using Server_DB_UserData;
using Server.Game03;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;

namespace Server.WebSocket_NS;

public class WebSocketConnection(WebSocket webSocket, ILogger<WebSocketConnection> logger, IConfiguration configuration, WebSocketConnectionHandler webSocketServer, MongoRepository mongoRepository)
{
    private Guid Id { get; } = Guid.NewGuid();
    private readonly WebSocket _webSocket = webSocket;
    private readonly ILogger<WebSocketConnection> _logger = logger;
    private readonly int _receiveBufferSize = configuration.GetValue<int>("WebSocketSettings:ReceiveBufferSize");
    private readonly WebSocketConnectionHandler _webSocketServer = webSocketServer;
    private readonly PlayerManager _playerManager = new(mongoRepository);

    public async Task HandleAsync(CancellationToken cancellationToken)
    {
        //string m = $"Клиент {_id} подключён";www
        //_logger.LogInformation("Клиент {_id} подключён", _id);
        _webSocketServer.ActiveConnectionsAdd(Id);

        //Console.WriteLine(m);

        // Используем ArrayPool для уменьшения нагрузки на GC
        var buffer = ArrayPool<byte>.Shared.Rent(_receiveBufferSize);

        try
        {
            // Отправляем приветственное сообщение
            await SendMessageSafeAsync($"Добро пожаловать! Ваш ID: {Id}", cancellationToken);

            while (_webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocketAsync(WebSocketCloseStatus.NormalClosure, "Закрытие по инициативе клиента", cancellationToken);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                //_logger.LogDebug("Принято сообщение от клиента {ClientId}: {Message}", _id, message);
                await _playerManager.Command(message);
                //DateTime dt = DateTime.Now;
                //string[] sA = message.Split(new string[] { ".", ":" }, StringSplitOptions.None);
                //int i1 = int.Parse(sA[0]);
                //int i2 = int.Parse(sA[1]);
                //int i3 = int.Parse(sA[2]);
                //int i4 = int.Parse(sA[3]);

                //int m1 = i4 / 1000;
                //int m2 = i4 - (m1 * 1000);

                //DateTime dtClient = new(2025, 09, 02, i1, i2, i3, m1, m2);
                //TimeSpan t = dt - dtClient;
                //Console.WriteLine($"Принято сообщение от клиента {Id}: {message} timeServer: {DateTime.Now:HH:mm:ss.ffffff} {t.TotalMicroseconds}");


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
            if (IsExpectedDisconnectException(ex))
            {
                //_logger.LogInformation("Клиент {ClientId} разорвал соединение", _id);
                Console.WriteLine($"Клиент {Id} разорвал соединение. Активных подключений: {_webSocketServer.GetCount()}");
            }
            else
            {
                _logger.LogError(ex, "Неожиданная ошибка при обработке клиента {ClientId}", Id);
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

    private async Task SendMessageSafeAsync(string message, CancellationToken cancellationToken)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            return;
        }

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
            _logger.LogDebug("Не удалось отправить сообщение клиенту {ClientId} (соединение закрыто): {Message}", Id, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Неожиданная ошибка при отправке сообщения клиенту {ClientId}", Id);
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
                    Id, ex.Message);
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
                _webSocket.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка при освобождении сокета клиента {ClientId}", Id);
            }
            try
            {
                _webSocketServer.ActiveConnectionsRemove(Id);
            }
            catch (Exception)
            {
            }
        }
    }
}
