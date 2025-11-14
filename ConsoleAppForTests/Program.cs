using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ConsoleAppForTests;

public sealed class UltraFastDiceRandom
{
    private uint _state;

    /// <summary>
    /// Инициализация генератора.
    /// </summary>
    /// <param name="seed">Начальный сид (должен быть ненулевым).</param>
    public UltraFastDiceRandom(uint seed = 1)
    {
        if (seed == 0) seed = 1;
        _state = seed;
    }

    /// <summary>
    /// Возвращает сумму count случайных целых в диапазоне [1, size].
    /// Максимально оптимизировано для скорости.
    /// </summary>
    /// <param name="count">Количество бросков.</param>
    /// <param name="size">Верхняя граница.</param>
    /// <returns>Сумма.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int GetRandomSum(int count, int size)
    {
        uint s = _state;
        long sum = 0;

        uint usize = (uint)size;
        for (int i = 0; i < count; i++)
        {
            // xorshift32 — 3 простейших операции
            s ^= s << 13;
            s ^= s >> 17;
            s ^= s << 5;

            // масштабирование: равномерное 0..size-1
            // (uint)(((ulong)s * usize) >> 32) — самое быстрое и корректное преобразование
            sum += (uint)(((ulong)s * usize) >> 32) + 1;
        }

        _state = s;

        if (sum > int.MaxValue)
            throw new OverflowException();

        return (int)sum;
    }
}

public sealed class UltraFastDiceRandom2
{
    private int _state;

    /// <summary>
    /// Инициализация генератора.
    /// </summary>
    /// <param name="seed">Начальный сид (должен быть ненулевым).</param>
    public UltraFastDiceRandom2(int seed = 1)
    {
        if (seed == 0) seed = 1;
        _state = seed;
    }

    /// <summary>
    /// Возвращает сумму count случайных целых в диапазоне [1, size].
    /// Максимально оптимизировано для скорости.
    /// </summary>
    /// <param name="count">Количество бросков.</param>
    /// <param name="size">Верхняя граница.</param>
    /// <returns>Сумма.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int GetRandomSum(int count, int size)
    {
        int s = _state;
        long sum = 0;

        int usize = size;
        for (int i = 0; i < count; i++)
        {
            // xorshift32 — 3 простейших операции
            s ^= s << 13;
            s ^= s >> 17;
            s ^= s << 5;

            // масштабирование: равномерное 0..size-1
            // (uint)(((ulong)s * usize) >> 32) — самое быстрое и корректное преобразование
            sum += (int)(((long)s * usize) >> 32) + 1;
        }

        _state = s;

        return (int)sum;
    }
}


internal class Program
{
    private static readonly UltraFastDiceRandom rand1 = new((uint)DateTime.Now.Ticks);
    private static readonly UltraFastDiceRandom2 rand2 = new();
    private static void Main()
    {
        //Game03Client.Game03 gameClient = Game03Client.Game03.Create(
        //    Path.Combine(@"c:\UnityProjects\Game03_Git\Client_Game03\Assets", @"GameData\Config\Main.ini"),
        //    new General.StringCapsule());
        //string qwe = gameClient.JwtToken.GetTokenAsync();
        //for (int i = 0; i < 20; i++)
        //{
        //    Console.WriteLine(r.GetRandomSum(1, 3));
        //}
        //return;

        for (int sim = 0; sim < 1000; sim++)
        {
            _ = GetRandomInt(10, 10);
        }
        int rand1Min = int.MaxValue, rand1Max = int.MinValue;

        int cube = 6;
        int rand1MinNeed = 10, rand1MaxNeed = rand1MinNeed * cube;
        int iter = 0;
        while (true)
        {
            int v = rand1.GetRandomSum(10, 6);
            if (rand1Min > v)
            {
                rand1Min = v;
            }
            if (rand1Max < v)
            {
                rand1Max = v;
            }
            iter++;
            if (rand1Min == rand1MinNeed && rand1Max == rand1MaxNeed)
            {
                break;
            }
        }
        Console.WriteLine(rand1Min.ToString() + " " + rand1Max.ToString());
        Console.WriteLine(iter.ToString());
        return;
        for (int sim = 0; sim < 1000; sim++)
        {
            //Console.Write(rand2.GetRandomSum(1, 5).ToString() + "; ");
        }


        Console.WriteLine($"Console log.");
        Console.WriteLine($"100000000 итераций для 3 вариантов.");

        DateTime start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = GetRandomInt(10, 10);
        }
        double var0 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"вариант GetRandomInt(10, 10) = {var0} секунд");




        start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = rand1.GetRandomSum(10, 10);
        }
        var var1 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"вариант GetRandomSum(10, 10) = {var1} секунд");
        //Console.WriteLine($"{(var0 / var1 - 1) * 100:0.00}%");




        start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = rand2.GetRandomSum(10, 10);
        }
        var var2 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"последний вариант = {var2} секунд");//
        //Console.WriteLine($"{(var0 / var2 - 1) * 100:0.00}%");



        Console.ReadLine();
    }

    private static readonly Random random = new();
    private static int GetRandomInt(int count, int size)//13.7
    {
        int result = 0;
        for (int i = 0; i < count; i++)
        {
            result += random.Next(size) + 1;
        }
        return result;
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
