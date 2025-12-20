using General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Jwt_NS;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Text.Json.Nodes;
using L = General.LocalizationKeys;

namespace Server.Http_NS.Controllers_NS.Users_NS;

/// <summary>
/// Контроллер аутентификации пользователей. Обрабатывает процесс входа (логин) по email и паролю.
/// При успешной проверке возвращает JWT-токен доступа.
/// </summary>
public class AuthenticationController(JwtService jwtService, ILogger<AuthenticationController> logger,
    IMemoryCache memoryCache, BackgroundLoggerAuthentificationService backgroundLoggerAuthentificationService, DbContext_Game dbContext) : ControllerBaseApi
{

    /// <summary>
    /// Время блокировки учётной записи при превышении допустимого количества неудачных попыток входа.
    /// </summary>
    private static readonly TimeSpan LockoutPeriod = TimeSpan.FromMinutes(2);


    private static readonly Random _randomDelay = new();

    /// <summary>
    /// Основной метод аутентификации: принимает email и пароль, проверяет учётные данные,
    /// при успехе возвращает JWT-токен. Доступ разрешён без авторизации.
    /// </summary>
    /// <returns>JWT-токен при успешной аутентификации или соответствующий код ошибки.</returns>
    [AllowAnonymous, HttpPost, EnableRateLimiting(Consts.RATE_LIMITER_POLICY_AUTH)]
    public async Task<IActionResult> Main()
    {
        // Проверка, что запрос содержит JSON.
        // Ожидается application/json, иначе — ошибка формата.
        if (Request.ContentType?.StartsWith(G.APPLICATION_JSON) != true || !Request.HasJsonContentType())
        {
            return BadRequestInvalidResponse();
        }

        // Извлечение JSON из тела запроса.
        // Если тело пустое или не JSON — возвращаем ошибку.
        JsonObject? jsonObject = await JsonObjectExtension.GetJsonObjectFromRequest(Request);
        if (jsonObject == null)
        {
            return BadRequestAuthInvalidCredentials();
        }



        // Извлечение и валидация email.
        string email = jsonObject.GetString("email");
        if (email.IsEmpty() || email.Length > Consts.EMAIL_MAX_LENGTH)
        {
            // Если пользователь временно заблокирован — возвращаем 429
            if (IsRateLimited(email))
            {
                return BadRequestEmailRateLimited(email);
            }

            // Логируем неудачную попытку входа
            LoggingAuthentification(false, jsonObject, null, null);
            // Увеличиваем счётчик неудачных попыток
            IncrementFailedLoginAttempt(email);

            return BadRequestAuthInvalidCredentials();
        }


        // Проверка формата email
        if (!email.IsEmail())
        {
            LoggingAuthentification(false, jsonObject, email, null);
            IncrementFailedLoginAttempt(email);

            return BadRequestAuthInvalidCredentials();
        }


        // Извлечение и валидация пароля
        string password = jsonObject.GetString("password", removeAfterSuccessGetting: true);
        if (password.IsEmpty() || password.Length > Consts.PASSWORD_MAX_LENGTH)
        {
            LoggingAuthentification(false, jsonObject, email, null);
            IncrementFailedLoginAttempt(email);

            return BadRequestAuthInvalidCredentials();
        }


        DateTimeOffset now = DateTimeOffset.UtcNow;
        // ----------------------------------------------------------------------------------------------------------------
        // Время для проверки блокировки (защита от тайминг-атак)
        double delaySeconds = _randomDelay.Next(600, 800) / 1000d;
        DateTimeOffset dtEndCheck = now.AddSeconds(delaySeconds);
        // ниже перед каждым return вызываем DelayTimingAttack для защиты от тайминг-атак так как тут начинаем читать данные из базы

        // Поиск пользователя по email в базе данных
        User? user = await dbContext.Users.Include(u => u.UserBans.Where(b => b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))).AsNoTracking().FirstOrDefaultAsync(u => u.Email != null && EF.Functions.ILike(u.Email, email));
        if (user == null)
        {
            // Даже если пользователь не найден, проверяем лимит запросов
            if (IsRateLimited(email))
            {
                await DelayTimingAttack(dtEndCheck);
                return BadRequestEmailRateLimited(email);
            }

            // Логируем попытку входа с несуществующим email
            LoggingAuthentification(false, jsonObject, email, null);
            IncrementFailedLoginAttempt(email);

            await DelayTimingAttack(dtEndCheck);
            return BadRequestAuthInvalidCredentials();
        }

        // если хеш пароля отсутствует
        if (user.PasswordHash.IsEmpty())
        {
            IncrementFailedLoginAttempt(email);
            await DelayTimingAttack(dtEndCheck);
            return BadRequestAuthInvalidCredentials();
        }


        // Проверка пароля через хеширование с использованием email как соли
        if (!PassHasher.Verify(email, user.PasswordHash, password))
        {
            if (IsRateLimited(email))
            {
                return BadRequestEmailRateLimited(email);
            }

            LoggingAuthentification(false, jsonObject, email, user.Id);
            IncrementFailedLoginAttempt(email);

            await DelayTimingAttack(dtEndCheck);
            return BadRequestAuthInvalidCredentials();
        }

        // Сброс счётчика неудачных попыток после успешного входа
        ResetLoginAttempts(email);

        // Проверяем не забанен ли пользователь
        DateTimeOffset dtUnban = DateTimeOffset.MinValue;
        if (user.UserBans != null && user.UserBans.Count > 0)
        {
            DateTimeOffset nowUtc = DateTimeOffset.UtcNow;
            foreach (UserBan ban in user.UserBans)
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
            await DelayTimingAttack(dtEndCheck);
            return BadRequestWithServerError(L.Error.Server.AccountBannedPermanently);
        }
        else
        {
            if (dtUnban > DateTimeOffset.MinValue)
            {
                _ = BadRequest(new { KEY_LOCALIZATION = L.Error.Server.AccountBannedUntil, DATE_TIME_EXPIRES_AT = $"{dtUnban:yyyy.MM.dd HH:mm:ss} UTC" });
            }
        }


        // Генерация JWT-токена для пользователя при успешной аутентификации
        string token = jwtService.GenerateToken(user.Id);

        // Логируем успешный вход
        LoggingAuthentification(true, jsonObject, email, user.Id);

        await DelayTimingAttack(dtEndCheck);
        // Возвращаем токен клиенту
        return Ok(new { token });
    }


    private static async Task DelayTimingAttack(DateTimeOffset dtEndCheck_)
    {
        TimeSpan timeDelay = dtEndCheck_ - DateTimeOffset.UtcNow;
        if (timeDelay > TimeSpan.Zero)
        {
            await Task.Delay(timeDelay);
        }
    }

    /// <summary>
    /// Возвращает ответ с кодом 429 (Too Many Requests), если пользователь временно заблокирован из-за множества попыток входа.
    /// </summary>
    /// <param name="email">Email пользователя, для которого проверяется блокировка.</param>
    /// <returns>Результат с кодом состояния 429 и данными о времени окончания блокировки.</returns>
    protected IActionResult BadRequestEmailRateLimited(string email)
    {
        return BadRequest(new { KEY_LOCALIZATION = L.Error.Server.TooManyRequests, SECONDS_REMAINING = GetRemainingLockoutTime(email) });
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
            _ = obj.Remove("password");
            backgroundLoggerAuthentificationService.EnqueueLog(authorizationSuccess, obj, email, userId, GetClientIpAddress());
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(ex, "Не удалось записать лог успешного входа для пользователя {UserId} (email: {Email})", userId, email);
            }
        }
    }

    /// <summary>
    /// Определяет IP-адрес клиента из HTTP-запроса.
    /// </summary>
    /// <returns>IP-адрес в формате <see cref="IPAddress"/> или null, если не удалось определить.</returns>
    private IPAddress? GetClientIpAddress()
    {
        // Пытаемся получить IP из подключения.
        string? ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? null;
        return ip.IsEmpty() ? null : IPAddress.TryParse(ip, out IPAddress? ipAddress) ? ipAddress : null;
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
        if (email.IsEmpty())
        {
            return;
        }

        string key = $"login-attempts:{email.ToLowerInvariant()}";
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = now + LockoutPeriod;

        LoginAttempt? attempt = memoryCache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpiration = expiresAt;
            return new LoginAttempt(1, expiresAt);
        });

        var updated = new LoginAttempt(
            Count: attempt!.Count + 1,
            ExpiresAt: expiresAt
        );

        _ = memoryCache.Set(key, updated, LockoutPeriod);
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
        memoryCache.Remove(key);
    }

    /// <summary>
    /// Проверяет, превышено ли допустимое количество попыток входа для указанного email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>True, если пользователь временно заблокирован, иначе — false.</returns>
    private bool IsRateLimited(string email)
    {
        string cacheKey = $"login-attempts:{email.ToLowerInvariant()}";
        return memoryCache.TryGetValue(cacheKey, out LoginAttempt? attempt) && attempt != null && attempt.Count >= Consts.MAX_LOGIN_ATTEMPTS && attempt.ExpiresAt > DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Определяет, сколько секунд осталось до окончания блокировки учётной записи.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Количество секунд до разблокировки или 0, если блокировка завершена.</returns>
    private long GetRemainingLockoutTime(string email)
    {
        string cacheKey = $"login-attempts:{email.ToLowerInvariant()}";
        return memoryCache.TryGetValue(cacheKey, out LoginAttempt? attempt) && attempt != null && attempt.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Max(0, Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds))
            : 0;
    }
}
