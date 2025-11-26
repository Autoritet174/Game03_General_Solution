using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.WebSocketClient;

public interface IWebSocketClient
{
    Task ConnectAsync(CancellationToken cancellationToken);
    bool Connected { get; }
    Task DisconnectAsync();
}
