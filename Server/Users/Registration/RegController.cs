using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using System.Text.Json;

namespace Server.Users.Registration;

/// <summary>
/// Контроллер регистрации пользователей. Обрабатывает процесс регистрации по email и паролю.
/// При успешной регистрации возвращает JWT-токен доступа.
/// </summary>
public class RegController(RegService _regService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Register([FromBody] DtoRequestAuthReg dtoRequest)
    {
        DtoResponseAuthReg dtoResult = await _regService.RegisterAsync(dtoRequest, HttpContext.Connection.RemoteIpAddress);

        string jsonResult = JsonSerializer.Serialize(dtoResult, GlobalJsonOptions.jsonOptions);
        return dtoResult.ErrorKey == null ? Ok(jsonResult) : BadRequest(jsonResult);
    }
}
