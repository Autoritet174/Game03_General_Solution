using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class Message {
    public string Sender { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}

public class WebSocketClient {
    private static readonly Random random = new();
    private static readonly string[] messageTemplates = {
        "Клиентское сообщение {0}",
        "Данные клиента {0}",
        "Информация от клиента {0}",
        "Текст сообщения {0}"
    };

    public static async Task Main(string[] args) {
        //Console.WriteLine("Введите имя клиента:");
        var clientName = $"CLIENT {Guid.NewGuid()}";

        using (var client = new ClientWebSocket()) {
            try {
                // Подключаемся с указанием имени в query string
                var uri = new Uri($"ws://localhost:8080/?name={Uri.EscapeDataString(clientName)}");
                await client.ConnectAsync(uri, CancellationToken.None);
                Console.WriteLine($"Подключено к серверу как {clientName}");

                var receiveTask = ReceiveMessages(client);
                var sendTask = SendRandomMessages(client, clientName);

                await Task.WhenAll(receiveTask, sendTask);
            }
            catch (Exception ex) {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    static async Task ReceiveMessages(ClientWebSocket client) {
        var buffer = new byte[1024 * 4];
        try {
            while (client.State == WebSocketState.Open) {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text) {
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = JsonSerializer.Deserialize<Message>(json);
                    Console.WriteLine($"[{message.Timestamp:HH:mm:ss}] {message.Sender}: {message.Content}");
                }
                else if (result.MessageType == WebSocketMessageType.Close) {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрыто сервером", CancellationToken.None);
                    Console.WriteLine("Сервер закрыл соединение");
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка при получении: {ex.Message}");
        }
    }

    static async Task SendRandomMessages(ClientWebSocket client, string clientName) {
        try {
            while (client.State == WebSocketState.Open) {
                await Task.Delay(random.Next(2000, 6000)); // 2-6 секунд между сообщениями

                var message = new Message {
                    Sender = clientName,
                    Content = string.Format(messageTemplates[random.Next(messageTemplates.Length)], random.Next(100)),
                    Timestamp = DateTime.Now
                };

                var json = JsonSerializer.Serialize(message);
                var buffer = Encoding.UTF8.GetBytes(json);

                await client.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);

                Console.WriteLine($"Отправлено: {message.Content}");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка при отправке: {ex.Message}");
        }
    }
}