using Game03Client.JwtToken;
using General;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.WebSocketClient;

public class WebSocketClientProvider(JwtTokenProvider jwtToken) 
{
    private const string SERVER_URL = "wss://localhost:7227/ws/";
    private ClientWebSocket _webSocket = new();
    private readonly Uri _serverUri = new(SERVER_URL);
    private bool _isReceiving = false;
    public bool Connected { get; private set; } = false;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            Connected = false;
            _webSocket = new();
            if (!jwtToken.token.IsEmpty())
            {
                // Добавляем JWT токен в заголовки, если он предоставлен
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {jwtToken.token}");
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
    private async Task ReceiveMessagesAsync()
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
                //Console.WriteLine($"Получено: {message}");
                //ProcessReceivedMessage(message);
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            Console.WriteLine("Соединение закрыто сервером");
            //OnAuthenticationResult?.Invoke(false, "Соединение закрыто сервером");
        }
        catch (Exception ex)
        {
            if (_isReceiving) // Логируем только если не было запланированного отключения
            {
                Console.WriteLine($"Ошибка приема: {ex.Message}");
                //OnAuthenticationResult?.Invoke(false, $"Ошибка приема: {ex.Message}");
            }
        }
    }


    /// <summary>
    /// Отправить сообщение на сервер.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendMessageAsync(string message)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            Console.WriteLine("WebSocket не подключен");
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
            Console.WriteLine("Отправка отменена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка отправки: {ex.Message}");
        }
    }

    public async Task StartSendingMessages(Action<int>? onMessagesSent = null)
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

                // Просто сериализуйте - Newtonsoft.Json по умолчанию не экранирует Unicode
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                Console.WriteLine(json);
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

    public async Task DisconnectAsync()
    {
        try
        {
            _isReceiving = false; // Останавливаем прием сообщений
            _cancellationTokenSource.Cancel(); // Отменяем операции отправки

            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Закрытие клиентом",
                    CancellationToken.None // Не используем _cancellationTokenSource.Token здесь
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отключении: {ex.Message}");
        }
        finally
        {
            _webSocket.Dispose();
            Console.WriteLine("Отключено");
        }
    }

}
