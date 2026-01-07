using General;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Http_NS.Controllers_NS;
using Server.Utilities;
using System.Text.Json.Nodes;

namespace Server.Users.Authentication;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthenticationController(AuthService _authService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Login([FromBody] DtoRequestAuthReg dtoRequest)
    {
        // Передаем данные в сервис. 
        // Если сервису все еще нужен JsonObject, его лучше формировать внутри сервиса или передавать DTO.
        DtoResponseAuthReg dtoResult = await _authService.LoginAsync(dtoRequest, HttpContext.Connection.RemoteIpAddress);
        string jsonResult = JsonConvert.SerializeObject(dtoResult, General.GlobalHelper.JsonSerializerSettings);
        return dtoResult.ErrorKey == null ? Ok(jsonResult) : BadRequest(jsonResult);
    }
}
