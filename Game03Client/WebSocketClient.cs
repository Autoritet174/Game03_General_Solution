using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

public class WebSocketClient
{
    private static readonly Logger<WebSocketClient> logger = new();
    private const string SERVER_URL = "wss://localhost:7227/ws/";
    private static ClientWebSocket _webSocket = new();
    private static readonly Uri _serverUri = new(SERVER_URL);
    private static bool _isReceiving = false;

    public static async Task<bool> ConnectAsync(CancellationToken cancellationTokenOpen, CancellationToken cancellationTokenReceive)
    {
        if (cancellationTokenOpen.IsCancellationRequested || cancellationTokenReceive.IsCancellationRequested)
        {
            logger.LogError("Подключение отменено");
            return false;
        }

        try
        {
            _webSocket = new();
            if (!string.IsNullOrWhiteSpace(Auth.AccessToken))
            {
                // Добавляем JWT токен в заголовки, если он предоставлен
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {Auth.AccessToken}");
            }
            await _webSocket.ConnectAsync(_serverUri, cancellationTokenOpen).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        if (_webSocket.State == WebSocketState.Open)
        {
            _isReceiving = true;
            _ = Task.Run(async () => { await ReceiveMessagesAsync(cancellationTokenReceive).ConfigureAwait(false); });
            return true;
        }

        logger.LogError("Не удалось подключиться к серверу WebSocket");
        return false;
    }

    /// <summary>
    /// Приём сообщений от сервера.
    /// </summary>
    /// <returns></returns>
    private static async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[4096];

        try
        {
            while (_isReceiving && _webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationToken
                ).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                logger.LogInfo($"Получено от сервера: {message}");
                //ProcessReceivedMessage(message);
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            logger.LogError("Соединение закрыто сервером");
        }
        catch (Exception ex)
        {
            if (_isReceiving) // Логируем только если не было запланированного отключения
            {
                logger.LogError($"Ошибка приема: {ex.Message}");
            }
        }
    }


    /// <summary>
    /// Отправить сообщение на сервер.
    /// </summary>
    /// <returns></returns>
    public static async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            logger.LogError("WebSocket не подключен");
            return;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            ).ConfigureAwait(false);
            //Console.WriteLine($"Отправлено: {message}");
        }
        catch (OperationCanceledException)
        {
            logger.LogError("Отправка отменена");
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка отправки: {ex.Message}");
        }
    }

    public static async Task StartSendingMessagesAsync(Action<int>? onMessagesSent, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
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
                logger.LogError(json);
                await SendMessageAsync(json, cancellationToken).ConfigureAwait(false);

            }


            // Сообщаем о количестве отправленных сообщений
            onMessagesSent?.Invoke(100);

            await Task.Delay(10, cancellationToken).ConfigureAwait(false);

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
            logger.LogInfo("Соединение закрыто");
        }
    }
}
