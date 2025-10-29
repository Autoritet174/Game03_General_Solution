internal class Program
{
    private static async Task Main()
    {
        Console.WriteLine("WebSocket Test Client");

        var serverUrl = "ws://localhost:5001/ws/";
        var clientCount = 1;

        // Переменные для статистики
        var connectedClients = 0;
        var totalMessagesSent = 0;
        DateTime startTime = DateTime.Now;

        // Запускаем отображение статистики
        var statsTask = Task.Run(async () =>
        {
            while (true)
            {
                TimeSpan elapsed = DateTime.Now - startTime;
                Console.Title = $"Clients: {connectedClients}/{clientCount} | Messages: {totalMessagesSent} | Time: {elapsed:mm\\:ss}";
                await Task.Delay(1000);
            }
        });

        List<Task> clientTasks = [];
        List<WebSocketClient> clients = [];

        try
        {
            for (var i = 0; i < clientCount; i++)
            {
                var clientId = i + 1;
                WebSocketClient client = new(serverUrl);
                clients.Add(client);

                try
                {
                    // Последовательно подключаем
                    await client.ConnectAsync();
                    _ = Interlocked.Increment(ref connectedClients);

                    Console.WriteLine($"Клиент {clientId} подключен ({connectedClients}/{clientCount})");

                    // Запускаем отправку сообщений в отдельной задаче
                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            await client.StartSendingMessages(client.GetOptions(), (msgCount) =>
                            {
                                _ = Interlocked.Add(ref totalMessagesSent, msgCount);
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Клиент {clientId} ошибка при отправке: {ex.Message}");
                        }
                    });

                    clientTasks.Add(task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка подключения клиента {clientId}: {ex.Message}");
                }

                // Задержка между подключениями
                //await Task.Delay(10);
            }

            Console.WriteLine($"Все клиенты подключены. Ожидание завершения...");

            // Ждем завершения всех задач или нажатия клавиши
            var completionTask = Task.WhenAll(clientTasks);
            Task keyPressTask = WaitForKeyPress();

            _ = await Task.WhenAny(completionTask, keyPressTask);

            if (keyPressTask.IsCompleted)
            {
                Console.WriteLine("Прерывание по запросу пользователя...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
        finally
        {
            // Корректное отключение всех клиентов
            Console.WriteLine("Отключение клиентов...");
            await DisconnectAllClients(clients);

            TimeSpan elapsed = DateTime.Now - startTime;
            Console.WriteLine($"Тест завершен. Время: {elapsed:mm\\:ss}");
            Console.WriteLine($"Подключено: {connectedClients}, Отправлено сообщений: {totalMessagesSent}");
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        _ = Console.ReadKey();
    }

    private static async Task WaitForKeyPress()
    {
        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                break;
            }
            await Task.Delay(100);
        }
    }

    private static async Task DisconnectAllClients(List<WebSocketClient> clients)
    {
        const int batchSize = 100;
        var disconnectedCount = 0;

        for (var i = 0; i < clients.Count; i += batchSize)
        {
            WebSocketClient[] batch = clients.Skip(i).Take(batchSize).ToArray();
            IEnumerable<Task> disconnectTasks = batch.Select(client =>
                Task.Run(async () =>
                {
                    try
                    {
                        await client.DisconnectAsync();
                        _ = Interlocked.Increment(ref disconnectedCount);
                    }
                    catch
                    {
                        // Игнорируем ошибки отключения
                    }
                })
            );

            await Task.WhenAll(disconnectTasks);
            Console.WriteLine($"Отключено {disconnectedCount}/{clients.Count} клиентов");
        }
    }
}
