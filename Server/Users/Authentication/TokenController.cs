using General.DTO.RestRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;
using Server.Jwt_NS;
using System.Security;

namespace Server.Users.Authentication;

public sealed class TokenController(SessionService sessionService, JwtService jwtService) : ControllerBaseApi
{
    /// <summary>
    /// ЭНДПОИНТ ДЛЯ ОБНОВЛЕНИЯ ТОКЕНОВ. 
    /// ДОСТУПЕН БЕЗ [Authorize], ТАК КАК ACCESS TOKEN УЖЕ МОЖЕТ БЫТЬ ПРОСРОЧЕН.
    /// </summary>
    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        try
        {
            (Guid userId, string? newRefreshToken) = await sessionService.RefreshSessionAsync(refreshToken);

            string newAccessToken = jwtService.GenerateToken(userId);

            return Ok(AuthRegResponse.Success(newAccessToken, newRefreshToken));
        }
        catch (SecurityException)
        {
            return Unauthorized(AuthRegResponse.InvalidCredentials());
        }
    }

    /// <summary>
    /// ВЫХОД ИЗ СИСТЕМЫ.
    /// ПРИНИМАЕТ REFRESH TOKEN В ТЕЛЕ ЗАПРОСА, ЧТОБЫ ЗНАТЬ, КАКУЮ ИМЕННО СЕССИЮ ЗАКРЫТЬ.
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
