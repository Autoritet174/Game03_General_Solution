using General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Http_NS.Controllers_NS;
using Server.Http_NS.Controllers_NS.Users_NS;
using Server.Jwt_NS;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Text.Json.Nodes;
using L = General.LocalizationKeys;

namespace Server.Users.Auth;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthenticationController(AuthService _authService) : ControllerBaseApi
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        JsonObject? json = await JsonObjectExtension.GetJsonObjectFromRequest(Request);

        if (json == null)
        {
            return BadRequestInvalidResponse();
        }

        string email = json.GetString("email");
        string password = json.GetString("password", true);

        AuthResult result = await _authService.LoginAsync(
            email,
            password,
            json,
            HttpContext.Connection.RemoteIpAddress);

        return result.Success
            ? Ok(new { token = result.Token })
            : BadRequest(new { KEY_LOCALIZATION = result.ErrorKey, result.Extra });
    }

}
