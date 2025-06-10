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
    //private readonly ConcurrentBag<WebSocket> _sockets = [];
    private WebSocket? _socket = null;
    private readonly Lock _sockets_lock = new();

    /// <summary>
    /// Добавляет WebSocket.
    /// </summary>
    public void AddSocket(WebSocket socket) {
        lock (_sockets_lock) {
            _socket = socket;
        }
    }

    /// <summary>
    /// Удаляет WebSocket.
    /// </summary>
    public void RemoveSocket(WebSocket socket) {
        // Синхронизация доступа, если нужно обеспечить потокобезопасность
        //lock (_sockets_lock) {
        //    //KeyValuePair<string, WebSocket>[] socketsArray = [.. _sockets];
        //    KeyValuePair<string, WebSocket> s = _sockets.FirstOrDefault(a => a.Value == socket1);
        //    string key = s.Key;
        //    try {

        //        _ = _sockets.TryRemove(s.Key, out _);
        //    }
        //    catch {

        //    }
        //    if (_sockets.TryRemove(key, out WebSocket? socket)) {
        //        try {
        //            // Проверяем, открыт ли сокет
        //            if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived) {
        //                // Отправляем корректное закрытие соединения
        //                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        //            }
        //        }
        //        catch {
        //            // Игнорируем ошибки при закрытии (например, если сокет уже оборван)
        //        }
        //        finally {
        //            // Освобождаем ресурсы сокета
        //            socket.Dispose();
        //        }
        //    }
        //    //_sockets.Clear();

        //    //// Фильтрация и добавление обратно
        //    //foreach (WebSocket? s in socketsArray.Where(s => s != socket)) {
        //    //    _sockets.Add(s);
        //    //}
        //}

        lock (_sockets_lock) {
            try {
                if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived) {
                    // Закрытие соединения
                    Task closeTask = socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    closeTask.Wait(); // Ждём завершения внутри lock
                }
            }
            catch (Exception ex) {
                // Логируем или игнорируем ошибку закрытия
                Console.WriteLine($"Ошибка при закрытии сокета: {ex.Message}");
            }
            finally {
                try {
                    socket.Dispose();
                }
                catch (Exception ex) {
                    // Логируем или игнорируем ошибку освобождения ресурсов
                    Console.WriteLine($"Ошибка при освобождении сокета: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Возвращает все активные WebSocket'ы клиента.
    /// </summary>
    public WebSocket GetSockets() {
        return _socket;
    }
}
