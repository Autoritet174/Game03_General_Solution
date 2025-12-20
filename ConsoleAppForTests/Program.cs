using Game03Client.DTO;
using Game03Client.PlayerCollection;

namespace ConsoleAppForTests;

internal class Program
{

    private static void Main()
    {

        //for (int i = 0; i < 50; i++)
        //{
        //    StringBuilder sb = new();
        //    foreach (byte item in Guid.NewGuid().ToByteArray())
        //    {
        //        sb.Append(item.ToString("000"));
        //        sb.Append('-');
        //    }
        //    Console.WriteLine(sb.ToString());
        //}
        //Console.ReadLine();
        //Console.WriteLine(nameof(General.Url.Collection.All));
       
        Start();
    }
    private static void Game_OnLog(object message)
    {
        Console.WriteLine("[Library: Game03Client] " + message);
    }
    private static void Start()
    {
        General.StringCapsule capsule = new()
        {
            Value = File.ReadAllText(@"C:\UnityProjects\Game03_Git\Client_Game03\Assets\Resources\localization\ru\data.json"),
        };

        var Game = Game03Client.Game03.Create(
            iniFileFullPath: Path.Combine(@"c:\UnityProjects\Game03_Git\Client_Game03\Assets", @"GameData\Config\Main.ini"),
            stringCapsuleJsonFileData: capsule, languageGame: Game03Client.GameLanguage.Ru, loggerCallback: Game_OnLog);


        CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(30));

        string json = """
            {"Email":"SUPERADMIN@MAIL.RU","Password":"testPassword","TimeZoneInfo_Local_BaseUtcOffset_Minutes":480,"System_Environment_UserName":"AUTORITET","DeviceModel":"B550 GAMING X V2 (Gigabyte Technology Co., Ltd.)","DeviceType":"Desktop","OperatingSystem":"Windows 11  (10.0.22000) 64bit","ProcessorType":"AMD Ryzen 7 3700X 8-Core Processor ","ProcessorCount":16,"SystemMemorySize":32691,"GraphicsDeviceName":"NVIDIA GeForce RTX 4070","GraphicsMemorySize":12011,"DeviceUniqueIdentifier":"e307f13fd5fb9d8c59a3a7b4df863c02bdbb300c","SystemInfo_supportsInstancing":true,"SystemInfo_npotSupport":"Full"}
            """;
        string? token = Game.JwtToken.GetTokenAsync(json, cancellationTokenSource.Token).Result;


        //Game03Client.WebSocketClient.IWebSocketClientProvider webSocketClient = Game.WebSocketClient;
        //cancellationTokenSource = new(TimeSpan.FromSeconds(30));
        //webSocketClient.ConnectAsync(cancellationTokenSource.Token).Wait();
        //if (!webSocketClient.Connected)
        //{

        //}

        cancellationTokenSource = new(TimeSpan.FromSeconds(30));
        Game.GameData.LoadGameData(cancellationTokenSource.Token).Wait();

        cancellationTokenSource = new(TimeSpan.FromSeconds(30));
        Game.Collection.LoadAllCollectionFromServer(cancellationTokenSource.Token).Wait();

        IEnumerable<DtoCollectionHero> coll = Game.Collection.GetCollectionHeroesFromCache();
        IEnumerable<GroupCollectionElement> coll1 = Game.Collection.GetCollectionHeroesGroupedByGroupNames();
        Console.WriteLine(coll.Count());

        //Console.ReadLine();
        //webSocketClient.DisconnectAsync().Wait();
        _ = Console.ReadLine();
    }


    public async void webSocketTest()
    {

        Console.WriteLine("WebSocket Test Client");

        string serverUrl = "ws://localhost:5001/ws/";
        int clientCount = 1;

        // Переменные для статистики
        int connectedClients = 0;
        int totalMessagesSent = 0;
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
            for (int i = 0; i < clientCount; i++)
            {
                int clientId = i + 1;
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
        int disconnectedCount = 0;

        for (int i = 0; i < clients.Count; i += batchSize)
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
