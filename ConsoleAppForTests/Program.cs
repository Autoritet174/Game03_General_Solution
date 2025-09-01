internal class Program
{
    private static async Task Main()
    {
        int _connectedClients = 0;
        int _totalMessagesSent = 0;
        DateTime _startTime = default;

        Console.WriteLine("WebSocket Test Client");

        var serverUrl = "ws://localhost:5001/ws/";
        int clientCount = 100000;

        var tasks = new List<Task>();

        for (int i = 0; i < clientCount; i++)
        {
            var clientId = i + 1;
            tasks.Add(Task.Run(async () =>
            {
                var client = new WebSocketClient(serverUrl);

                try
                {
                    await client.ConnectAsync();
                    Console.WriteLine($"Клиент {clientId} подключен");

                    await client.StartSendingMessages();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Клиент {clientId} ошибка: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync();
                }
            }));

            await Task.Delay(1); // Задержка между подключениями
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}");
            }
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}