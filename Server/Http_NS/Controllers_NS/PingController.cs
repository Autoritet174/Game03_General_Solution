using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер для проверки доступности сервера путем отправки тестового сигнала (ping).
/// </summary>
public class PingController : ControllerBase
{
    /// <summary>
    /// Возвращает сообщение "Pong!" для подтверждения доступности сервера.
    /// </summary>
    /// <returns>JSON-ответ с полем <see cref="string"/> message, содержащее значение "Pong!".</returns>
    [HttpGet("api/[controller]")]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok(new { message = "pong" });
    }
}
