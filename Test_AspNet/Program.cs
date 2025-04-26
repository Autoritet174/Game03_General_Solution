using System;
using System.Collections.Concurrent;
using System.Net;
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

public class WebSocketServer {
    private static readonly ConcurrentDictionary<WebSocket, string> clients = new();
    private static readonly Random random = new();
    private static readonly string[] messageTemplates = {
        "Серверное сообщение {0}",
        "Данные сервера {0}",
        "Информация от сервера {0}",
        "Случайный текст {0}"
    };

    public static async Task Main(string[] args) {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        Console.WriteLine("WebSocket сервер запущен на ws://localhost:8080/");

        var restTask = Task.Run(() => StartRestApi());
        var wsTask = Task.Run(() => StartWebSocketServer(listener));
        var broadcastTask = Task.Run(() => BroadcastRandomMessages());

        await Task.WhenAll(restTask, wsTask, broadcastTask);
    }

    static async Task StartRestApi() {
        var restListener = new HttpListener();
        restListener.Prefixes.Add("http://localhost:8081/");
        restListener.Start();
        Console.WriteLine("REST API доступен на http://localhost:8081/api/");

        while (true) {
            var context = await restListener.GetContextAsync();
            if (context.Request.Url.PathAndQuery == "/api/status" && context.Request.HttpMethod == "GET") {
                var response = new {
                    Status = "OK",
                    ActiveConnections = clients.Count,
                    ClientNames = clients.Values
                };
                var json = JsonSerializer.Serialize(response);

                context.Response.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                await context.Response.OutputStream.WriteAsync(buffer);
                context.Response.Close();
            }
            else {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }
    }

    static async Task StartWebSocketServer(HttpListener listener) {
        while (true) {
            var context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest) {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                var webSocket = webSocketContext.WebSocket;

                var clientName = context.Request.QueryString["name"] ?? $"Client-{Guid.NewGuid().ToString()[..4]}";

                if (clients.TryAdd(webSocket, clientName)) {
                    Console.WriteLine($"Новое подключение: {clientName} (Всего подключений: {clients.Count})");
                    _ = HandleClientConnection(webSocket, clientName);
                }
            }
            else {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    static async Task HandleClientConnection(WebSocket socket, string clientName) {
        var buffer = new byte[1024 * 4];
        try {
            while (socket.State == WebSocketState.Open) {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text) {
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = JsonSerializer.Deserialize<Message>(json);
                    Console.WriteLine($"Получено от {clientName}: {message.Content}");

                    await BroadcastMessage(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close) {
                    await CloseConnection(socket, clientName, "Закрыто клиентом");
                    break;
                }
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely) {
            Console.WriteLine($"Клиент {clientName} неожиданно отключился");
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка с клиентом {clientName}: {ex.Message}");
        }
        finally {
            await CloseConnection(socket, clientName, "Ошибка соединения");
        }
    }

    static async Task CloseConnection(WebSocket socket, string clientName, string reason) {
        try {
            if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived) {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    reason,
                    CancellationToken.None);
            }
        }
        catch {
            // Игнорируем ошибки при закрытии
        }
        finally {
            clients.TryRemove(socket, out _);
            Console.WriteLine($"Отключение: {clientName} (Причина: {reason}, Осталось подключений: {clients.Count})");
        }
    }

    static async Task BroadcastMessage(Message message) {
        var json = JsonSerializer.Serialize(message);
        var buffer = Encoding.UTF8.GetBytes(json);

        foreach (var client in clients.Keys) {
            if (client.State == WebSocketState.Open) {
                try {
                    await client.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
                catch (Exception ex) {
                    Console.WriteLine($"Ошибка при отправке клиенту: {ex.Message}");
                    await CloseConnection(client, clients[client], "Ошибка отправки");
                }
            }
        }
    }

    static async Task BroadcastRandomMessages() {
        while (true) {
            await Task.Delay(random.Next(3000, 8000));

            if (clients.IsEmpty)
                continue;

            var message = new Message {
                Sender = "Server",
                Content = string.Format(messageTemplates[random.Next(messageTemplates.Length)], random.Next(100)),
                Timestamp = DateTime.Now
            };

            Console.WriteLine($"Сервер рассылает: {message.Content}");
            await BroadcastMessage(message);
        }
    }
}