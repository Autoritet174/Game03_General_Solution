using General;
using General.DTO.RestRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Http_NS.Controllers_NS;
using Server.Utilities;
using System.Text.Json.Nodes;

namespace Server.Users.Registration;

/// <summary>
/// Контроллер регистрации пользователей. Обрабатывает процесс регистрации по email и паролю.
/// При успешной регистрации возвращает JWT-токен доступа.
/// </summary>
public class RegistrationController(RegistrationService _regService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Register([FromBody] DtoRequestAuthReg dtoRequest)
    {
        var dtoResult = await _regService.RegisterAsync(dtoRequest, HttpContext.Connection.RemoteIpAddress);

        string jsonResult = JsonConvert.SerializeObject(dtoResult, General.GlobalHelper.JsonSerializerSettings);
        return dtoResult.ErrorKey == null ? Ok(jsonResult) : BadRequest(jsonResult);
    }
}
