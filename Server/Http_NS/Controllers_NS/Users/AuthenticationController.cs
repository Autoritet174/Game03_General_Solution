using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NpgsqlTypes;
using Server.Jwt_NS;
using Server.Utilities;
using Server_DB;
using Server_DB_Users.Entities;
using Server_DB_Users.Repositories;
using System.Net;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает вход (логин) по email и паролю.
/// В случае успеха возвращает JWT-токен доступа.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(UserRepository userRepository, JwtService jwtService, ILogger<AuthenticationController> logger) : ControllerBaseApi
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly JwtService _jwtService = jwtService;
    private readonly ILogger<AuthenticationController> _logger = logger;

    /// <summary>
    /// Основной метод аутентификации: принимает email и пароль, проверяет учётные данные,
    /// при успехе возвращает JWT-токен. Доступ разрешён без авторизации.
    /// </summary>
    /// <returns>JWT-токен при успешной аутентификации или код ошибки.</returns>
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost]
    public async Task<IActionResult> Main()
    {

        // Проверка, что запрос содержит JSON.
        // Ожидается application/json, иначе — ошибка формата.
        if (Request.ContentType?.StartsWith("application/json") != true)
        {
            return BadRequestUnsupportedMediaType();
        }


        // Извлечение JSON из тела запроса.
        // Если тело пустое или не JSON — возвращаем ошибку.
        JsonObject? obj = await JsonObjectExt.GetJsonObjectFromRequest(Request);
        if (obj == null)
        {
            return BadRequestAuthInvalidCredentials();
        }


        // Извлечение и валидация email и пароля.
        // Trim() гарантирует отсутствие начальных/конечных пробелов у email.
        string email = JsonObjectHelper.GetString(obj, "email").Trim();
        if (!string.IsNullOrEmpty(email) && email.Length > Server_Common.Consts.EMAIL_MAX_LENGTH)
        {
            await LoggingAuthentification(false, obj, null, null);
            return BadRequestAuthInvalidCredentials();
        }
        // Проверка email: не пустой, не превышает длину, корректный формат.
        if (!General.Functions.IsEmail(email))
        {
            await LoggingAuthentification(false, obj, null, null);
            return BadRequestAuthInvalidCredentials();
        }


        string password = JsonObjectHelper.GetString(obj, "password");
        // Проверка пароля: не пустой, не слишком длинный.
        if (string.IsNullOrWhiteSpace(password) || password.Length > Server_Common.Consts.PASSWORD_MAX_LENGTH)
        {
            await LoggingAuthentification(false, obj, email, null);
            return BadRequestAuthInvalidCredentials();
        }


        // Поиск пользователя по email в базе.
        // Если пользователь не найден — всё равно возвращаем общую ошибку "неверные учётные данные".
        User? user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            await LoggingAuthentification(false, obj, email, null);
            return BadRequestAuthInvalidCredentials();
        }

        // Проверка хеша пароля с использованием email как соли.
        if (string.IsNullOrWhiteSpace(user.PasswordHash)) {
            // Делаем фиктивную проверку, чтобы не создавать timing-разницу
            PassHasher.Verify(email, "dummy", "dummy");// Режим параноика так как хеш пароля никогда не должен быть пустым
            return BadRequestAuthInvalidCredentials();
        }


        if (!PassHasher.Verify(email, user.PasswordHash, password))
        {   
            await LoggingAuthentification(false, obj, email, user.Id);
            return BadRequestAuthInvalidCredentials();
        }

        // Генерация JWT-токена на основе идентификатора пользователя.
        string token = _jwtService.GenerateToken(user.Id);

        // Логирование успешного входа.
        await LoggingAuthentification(true, obj, email, user.Id);

        // Возвращаем токен в формате JSON.
        return Ok(new { token });
    }

    private async Task LoggingAuthentification(bool authorizationSuccess, JsonObject obj, string? email, Guid? userId) {
        try
        {
            NpgsqlInet? inet = GetClientIpAddress();
            obj.Remove("password");
            await Server_DB_Users.Sql.UsersLogger.WriteLog(
                obj: obj,
                user_id: userId,
                email: email,
                ip_address: inet ?? new NpgsqlInet("0.0.0.0"),
                authorizationSuccess: authorizationSuccess,
                logger: _logger);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Не удалось записать лог успешного входа для пользователя {UserId} (email: {Email})", userId, email);
        }
    }

    /// <summary>
    /// Получает IP-адрес клиента из HTTP-запроса.
    /// </summary>
    /// <returns>IP-адрес в формате <see cref="NpgsqlInet"/> или null, если не удалось определить.</returns>
    private NpgsqlInet? GetClientIpAddress()
    {
        string? ip = null;

        // Пытаемся получить IP из подключения.
        ip ??= HttpContext.Connection.RemoteIpAddress?.ToString();

        return string.IsNullOrEmpty(ip)
            ? null
            : IPAddress.TryParse(ip, out IPAddress? ipAddress) ? new NpgsqlInet(ipAddress) : (NpgsqlInet?)null;
    }
}
