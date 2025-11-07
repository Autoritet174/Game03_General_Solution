using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Newtonsoft.Json;

public class WebSocketClient
{
    private ClientWebSocket _webSocket;
    private Uri _serverUri;
    private CancellationTokenSource _cts;
    private bool _isReceiving = false;

    public WebSocketClient(string serverUrl)
    {
        _webSocket = new ClientWebSocket();
        _serverUri = new Uri(serverUrl);
        _cts = new CancellationTokenSource();
    }

    public async Task ConnectAsync()
    {
        bool connected = false;

        for (int i = 0; i < 3; i++)
        {
            try
            {
                //Console.WriteLine($"Подключение к {_serverUri}...");
                await _webSocket.ConnectAsync(_serverUri, CancellationToken.None);
                //Console.WriteLine("Подключение установлено!");

                // Запускаем прием сообщений без привязки к _cts.Token
                _isReceiving = true;
                _ = Task.Run(ReceiveMessagesAsync);
                connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                throw;
            }
            if (connected) { 
                break;
            }
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[4096];

        try
        {
            while (_isReceiving && _webSocket.State == WebSocketState.Open)
            {
                // Используем CancellationToken.None вместо _cts.Token
                var result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                //Console.WriteLine($"Получено: {message}");
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            Console.WriteLine("Соединение закрыто сервером");
        }
        catch (Exception ex)
        {
            if (_isReceiving) // Логируем только если не было запланированного отключения
            {
                Console.WriteLine($"Ошибка приема: {ex.Message}");
            }
        }
    }

    public async Task SendMessageAsync(string message)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            Console.WriteLine("WebSocket не подключен");
            return;
        }

        try
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                _cts.Token // Для отправки можно использовать _cts.Token
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

    public JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
    }

    public async Task StartSendingMessages(JsonSerializerOptions options, Action<int> onMessagesSent = null)
    {
        int messageCount = 0;

        while (!_cts.Token.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
        {
            //for (int i = 0; i < 1; i++)
            //{
            //    var message = $"Сообщение #{++messageCount} - {DateTime.Now:HH:mm:ss}";
            //    await SendMessageAsync(message);
            //}
            string com = Console.ReadLine();
            Random r = new Random();
            if (com == "1")
            {

                var data = new
                {
                    command = "AddItem",
                    item = new {
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

            await Task.Delay(10, _cts.Token);

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
            _cts.Cancel(); // Отменяем операции отправки

            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Закрытие клиентом",
                    CancellationToken.None // Не используем _cts.Token здесь
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
