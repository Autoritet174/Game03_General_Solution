using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.UserRegAuth_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
/// <remarks>
/// Конструктор контроллера.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class OpenWebSocketController() : ControllerBase
{

    /// <summary>
    /// Выполняет аутентификацию и возвращает JWT токен.
    /// </summary>
    /// <returns>JWT токен при успешной аутентификации или ошибка 401.</returns>
    [HttpPost("OpenWebSocket")]
    [AllowAnonymous]
    public IActionResult Login()
    {

        return Ok();
    }
}
