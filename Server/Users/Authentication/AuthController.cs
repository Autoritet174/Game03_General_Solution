using FluentResults;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;

namespace Server.Users.Authentication;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthController(AuthService _authService) : ControllerBaseApi
{
    [AllowAnonymous, HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] DtoRequestAuthReg dtoRequest, CancellationToken cancellationToken)
    {
        // Используем FluentResults для получения результата
        Result<DtoResponseAuthReg> result = await _authService.LoginAsync(dtoRequest, HttpContext.Connection.RemoteIpAddress, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            // Обработка системных ошибок или BadRequest
            return result.HasError(e => e.Metadata.ContainsValue("BadRequest"))
                ? BadRequest(result.ToResult())
                : StatusCode(500);
        }

        DtoResponseAuthReg dtoResult = result.Value;

        // Вместо ручной сериализации в строку возвращаем объект. 
        // ASP.NET Core сам применит System.Text.Json (который быстрее Newtonsoft).
        return dtoResult.ErrorKey == null ? Ok(dtoResult) : BadRequest(dtoResult);
    }

    [HttpGet("validate")]
    public IActionResult Validate() => Ok();
}
