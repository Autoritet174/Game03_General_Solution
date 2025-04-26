using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Server;

/// <summary>
/// Хранит список WebSocket-соединений одного клиента.
/// </summary>
public class ConnectedClient(string userId) {
    public string UserId { get; } = userId;

    /// <summary>
    /// Потокобезопасный список WebSocket-соединений.
    /// </summary>
    private readonly ConcurrentBag<WebSocket> _sockets = [];
    private readonly Lock _sockets_lock = new();

    /// <summary>
    /// Добавляет WebSocket.
    /// </summary>
    public void AddSocket(WebSocket socket) {
        lock (_sockets_lock) {
            _sockets.Add(socket);
        }
    }

    /// <summary>
    /// Удаляет WebSocket.
    /// </summary>
    public void RemoveSocket(WebSocket socket) {
        // Синхронизация доступа, если нужно обеспечить потокобезопасность
        lock (_sockets_lock) {
            WebSocket[] socketsArray = [.. _sockets];
            _sockets.Clear();

            // Фильтрация и добавление обратно
            foreach (WebSocket? s in socketsArray.Where(s => s != socket)) {
                _sockets.Add(s);
            }
        }
    }

    /// <summary>
    /// Возвращает все активные WebSocket'ы клиента.
    /// </summary>
    public IEnumerable<WebSocket> GetSockets() {
        return _sockets.Where(s => s.State == WebSocketState.Open);
    }
}
