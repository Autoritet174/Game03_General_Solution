using General.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DB.Users.Entities;
using Server.DB.Users.Repositories;
using Server.Jwt_NS;
using Server.Utilities;
using SR = General.ServerErrors.Response;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
[AllowAnonymous]
[Route("api/[controller]/[action]")]
public class AuthenticationController(UserRepository userRepository, JwtService jwtService) : ControllerBaseApi
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly JwtService _jwtService = jwtService;

    /// <summary>
    /// Выполняет аутентификацию пользователя и возвращает JWT-токен.
    /// </summary>
    /// <param name="request">Данные для входа.</param>
    /// <returns>JWT-токен при успешной аутентификации или код ошибки.</returns>
    [HttpPost]
    public async Task<IActionResult> Authentication([FromBody] Login request)
    {
        await Task.Delay(2000); // Защита от брутфорса

        string email = request.Email?.Trim() ?? string.Empty;
        string password = request.Password?.Trim() ?? string.Empty;

        if (email == string.Empty || password == string.Empty)
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        User? user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null || !PassHasher.Verify(email, user.PasswordHash, password))
        {
            return CBA_BadRequest(SR.Auth_EmailAndPassword_NotFound);
        }

        string token = _jwtService.GenerateToken(user.Id);
        return Ok(new { token });
    }
}
