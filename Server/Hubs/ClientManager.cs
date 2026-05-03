using System.Collections.Concurrent;

namespace Server.Hubs;

public class ClientManager
{
    private readonly ConcurrentDictionary<string, Client> _clients = new();

    public bool TryAdd(string connectionId, Client client) =>
        _clients.TryAdd(connectionId, client);

    public bool TryRemove(string connectionId, out Client? client) =>
        _clients.TryRemove(connectionId, out client);

    public Client? Get(string connectionId) =>
        _clients.TryGetValue(connectionId, out Client? client) ? client : null;

    public int Count => _clients.Count;
}
