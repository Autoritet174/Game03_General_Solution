using System.Collections.Concurrent;

namespace Server.Hubs;

public class ClientManager
{
    private readonly ConcurrentDictionary<string, Client> clients = new();

    public bool TryAdd(string connectionId, Client client) =>
        clients.TryAdd(connectionId, client);

    public bool TryRemove(string connectionId, out Client? client) =>
        clients.TryRemove(connectionId, out client);

    public Client? Get(string connectionId) =>
        clients.TryGetValue(connectionId, out Client? client) ? client : null;

    public int count => clients.Count;
}
