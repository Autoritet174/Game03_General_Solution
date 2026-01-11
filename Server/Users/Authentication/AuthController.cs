using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Http_NS.Controllers_NS;

namespace Server.Users.Authentication;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthController(AuthService _authService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Login([FromBody] DtoRequestAuthReg dtoRequest)
    {
        DtoResponseAuthReg dtoResult = await _authService.LoginAsync(dtoRequest, HttpContext.Connection.RemoteIpAddress);
        string jsonResult = JsonConvert.SerializeObject(dtoResult, General.GlobalHelper.JsonSerializerSettings);
        return dtoResult.ErrorKey == null ? Ok(jsonResult) : BadRequest(jsonResult);
    }
}
