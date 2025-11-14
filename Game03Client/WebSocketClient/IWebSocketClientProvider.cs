using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game03Client.WebSocketClient;
public interface IWebSocketClientProvider
{
    Task ConnectAsync();
    bool Connected { get; }
}
