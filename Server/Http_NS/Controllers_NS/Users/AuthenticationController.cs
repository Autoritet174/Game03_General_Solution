using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using Server.DB;
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
        await GF.DelayWithOutDebug2000ms();//Добавляет задержку 2000 миллисекунд если работает без отладчика.

        Request.EnableBuffering();

        using StreamReader reader = new(Request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();
        if (body == null || body.Trim() == string.Empty)
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        Request.Body.Position = 0;
        JsonNode? data = null;
        try
        {
            data = JsonNode.Parse(body);
        }
        catch
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        if (data is not JsonObject obj)
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        string email = JsonObjectHelper.GetString(obj, "email");
        string password = JsonObjectHelper.GetString(obj, "password");

        if (email == string.Empty || password == string.Empty)
        {
            return CBA_BadRequest(SR.Auth_EmailOrPassword_Empty);
        }

        User? user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !PassHasher.Verify(email, user.PasswordHash, password))
        {
            return CBA_BadRequest(SR.Auth_EmailAndPassword_NotFound);
        }

        string token = _jwtService.GenerateToken(user.Id);

        try
        {
            NpgsqlInet inet = new("0.0.0.0");
            try
            {
                string? ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (ip != null)
                {
                    inet = new(ip);
                }
            }
            catch { }
            await DB.Users.Sql.UsersLogger.WriteLog(obj, user.Id, email, PassHasher.Create(email, password), inet, true);
        }
        catch { }


        return Ok(new { token });
    }
}
