using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal class Program {
    private static async Task Main(string[] args) {
        // 1. Аутентификация для получения токена
        HttpClient httpClient = new();
        var loginData = new { Username = "test", Password = "password" };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5000/api/auth/login", loginData);

        if (!response.IsSuccessStatusCode) {
            Console.WriteLine("Ошибка аутентификации");
            return;
        }

        dynamic? result = await response.Content.ReadFromJsonAsync<dynamic>();
        if (result == null) {
            return;
        }
        dynamic token = result.token.ToString();

        // 2. Подключение через WebSocket с токеном
        ClientWebSocket client = new();
        Uri uri = new($"ws://localhost:5000/ws?access_token={token}");

        await client.ConnectAsync(uri, CancellationToken.None);
        Console.WriteLine("Подключено к WebSocket с токеном");

        // Запускаем задачу для получения сообщений
        Task receiveTask = Task.Run(() => ReceiveMessages(client));

        // Отправляем сообщения
        while (true) {
            Console.Write("Введите сообщение (или 'exit' для выхода): ");
            string message = Console.ReadLine();

            if (message == "exit") {
                break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        await client.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "Закрыто клиентом",
            CancellationToken.None);

        await receiveTask;
    }

    private static async Task ReceiveMessages(ClientWebSocket client) {
        byte[] buffer = new byte[1024];
        try {
            while (client.State == WebSocketState.Open) {
                WebSocketReceiveResult result = await client.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text) {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Получено: {message}");
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Ошибка при получении: {ex.Message}");
        }
    }
}