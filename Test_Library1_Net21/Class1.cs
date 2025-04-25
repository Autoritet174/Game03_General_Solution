using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_Library1_Net21 {
    public static class Class1 {
        private static HttpListener? httpListener;
        private static WebSocket? webSocket;
        public static async void Start() {
            // Запуск HTTP сервера
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8080/");
            httpListener.Start();
            Console.WriteLine("HTTP сервер запущен на http://localhost:8080/");

            // Запуск WebSocket сервера в отдельном потоке
            Task webSocketTask = Task.Run(StartWebSocketServer);

            // Обработка HTTP запросов
            while (true) {
                HttpListenerContext context = await httpListener.GetContextAsync();
                _ = Task.Run(() => ProcessHttpRequest(context));
            }
        }

        private static async Task ProcessHttpRequest(HttpListenerContext context) {
            if (context.Request.HttpMethod == "GET" && context.Request.Url.PathAndQuery == "/api/hello") {
                // REST API endpoint
                string responseString = "{\"message\":\"Hello from REST API!\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.Close();
            }
            else {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }

        private static async Task StartWebSocketServer() {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8081/");
            listener.Start();
            Console.WriteLine("WebSocket сервер запущен на ws://localhost:8081/");

            while (true) {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest) {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    webSocket = webSocketContext.WebSocket;
                    Console.WriteLine("WebSocket соединение установлено");

                    await HandleWebSocketConnection(webSocket);
                }
                else {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private static async Task HandleWebSocketConnection(WebSocket socket) {
            byte[] buffer = new byte[1024];
            try {
                while (socket.State == WebSocketState.Open) {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text) {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Получено WebSocket сообщение: {message}");

                        string response = $"Echo: {message}";
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
                        await socket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close) {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрыто по запросу клиента", CancellationToken.None);
                        Console.WriteLine("WebSocket соединение закрыто");
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Ошибка WebSocket: {ex.Message}");
            }
        }
    }
}
