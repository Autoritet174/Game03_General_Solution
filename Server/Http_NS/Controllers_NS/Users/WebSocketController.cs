using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.WebSocket_NS;
using System.Net.WebSockets;
namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер для обработки подключений WebSocket.
/// </summary>
[ApiController]
[Route("ws")]
public class WebSocketController: ControllerBaseApi
{
    private readonly ClientQueue _queue;
    private readonly ILogger<WebSocketController> _logger;
    public WebSocketController(ClientQueue queue, ILogger<WebSocketController> logger)
    {
        _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        _logger = logger;
    }
    /// <summary>
    /// Принимает WebSocket-соединение и обрабатывает клиента в фоне.
    /// </summary>
    [HttpGet("connect")]
    [AllowAnonymous]
    public async Task Connect()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsync("Требуется WebSocket соединение");
            return;
        }

        // Перенаправляем на независимый WebSocket сервер
        HttpContext.Response.Redirect("http://localhost:5001/ws/");
        await HttpContext.Response.CompleteAsync();
    }
}