using General;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Jwt_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Security.Cryptography;

namespace Server.Users.Authentication;

public sealed class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    DbContext_Game dbContext,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService,
    SessionService sessionService,
    ILogger<AuthService> logger)
{
    private static readonly Random _Random = new();
    private static readonly TimeSpan _LockoutPeriod = TimeSpan.FromMinutes(2);

    /// <summary>
    /// СРОК ЖИЗНИ REFRESH ТОКЕНА (НАПРИМЕР, 14 ДНЕЙ)
    /// </summary>
    private static readonly TimeSpan _RefreshTokenLifeTime = TimeSpan.FromDays(14);
    public async Task<DtoResponseAuthReg> LoginAsync(
        DtoRequestAuthReg dto,
        IPAddress? ip)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

        Guid? userId = null; // ID пользователя для логирования
        bool success = false;

        try
        {
            // 1. Уровень: Защита от Flood (Memory Cache)
            if (IsFloodDetected(dto.Email))
            {
                return AuthRegResponse.TooManyRequests((long)_FastFloodPeriod.TotalSeconds);
            }

            User? user = null;
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return AuthRegResponse.InvalidCredentials();
            }

            user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return AuthRegResponse.InvalidCredentials();
            }
            userId = user.Id;

            // 2. Уровень: Проверка Lockout в Identity (Persistent)
            if (await userManager.IsLockedOutAsync(user))
            {
                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                return AuthRegResponse.TooManyRequests(GetSecondsRemaining(lockoutEnd));
            }



            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
            {
                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                return AuthRegResponse.TooManyRequests(GetSecondsRemaining(lockoutEnd));
            }
            if (!signInResult.Succeeded)
            {
                IncrementFloodAttempt(dto.Email);
                return AuthRegResponse.InvalidCredentials();
            }
            if (signInResult.RequiresTwoFactor)
            {
                return AuthRegResponse.RequiresTwoFactor();
            }



            //else if (!string.IsNullOrWhiteSpace(dto.RefreshToken))
            //{
            //    // Аутентификация по RefreshToken
            //    RefreshToken? refreshTokens = dbContext.RefreshTokens.AsNoTracking().FirstOrDefault(a => a.Token == dto.RefreshToken);
            //    if (refreshTokens == null)
            //    {
            //        return AuthRegResponse.InvalidCredentials();
            //    }
            //    if (refreshTokens.ExpiredDate < DateTime.UtcNow)
            //    {
            //        return AuthRegResponse.InvalidCredentials();
            //    }

            //    userId = refreshTokens.UserId;
            //    user = dbContext.Users.AsNoTracking().FirstOrDefault(a => a.Id == userId);
            //    if (user == null)
            //    {
            //        if (logger.IsEnabled(LogLevel.Warning))
            //        {
            //            logger.LogWarning("Пользователь [{userId}] не был найдет, возможно пользователь это злоумышленник пытающийся подделать RefreshToken.", userId);
            //        }
            //        return AuthRegResponse.InvalidCredentials();
            //    }
            //}

            // 3. Уровень: Бизнес-валидация (Баны)
            if (await IsUserBannedAsync(userId.Value, now))
            {
                return await GetBanResponseAsync(userId.Value, now);
            }

            success = true;

            ResetFloodAttempts(dto.Email);


            //// всегда при успешном логине любым методом удаляем старые токены и генерируем новый RefreshToken.
            // await dbContext.RefreshTokens.Where(a => a.UserId == userId).ExecuteDeleteAsync();

            string accessToken = jwt.GenerateToken(userId.Value);

            // ВЫЗЫВАЕМ НОВЫЙ СЕРВИС ДЛЯ СОЗДАНИЯ СЕССИИ
            string refreshToken = await sessionService.CreateSessionAsync(user.Id, dto);
            return AuthRegResponse.Success(accessToken, refreshToken);
        }
        catch
        {
            IncrementFloodAttempt(dto.Email);
            throw;
        }
        finally
        {
            try
            {
                backgroundLoggerAuthentificationService.EnqueueLog(success, dto, userId, ip, true);
                await Delay(dtEndCheck); // Задержка перед возвратом
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, "Error in finally block during auth for email {Email}", dto.Email);
                }
            }
        }
    }



    private static async Task Delay(DateTimeOffset end)
    {
        TimeSpan delay = end - DateTimeOffset.UtcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }
    }


    private record Attempt(int Count, DateTimeOffset ExpiresAt);

    //private void IncrementFailedAttempt(string email)
    //{
    //    if (string.IsNullOrWhiteSpace(email))
    //    {
    //        return;
    //    }

    //    string key = $"login-attempts:{email.NormalizedValueGame03()}";
    //    DateTimeOffset expires = DateTimeOffset.UtcNow + _LockoutPeriod;

    //    Attempt? attempt = cache.GetOrCreate(key, e =>
    //    {
    //        e.AbsoluteExpiration = expires;
    //        return new Attempt(1, expires);
    //    });

    //    _ = cache.Set(key,
    //        new Attempt(attempt!.Count + 1, expires),
    //        _LockoutPeriod);
    //}

    //private void ResetAttempts(string email)
    //{
    //    if (!string.IsNullOrWhiteSpace(email))
    //    {
    //        cache.Remove($"login-attempts:{email.NormalizedValueGame03()}");
    //    }
    //}

    //private long GetRemainingLockoutTime(string email) => cache.TryGetValue(
    //        $"login-attempts:{email.NormalizedValueGame03()}",
    //        out Attempt? attempt)
    //        && attempt!.ExpiresAt > DateTimeOffset.UtcNow
    //        ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
    //        : 0;









    private static readonly TimeSpan _FastFloodPeriod = TimeSpan.FromSeconds(30);

    private bool IsFloodDetected(string email) =>
        cache.TryGetValue($"flood:{email.NormalizedValueGame03()}", out int count) && count >= 10;

    private void IncrementFloodAttempt(string email)
    {
        string key = $"flood:{email.NormalizedValueGame03()}";
        int current = cache.Get<int?>(key) ?? 0;
        _ = cache.Set(key, current + 1, _FastFloodPeriod);
    }
    private void ResetFloodAttempts(string email) => cache.Remove($"flood:{email.NormalizedValueGame03()}");
    private static long GetSecondsRemaining(DateTimeOffset? end) =>
        end.HasValue ? (long)Math.Max(0, (end.Value - DateTimeOffset.UtcNow).TotalSeconds) : 0;

    private async Task<bool> IsUserBannedAsync(Guid uId, DateTimeOffset now) =>
        await dbContext.UserBans.AnyAsync(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now));

    private async Task<DtoResponseAuthReg> GetBanResponseAsync(Guid uId, DateTimeOffset now)
    {
        UserBan? ban = await dbContext.UserBans
            .Where(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
            .OrderByDescending(b => b.ExpiresAt == null)
            .ThenByDescending(b => b.ExpiresAt)
            .FirstOrDefaultAsync();
        return ban == null ? AuthRegResponse.InvalidResponse() : AuthRegResponse.Banned(ban.ExpiresAt);
    }





    /// <summary>
    /// МЕТОД ОБНОВЛЕНИЯ ТОКЕНОВ (REFRESH FLOW).
    /// РЕАЛИЗУЕТ ROTATION И REUSE DETECTION.
    /// </summary>
    //public async Task<DtoResponseAuthReg> RefreshAsync(string refreshTokenBase64, string? deviceId)
    //{
    //    ArgumentException.ThrowIfNullOrWhiteSpace(refreshTokenBase64);

    //    // ПЕРЕВОДИМ BASE64 В БАЙТЫ ДЛЯ ПОИСКА В ПОСТГРЕСЕ (BYTEA)
    //    byte[] tokenHash = Convert.FromBase64String(refreshTokenBase64);

    //    // ИЩЕМ СЕССИЮ В БАЗЕ
    //    UserSession? session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.TokenHash == tokenHash);

    //    if (session == null)
    //    {
    //        return AuthRegResponse.InvalidCredentials();
    //    }

    //    // REUSE DETECTION: ЕСЛИ ТОКЕН УЖЕ ИСПОЛЬЗОВАН, ЗНАЧИТ ЭТО ПОПЫТКА ВЗЛОМА (ПОВТОРНЫЙ REFRESH)
    //    if (session.IsUsed || session.IsRevoked)
    //    {
    //        // АННУЛИРУЕМ ВООБЩЕ ВСЕ СЕССИИ ПОЛЬЗОВАТЕЛЯ ДЛЯ БЕЗОПАСНОСТИ
    //        _ = await dbContext.UserSessions
    //            .Where(s => s.UserId == session.UserId)
    //            .ExecuteUpdateAsync(set => set
    //                .SetProperty(s => s.IsRevoked, true)
    //                .SetProperty(s => s.InactivationReason, "REUSE_DETECTION_TRIGGERED")
    //                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow));

    //        logger.LogCritical("ОБНАРУЖЕНО ПОВТОРНОЕ ИСПОЛЬЗОВАНИЕ ТОКЕНА ДЛЯ ПОЛЬЗОВАТЕЛЯ {UserId}!", session.UserId);
    //        return AuthRegResponse.InvalidCredentials();
    //    }

    //    // ПРОВЕРКА ИСТЕЧЕНИЯ СРОКА ГОДНОСТИ
    //    if (session.ExpiresAt < DateTimeOffset.UtcNow)
    //    {
    //        return AuthRegResponse.InvalidCredentials();
    //    }

    //    // МАРКИРУЕМ ТЕКУЩИЙ ТОКЕН КАК ИСПОЛЬЗОВАННЫЙ (ROTATION)
    //    session.IsUsed = true;
    //    session.InactivatedAt = DateTimeOffset.UtcNow;
    //    session.InactivationReason = "ROTATION";

    //    // ГЕНЕРИРУЕМ НОВУЮ ПАРУ
    //    string accessToken = jwt.GenerateToken(session.UserId);
    //    string newRefreshTokenBase64 = await CreateNewSessionAsync(session.UserId, deviceId);

    //    _ = await dbContext.SaveChangesAsync();

    //    return AuthRegResponse.Success(accessToken, newRefreshTokenBase64);
    //}
    /// <summary>
    /// ВСПОМОГАТЕЛЬНЫЙ МЕТОД СОЗДАНИЯ ЗАПИСИ О СЕССИИ.
    /// </summary>
    //private async Task<string> CreateNewSessionAsync(Guid userId, string? deviceId)
    //{
    //    // ГЕНЕРИРУЕМ 32 СЛУЧАЙНЫХ БАЙТА (OPAQUE TOKEN)
    //    byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
    //    string base64Token = Convert.ToBase64String(randomBytes);

    //    UserSession newSession = new()
    //    {
    //        UserId = userId,
    //        TokenHash = randomBytes,
    //        ExpiresAt = DateTimeOffset.UtcNow.Add(_RefreshTokenLifeTime),
    //        IsUsed = false,
    //        IsRevoked = false,
    //        // ТУТ МОЖНО ДОБАВИТЬ DEVICE_ID В ТАБЛИЦУ, ЕСЛИ ДОБАВИТЕ ТАКОЕ ПОЛЕ В USER_SESSION.CS
    //    };

    //    _ = dbContext.UserSessions.Add(newSession);
    //    _ = await dbContext.SaveChangesAsync();

    //    return base64Token;
    //}
}
