using General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using Server.Utilities;
using System.Text.Json.Nodes;

namespace Server.Users.Auth;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthController(AuthService _authService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Login()
    {
        JsonObject? json = await JsonObjectExtension.GetJsonObjectFromRequest(Request);

        if (json == null)
        {
            return BadRequestInvalidResponse();
        }

        string email = json.GetString("email");
        string password = json.GetString("password", true);

        AuthRegResponse result = await _authService.LoginAsync(email,password,json,HttpContext.Connection.RemoteIpAddress);

        return result.Success
            ? Ok(new { token = result.Token })
            : BadRequest(new { result.ErrorKey, result.Extra });
    }

}
