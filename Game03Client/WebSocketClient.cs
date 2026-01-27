using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LOGGER = Game03Client.LOGGER<Game03Client.WebSocketClient>;

namespace Game03Client;

public class WebSocketClient
{
    private const string SERVER_URL = "wss://localhost:7227/ws/";
    private static ClientWebSocket _webSocket = new();
    private static readonly Uri _serverUri = new(SERVER_URL);
    private static bool _isReceiving = false;
    public static bool Connected { get; private set; } = false;
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    public static async Task ConnectAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            Connected = false;
            _webSocket = new();
            string? accessToken = Auth.AccessToken;

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // Добавляем JWT токен в заголовки, если он предоставлен
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            }
            await _webSocket.ConnectAsync(_serverUri, cancellationToken);
        }
        catch { }

        if (_webSocket.State == WebSocketState.Open)
        {
            _isReceiving = true;
            _ = Task.Run(ReceiveMessagesAsync);
            Connected = true;
        }
    }

    /// <summary>
    /// Приём сообщений от сервера.
    /// </summary>
    /// <returns></returns>
    private static async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[4096];

        try
        {
            while (_isReceiving && _webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    _cancellationTokenSource.Token
                );

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                LOGGER.LogInfo($"Получено от сервера: {message}");
                //ProcessReceivedMessage(message);
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            LOGGER.LogError("Соединение закрыто сервером");
            //OnAuthenticationResult?.Invoke(false, "Соединение закрыто сервером");
        }
        catch (Exception ex)
        {
            if (_isReceiving) // Логируем только если не было запланированного отключения
            {
                LOGGER.LogError($"Ошибка приема: {ex.Message}");
                //OnAuthenticationResult?.Invoke(false, $"Ошибка приема: {ex.Message}");
            }
        }
    }


    /// <summary>
    /// Отправить сообщение на сервер.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task SendMessageAsync(string message)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            LOGGER.LogError("WebSocket не подключен");
            return;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                _cancellationTokenSource.Token
            );
            //Console.WriteLine($"Отправлено: {message}");
        }
        catch (OperationCanceledException)
        {
            LOGGER.LogError("Отправка отменена");
        }
        catch (Exception ex)
        {
            LOGGER.LogError($"Ошибка отправки: {ex.Message}");
        }
    }

    public static async Task StartSendingMessages(Action<int>? onMessagesSent = null)
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
        {
            //for (int i = 0; i < 1; i++)
            //{
            //    var message = $"Сообщение #{++messageCount} - {DateTime.Now:HH:mm:ss}";
            //    await SendMessageAsync(message);
            //}
            string com = Console.ReadLine();
            Random r = new();
            if (com == "1")
            {

                var data = new
                {
                    command = "AddItem",
                    item = new
                    {
                        damage = r.Next(1, 10),
                        health = r.Next(10, 20),
                        location = Guid.NewGuid()
                    }
                };

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                LOGGER.LogError(json);
                await SendMessageAsync(json);

            }


            // Сообщаем о количестве отправленных сообщений
            onMessagesSent?.Invoke(100);

            await Task.Delay(10, _cancellationTokenSource.Token);

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
        }

        onMessagesSent?.Invoke(0); // Завершение
    }
    /// <summary>
    /// Корректно отключает клиента от сервера.
    /// </summary>
    /// <returns>Задача асинхронного отключения.</returns>
    public static async Task DisconnectAsync()
    {
        try
        {
            _isReceiving = false;

            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                // CloseAsync отправляет фрейм закрытия и ждет подтверждения от сервера.
                // Это корректный способ завершить соединение, который сервер поймет как CloseMessageType.
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Закрытие клиентом",
                    timeoutCts.Token);
            }
        }
        catch (Exception ex)
        {
            LOGGER.LogError($"Ошибка при отключении: {ex.Message}");
        }
        finally
        {
            _webSocket.Dispose();
            _cancellationTokenSource.Cancel();
            Connected = false;
            LOGGER.LogInfo("Соединение закрыто");
        }
    }
    //public async Task DisconnectAsync()
    //{
    //    try
    //    {
    //        _isReceiving = false; // Останавливаем приём сообщений
    //        _cancellationTokenSource.Cancel(); // Отменяем операции отправки

    //        if (_webSocket.State == WebSocketState.Open)
    //        {
    //            // Отправляем фрейм закрытия с типом Close
    //            await _webSocket.CloseOutputAsync(
    //                WebSocketCloseStatus.NormalClosure,
    //                "Закрытие клиентом",
    //                CancellationToken.None
    //            );

    //            // Опционально: Ждём подтверждения от сервера (до 5 сек)
    //            CancellationTokenSource timeoutCts = new(TimeSpan.FromSeconds(5));
    //            byte[] buffer = new byte[1024];
    //            while (_webSocket.State != WebSocketState.Closed && !timeoutCts.IsCancellationRequested)
    //            {
    //                try
    //                {
    //                    WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), timeoutCts.Token);
    //                    if (result.MessageType == WebSocketMessageType.Close)
    //                    {
    //                        // Сервер подтвердил закрытие
    //                        await _webSocket.CloseAsync(
    //                            result.CloseStatus ?? WebSocketCloseStatus.NormalClosure,
    //                            result.CloseStatusDescription ?? "Подтверждение закрытия",
    //                            CancellationToken.None
    //                        );
    //                        break;
    //                    }
    //                }
    //                catch (OperationCanceledException) { } // Таймаут
    //                catch (WebSocketException ex)
    //                {
    //                    LOGGER.LogError($"Ошибка при ожидании закрытия: {ex.Message}");
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        LOGGER.LogError($"Ошибка при отключении: {ex.Message}");
    //    }
    //    finally
    //    {
    //        _webSocket.Dispose();
    //        LOGGER.LogInfo("Соединение закрыто");
    //    }
    //}

}
