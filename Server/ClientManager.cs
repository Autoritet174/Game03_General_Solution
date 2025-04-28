using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Server;

/// <summary>
/// Управляет всеми подключёнными клиентами.
/// </summary>
public class ClientManager {

    /// <summary>
    /// Все клиенты у которых есть хотя бы один открытый веб сокет
    /// </summary>
    private readonly ConcurrentDictionary<string, ConnectedClient> _clients = new();
    private readonly Timer _broadcastTimer;
    private readonly Random _random = new();

    /// <summary>
    /// Конструктор и запуск фоновой задачи.
    /// </summary>
    public ClientManager() {
        // Таймер каждые 5 секунд вызывает BroadcastRandomMessage
        _broadcastTimer = new Timer(_ => BroadcastRandomMessage(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Добавляет WebSocket клиенту.
    /// </summary>
    public void AddSocket(string userId, WebSocket socket) {
        ConnectedClient client = _clients.GetOrAdd(userId, _ => new ConnectedClient(userId));
        client.AddSocket("name_01",socket);

        Console.WriteLine($"[+] Подключился пользователь '{userId}', всего клиентов: {_clients.Count}");
    }

    /// <summary>
    /// Удаляет WebSocket клиента.
    /// </summary>
    public void RemoveSocket(string userId, WebSocket socket) {
        if (_clients.TryGetValue(userId, out ConnectedClient? client)) {
            client.RemoveSocket(socket);
            Console.WriteLine($"[-] Отключён WebSocket от '{userId}'");

            // Удаляем клиента, если у него не осталось соединений
            if (!client.GetSockets().Any()) {
                _ = _clients.TryRemove(userId, out _);
                Console.WriteLine($"[x] Клиент '{userId}' полностью отключён, всего клиентов: {_clients.Count}");
            }
        }
    }

    /// <summary>
    /// Рассылка случайного сообщения через случайный WebSocket.
    /// </summary>
    private void BroadcastRandomMessage() {
        List<ConnectedClient> allClients = [.. _clients.Values];
        if (allClients.Count == 0) {
            return;
        }

        // Случайный клиент
        //var client = allClients[_random.Next(allClients.Count)];
        foreach (ConnectedClient? client in allClients) {


            List<WebSocket> sockets = [.. client.GetSockets()];
            if (sockets.Count == 0) {
                continue;
            }

            // Случайный сокет этого клиента
            int indexSocket = _random.Next(sockets.Count);
            WebSocket socket = sockets[indexSocket];

            string message = $"{DateTime.Now:yyyy.MM.dd HH:mm:ss.fffffff}: ws={indexSocket}; test message";
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            try {
                socket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None).Wait();

                Console.WriteLine($"[>] Сервер → '{client.UserId}': {message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"[!] Ошибка отправки для '{client.UserId}': {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Обрабатывает сообщение от клиента.
    /// </summary>
    public void HandleClientMessage(string userId, string message) {
        Console.WriteLine($"[<] Клиент '{userId}': {message}");
    }

    /// <summary>
    /// Возвращает всех клиентов.
    /// </summary>
    public IEnumerable<ConnectedClient> GetAllClients() {
        return _clients.Values;
    }
}
