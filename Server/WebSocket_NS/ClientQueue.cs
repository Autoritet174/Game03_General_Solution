using System.Net.WebSockets;
using System.Threading.Channels;

namespace Server.WebSocket_NS;

/// <summary>
/// Очередь WebSocket соединений для фоновой обработки.
/// </summary>
//public class ClientQueue
//{
//    private readonly Channel<WebSocket> _queue = Channel.CreateUnbounded<WebSocket>();

//    /// <summary>
//    /// Добавляет новый WebSocket в очередь.
//    /// </summary>
//    public void Enqueue(WebSocket socket)
//    {
//        _ = _queue.Writer.TryWrite(socket);
//    }

//    /// <summary>
//    /// Получает читатель очереди для BackgroundService.
//    /// </summary>
//    public ChannelReader<WebSocket> Reader => _queue.Reader;
//}