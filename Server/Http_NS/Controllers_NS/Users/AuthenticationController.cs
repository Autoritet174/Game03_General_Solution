using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using NpgsqlTypes;
using Server.Jwt_NS;
using Server.Utilities;
using Server_DB_Users.Entities;
using Server_DB_Users.Repositories;
using System.Net;
using System.Text.Json.Nodes;
using SR = General.ServerErrors.Error;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthenticationController(UserRepository userRepository, JwtService jwtService, ILogger<AuthenticationController> logger,
    IMemoryCache memoryCache, BackgroundLoggerAuthentificationService backgroundLoggerAuthentificationService) : ControllerBaseApi
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly JwtService _jwtService = jwtService;
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly BackgroundLoggerAuthentificationService _backgroundLoggerAuthentificationService = backgroundLoggerAuthentificationService;

    /// <summary>
    /// Время блокировки учётной записи при превышении допустимого количества неудачных попыток входа.
    /// </summary>
    private static readonly TimeSpan LockoutPeriod = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Основной метод аутентификации: принимает email и пароль, проверяет учётные данные,
    /// при успехе возвращает JWT-токен. Доступ разрешён без авторизации.
    /// </summary>
    /// <returns>JWT-токен при успешной аутентификации или соответствующий код ошибки.</returns>
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

        // Извлечение и валидация email.
        string email = JsonObjectExt.GetString(obj, "email");
        if (string.IsNullOrEmpty(email) || email.Length > Server_Common.Consts.EMAIL_MAX_LENGTH)
        {
            // Если пользователь временно заблокирован — возвращаем 429
            if (IsRateLimited(email))
            {
                return BadRequestEmailRateLimited(email);
            }

            // Логируем неудачную попытку входа
            LoggingAuthentification(false, obj, null, null);
            // Увеличиваем счётчик неудачных попыток
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Проверка формата email
        if (!General.Functions.IsEmail(email))
        {
            LoggingAuthentification(false, obj, email, null);
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Извлечение и валидация пароля
        string password = JsonObjectExt.GetString(obj, "password", removeAfterSuccessGetting: true);
        if (string.IsNullOrEmpty(password) || password.Length > Server_Common.Consts.PASSWORD_MAX_LENGTH)
        {
            LoggingAuthentification(false, obj, email, null);
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Поиск пользователя по email в базе данных
        User? user = await _userRepository.GetByEmailWithBansAsync(email);
        if (user == null)
        {
            // Даже если пользователь не найден, проверяем лимит запросов
            if (IsRateLimited(email))
            {
                return BadRequestEmailRateLimited(email);
            }

            // Логируем попытку входа с несуществующим email
            LoggingAuthentification(false, obj, email, null);
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Защита от атак по времени: если хеш пароля отсутствует, выполняем фиктивную проверку
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            _ = PassHasher.Verify(email, "dummy", "dummy"); // Подмена для симуляции задержки
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Проверка пароля через хеширование с использованием email как соли
        if (!PassHasher.Verify(email, user.PasswordHash, password))
        {
            if (IsRateLimited(email))
            {
                return BadRequestEmailRateLimited(email);
            }

            LoggingAuthentification(false, obj, email, user.Id);
            IncrementFailedLoginAttempt(email);
            return BadRequestAuthInvalidCredentials();
        }

        // Сброс счётчика неудачных попыток после успешного входа
        ResetLoginAttempts(email);

        // Проверяем не забанен ли пользователь
        DateTimeOffset dtUnban = DateTimeOffset.MinValue;
        if (user.Bans != null && user.Bans.Count > 0)
        {
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow;
            foreach (User_Ban ban in user.Bans)
            {
                if (ban.ExpiresAt == null)
                {
                    // Бан бессрочный
                    dtUnban = DateTimeOffset.MaxValue;
                    break;
                }
                else
                {
                    DateTimeOffset v = ban.ExpiresAt.Value;
                    if (nowUtc < v && dtUnban < v)
                    {
                        dtUnban = v;
                    }
                }
            }
        }
       
        if (dtUnban == DateTimeOffset.MaxValue)
        {
            return StatusCode(403, new { code = (long)SR.AccountBannedPermanently });
        }
        else {
            if (dtUnban > DateTimeOffset.MinValue)
            {

                return StatusCode(403, new {
                    code = (long)SR.AccountBannedUntil,
                    dateTimeExpiresAt= $"{dtUnban:yyyy.MM.dd HH:mm:ss} UTC"
                });
            }
        }


            // Генерация JWT-токена для пользователя при успешной аутентификации
            string token = _jwtService.GenerateToken(user.Id);

        // Логируем успешный вход
        LoggingAuthentification(true, obj, email, user.Id);

        // Возвращаем токен клиенту
        return Ok(new { token });
    }

    /// <summary>
    /// Возвращает ответ с кодом 429 (Too Many Requests), если пользователь временно заблокирован из-за множества попыток входа.
    /// </summary>
    /// <param name="email">Email пользователя, для которого проверяется блокировка.</param>
    /// <returns>Результат с кодом состояния 429 и данными о времени окончания блокировки.</returns>
    protected IActionResult BadRequestEmailRateLimited(string email)
    {
        return StatusCode(429, new { code = (long)SR.TooManyRequests, secondsRemaining = GetRemainingLockoutTime(email) });
    }

    /// <summary>
    /// Асинхронно добавляет запись о попытке аутентификации в очередь фонового логгера.
    /// В случае ошибки записывает предупреждение в лог.
    /// </summary>
    /// <param name="authorizationSuccess">Флаг успешности авторизации.</param>
    /// <param name="obj">JSON-объект запроса (для аудита).</param>
    /// <param name="email">Email пользователя (если известен).</param>
    /// <param name="userId">Идентификатор пользователя (если известен).</param>
    private void LoggingAuthentification(bool authorizationSuccess, JsonObject obj, string? email, Guid? userId)
    {
        try
        {
            _backgroundLoggerAuthentificationService.EnqueueLog(authorizationSuccess, obj, email, userId, GetClientIpAddress());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Не удалось записать лог успешного входа для пользователя {UserId} (email: {Email})", userId, email);
        }
    }

    /// <summary>
    /// Определяет IP-адрес клиента из HTTP-запроса.
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

    /// <summary>
    /// Запись, представляющая данные о попытке входа: количество попыток и время окончания блокировки.
    /// </summary>
    private record LoginAttempt(int Count, DateTimeOffset ExpiresAt);

    /// <summary>
    /// Увеличивает счётчик неудачных попыток входа для указанного email.
    /// Если счётчик достиг предела, пользователь будет временно заблокирован.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    private void IncrementFailedLoginAttempt(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        string key = $"login-attempts:{email.ToLowerInvariant()}";
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = now + LockoutPeriod;

        LoginAttempt? attempt = _memoryCache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpiration = expiresAt;
            return new LoginAttempt(1, expiresAt);
        });

        var updated = new LoginAttempt(
            Count: attempt!.Count + 1,
            ExpiresAt: expiresAt
        );

        _ = _memoryCache.Set(key, updated, LockoutPeriod);
    }

    /// <summary>
    /// Сбрасывает счётчик попыток входа после успешного входа.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    private void ResetLoginAttempts(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        string key = $"login-attempts:{email.ToLowerInvariant()}";
        _memoryCache.Remove(key);
    }

    /// <summary>
    /// Проверяет, превышено ли допустимое количество попыток входа для указанного email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>True, если пользователь временно заблокирован, иначе — false.</returns>
    private bool IsRateLimited(string email)
    {
        string cacheKey = $"login-attempts:{email.ToLowerInvariant()}";
        return _memoryCache.TryGetValue(cacheKey, out LoginAttempt? attempt) && attempt != null && attempt.Count >= Server_Common.Consts.MAX_LOGIN_ATTEMPTS &&
                   attempt.ExpiresAt > DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Определяет, сколько секунд осталось до окончания блокировки учётной записи.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Количество секунд до разблокировки или 0, если блокировка завершена.</returns>
    private long GetRemainingLockoutTime(string email)
    {
        string cacheKey = $"login-attempts:{email.ToLowerInvariant()}";
        return _memoryCache.TryGetValue(cacheKey, out LoginAttempt? attempt) && attempt != null && attempt.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Max(0, Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds))
            : 0;
    }
}
