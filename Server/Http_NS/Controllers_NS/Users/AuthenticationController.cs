// Server/Http_NS/Controllers_NS/Users/AuthenticationController.cs
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
using System.Net.Mail;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер аутентификации пользователей.
/// Обрабатывает запросы на вход в систему (аутентификацию) по email и паролю.
/// Реализует защиту от брутфорса, безопасное логирование и единое сообщение об ошибке.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(UserRepository userRepository, JwtService jwtService, ILogger<AuthenticationController> logger) : ControllerBaseApi
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly JwtService _jwtService = jwtService;
    private readonly ILogger<AuthenticationController> _logger = logger;

    /// <summary>
    /// Обрабатывает HTTP-запрос на аутентификацию пользователя.
    /// Возвращает JWT-токен при успехе или обобщённую ошибку при неудаче.
    /// </summary>
    /// <remarks>
    /// - Доступ без аутентификации.
    /// - Защита от брутфорса: rate limiting (настраивается вне метода).
    /// - Все ошибки возвращаются единым сообщением для предотвращения enumeration-атак.
    /// </remarks>
    /// <returns>
    /// - 200 OK с JWT-токеном при успехе.
    /// - 400 Bad Request с обобщённым сообщением при любой ошибке.
    /// - 415 Unsupported Media Type, если Content-Type не JSON.
    /// </returns>
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost]
    public async Task<IActionResult> Main()
    {
        _logger.LogError("blablabla");
        // Если пользователь уже авторизован, то не даем ему авторизоваться ещё раз.
        if (User.Identity?.IsAuthenticated == true)
        {
            return StatusCode(403, new { code = (long)SR.AuthAlreadyAuthenticated });
        }

        // Проверка Content-Type
        if (Request.ContentType?.StartsWith("application/json") != true)
        {
            return BadRequestUnsupportedMediaType();
        }

        // Попытка прочитать JSON из тела запроса
        JsonObject? obj = await JsonObjectExt.GetJsonObjectFromRequest(Request);
        if (obj == null)
        {
            return BadRequestAuthInvalidCredentials();
        }

        // Извлечение данных
        string email = JsonObjectHelper.GetString(obj, "email").Trim();
        string password = JsonObjectHelper.GetString(obj, "password");

        // Валидация длины и формата
        if (!IsValidEmail(email))
        {
            return BadRequestAuthInvalidCredentials();
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length > Server_Common.Consts.PASSWORD_MAX_LENGTH)
        {
            return BadRequestAuthInvalidCredentials();
        }

        // Поиск пользователя
        User? user = await _userRepository.GetByEmailAsync(email);
        if (user == null) {
            return BadRequestAuthInvalidCredentials();
        }

        bool isValid = user.PasswordHash != null && PassHasher.Verify(email, user.PasswordHash, password);

        if (!isValid)
        {
            // Единое сообщение для всех случаев неудачи
            return BadRequestAuthInvalidCredentials();
        }

        // Генерация JWT-токена
        string token = _jwtService.GenerateToken(user.Id);

        // Логирование (с маскировкой пароля)
        try
        {
            NpgsqlInet? inet = GetClientIpAddress();
            await Server_DB_Users.Sql.UsersLogger.WriteLog(
                obj: obj,
                user_id: user.Id,
                email: email,
                ip_address: inet ?? new NpgsqlInet("0.0.0.0"),
                authorizationSuccess: true,
                logger: logger);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write login success log for user {UserId} (email: {Email})", user.Id, email);
        }

        return Ok(new { token });
    }

    /// <summary>
    /// Проверяет, является ли строка валидным email-адресом.
    /// </summary>
    /// <param name="email">Email для проверки</param>
    /// <returns>true, если email валиден</returns>
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Length > Server_Common.Consts.EMAIL_MAX_LENGTH)
        {
            return false;
        }

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Извлекает IP-адрес клиента из X-Forwarded-For или RemoteIpAddress.
    /// Использует <see cref="IPAddress.TryParse"/> — так как NpgsqlInet не имеет TryParse.
    /// </summary>
    private NpgsqlInet? GetClientIpAddress()
    {
        string? ip = null;

        // Fallback: прямое подключение
        ip ??= HttpContext.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ip))
        {
            return null;
        }

        // Парсим через .NET IPAddress, затем оборачиваем в NpgsqlInet
        return IPAddress.TryParse(ip, out IPAddress? ipAddress) ? new NpgsqlInet(ipAddress) : (NpgsqlInet?)null;
    }
}
