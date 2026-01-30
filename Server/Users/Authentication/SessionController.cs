using FluentResults;
using General.DTO.RestRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using Server.Jwt_NS;

namespace Server.Users.Authentication;

public sealed class SessionController(SessionService sessionService, JwtService jwtService) : ControllerBaseApi
{
    /// <summary>
    /// Эндпоинт для обновления токенов. 
    /// Доступен без [authorize], так как access token уже может быть просрочен.
    /// </summary>
    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> RefreshAsync([FromBody] DtoRequestAuthReg dto, CancellationToken cancellationToken)
    {
        Result<SessionResponseData> result = await sessionService.RefreshSessionAsync(dto, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            // Можно детализировать статус-коды на основе типа ошибки в result.Errors
            return Unauthorized(AuthRegResponse.InvalidCredentials());
        }

        SessionResponseData data = result.Value;
        string accessToken = jwtService.GenerateToken(data.UserId);

        return Ok(AuthRegResponse.Success(accessToken, data.RefreshToken, data.ExpiresAt));
    }

    /// <summary>
    /// Выход из системы.
    /// Принимает refresh token в теле запроса, чтобы знать, какую именно сессию закрыть.
    /// </summary>
    [HttpPost("logout"), Authorize]
    public async Task<IActionResult> LogoutAsync([FromBody] DtoRequestLogout request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest("Refresh token is required.");
        }

        _ = await sessionService.LogoutAsync(request.RefreshToken, cancellationToken).ConfigureAwait(false);

        return Ok();
    }
}
