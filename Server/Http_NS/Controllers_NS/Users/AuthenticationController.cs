using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DB.Users.Entities;
using Server.DB.Users.Repositories;
using Server.Jwt_NS;
using Server.Utilities;
using System.Text;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
public class AuthenticationController(UserRepository userRepository, JwtService jwtService) : ControllerBaseApi
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly JwtService _jwtService = jwtService;

    /// <summary>
    /// Выполняет аутентификацию пользователя и возвращает JWT-токен.
    /// </summary>
    /// <param name="request">Данные для входа.</param>
    /// <returns>JWT-токен при успешной аутентификации или код ошибки.</returns>
    [AllowAnonymous]
    public async Task<IActionResult> Authentication()
    {
        await GF.DelayWithOutDebug();

        Request.EnableBuffering();

        using StreamReader reader = new(Request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        Request.Body.Position = 0;
        JsonNode? data = JsonNode.Parse(body);


        if (data is not JsonObject obj)
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        string email = JsonObjectHelper.FindValueIgnoreCase(obj, "email");
        string password = JsonObjectHelper.FindValueIgnoreCase(obj, "password");

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

        string? ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();


        return Ok(new { token });
    }
}
