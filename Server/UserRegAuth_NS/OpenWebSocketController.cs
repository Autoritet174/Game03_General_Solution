using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Jwt_NS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.UserAuth_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
/// <remarks>
/// Конструктор контроллера.
/// </remarks>
/// <param name="configuration">Конфигурация приложения для получения секретного ключа и параметров JWT.</param>
[ApiController]
[Route("api/[controller]")]
public class OpenWebSocketController() : ControllerBase
{

    /// <summary>
    /// Выполняет аутентификацию и возвращает JWT токен.
    /// </summary>
    /// <param name="request">Модель с именем пользователя и паролем.</param>
    /// <returns>JWT токен при успешной аутентификации или ошибка 401.</returns>
    [HttpPost("OpenWebSocket")]
    [AllowAnonymous]
    public IActionResult Login()
    {
       
        return Ok();
    }
}
