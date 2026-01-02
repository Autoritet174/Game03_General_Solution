using General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using Server.Utilities;
using System.Text.Json.Nodes;

namespace Server.Users.Reg;

/// <summary>
/// Контроллер регистрации пользователей. Обрабатывает процесс регистрации по email и паролю.
/// При успешной регистрации возвращает JWT-токен доступа.
/// </summary>
public class RegController(RegService _regService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Register()
    {
        JsonObject? json = await JsonObjectExtension.GetJsonObjectFromRequest(Request);

        if (json == null)
        {
            return BadRequestInvalidResponse();
        }

        string email = json.GetString("email");
        string password = json.GetString("password", true);

        AuthRegResponse result = await _regService.RegisterAsync(email, password, json, HttpContext.Connection.RemoteIpAddress);

        return result.Success
            ? Ok(new { token = result.Token })
            : BadRequest(new { result.ErrorKey, result.Extra });
    }
}
