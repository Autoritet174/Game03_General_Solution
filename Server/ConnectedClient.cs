using System.Collections.Concurrent;
using System.Net.WebSockets;

/// <summary>
/// Хранит список WebSocket-соединений одного клиента.
/// </summary>
public class ConnectedClient {
    public string UserId { get; }

    /// <summary>
    /// Потокобезопасный список WebSocket-соединений.
    /// </summary>
    private readonly ConcurrentBag<WebSocket> _sockets = new();

    public ConnectedClient(string userId) {
        UserId = userId;
    }

    /// <summary>
    /// Добавляет WebSocket.
    /// </summary>
    public void AddSocket(WebSocket socket) {
        _sockets.Add(socket);
    }

    /// <summary>
    /// Удаляет WebSocket.
    /// </summary>
    public void RemoveSocket(WebSocket socket) {
        // ConcurrentBag не поддерживает удаление напрямую — создаём новый список
        var sockets = _sockets.ToArray().Where(s => s != socket).ToList();
        _sockets.Clear();
        foreach (var s in sockets) _sockets.Add(s);
    }

    /// <summary>
    /// Возвращает все активные WebSocket'ы клиента.
    /// </summary>
    public IEnumerable<WebSocket> GetSockets() => _sockets.Where(s => s.State == WebSocketState.Open);
}
