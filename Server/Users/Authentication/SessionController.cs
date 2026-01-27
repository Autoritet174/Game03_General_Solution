using General.DTO.RestRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using Server.Jwt_NS;
using System.Security;

namespace Server.Users.Authentication;

public sealed class SessionController(SessionService sessionService, JwtService jwtService) : ControllerBaseApi
{
    /// <summary>
    /// Эндпоинт для обновления токенов. 
    /// Доступен без [authorize], так как access token уже может быть просрочен.
    /// </summary>
    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] DtoRequestAuthReg dto)
    {
        try
        {
            (Guid? userId, string? newRefreshToken, DateTimeOffset? dtExpiration) = await sessionService.RefreshSessionAsync(dto);
            if (userId == null || string.IsNullOrWhiteSpace(newRefreshToken) || dtExpiration == null)
            {
                return Unauthorized(AuthRegResponse.InvalidCredentials());
            }
            string newAccessToken = jwtService.GenerateToken(userId.Value);

            return Ok(AuthRegResponse.Success(newAccessToken, newRefreshToken, dtExpiration.Value));
        }
        catch (SecurityException)
        {
            return Unauthorized(AuthRegResponse.InvalidCredentials());
        }
    }

    /// <summary>
    /// Выход из системы.
    /// Принимает refresh token в теле запроса, чтобы знать, какую именно сессию закрыть.
    /// </summary>
    [HttpPost("logout"), Authorize]
    public async Task<IActionResult> Logout([FromBody] DtoRequestLogout request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest("Refresh token is required.");
        }

        await sessionService.LogoutAsync(request.RefreshToken);

        return Ok();
    }
}
