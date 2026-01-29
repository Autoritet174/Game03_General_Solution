using FluentResults;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Jwt_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;

namespace Server.Users.Authentication;

/// <summary>
/// Сервис аутентификации, оптимизированный для высокой нагрузки.
/// </summary>
public sealed partial class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache memoryCache,
    DbContextGame dbContext,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService,
    SessionService sessionService,
    ILogger<AuthService> logger)
{
    private static readonly Random _Random = new();
    private static readonly TimeSpan _FloodPeriod = TimeSpan.FromSeconds(30);

    #region Compiled Queries

    // 1. Быстрая проверка существования бана (EXISTS). Оптимально для 95% пользователей.
    private static readonly Func<DbContextGame, Guid, DateTimeOffset, Task<bool>> IsUserBannedQuery =
        EF.CompileAsyncQuery((DbContextGame db, Guid uId, DateTimeOffset now) =>
            db.UserBans.Any(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now)));

    // 2. Получение деталей бана. Вызывается только для забаненных (5%).
    private static readonly Func<DbContextGame, Guid, DateTimeOffset, Task<UserBan?>> GetActiveBanDetailsQuery =
        EF.CompileAsyncQuery((DbContextGame db, Guid uId, DateTimeOffset now) =>
            db.UserBans
                .Where(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
                .OrderByDescending(b => b.ExpiresAt == null)
                .ThenByDescending(b => b.ExpiresAt)
                .FirstOrDefault());

    #endregion

    #region LoggerMessages
    [LoggerMessage(Level = LogLevel.Warning, Message = "Flood attempt: {Email}")]
    private partial void LogFlood(string Email);

    [LoggerMessage(Level = LogLevel.Error, Message = "Auth exception for {Email}: {Msg}")]
    private partial void LogAuthEx(string Email, string Msg, Exception ex);
    #endregion

    /// <summary>
    /// Выполняет вход пользователя в систему.
    /// </summary>
    public async Task<Result<DtoResponseAuthReg>> LoginAsync(DtoRequestAuthReg dto, IPAddress? ip)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

        if (string.IsNullOrWhiteSpace(dto?.Email) || string.IsNullOrWhiteSpace(dto?.Password))
        {
            return Result.Fail(new Error("Invalid credentials").WithMetadata("Type", "BadRequest"));
        }

        // Защита от Flood с использованием string.Create (Zero-allocation для ключа)
        string floodKey = GetFloodKey(dto.Email);
        if (memoryCache.TryGetValue(floodKey, out int attempts) && attempts >= 10)
        {
            LogFlood(dto.Email);
            return Result.Ok(AuthRegResponse.TooManyRequests((long)_FloodPeriod.TotalSeconds));
        }

        Guid? userId = null;
        bool success = false;

        try
        {
            User? user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Result.Ok(AuthRegResponse.InvalidCredentials());
            }

            userId = user.Id;

            // Проверка Lockout (Identity)
            if (await userManager.IsLockedOutAsync(user))
            {
                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                return Result.Ok(AuthRegResponse.TooManyRequests(GetSecondsLeft(lockoutEnd)));
            }

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
            if (!signInResult.Succeeded)
            {
                IncrementFlood(floodKey);
                return Result.Ok(signInResult.IsLockedOut
                    ? AuthRegResponse.TooManyRequests(GetSecondsLeft(await userManager.GetLockoutEndDateAsync(user)))
                    : AuthRegResponse.InvalidCredentials());
            }

            // --- ОПТИМИЗАЦИЯ ПРОВЕРКИ БАНА ---
            // Сначала легкий EXISTS для 95% случаев
            if (await IsUserBannedQuery(dbContext, userId.Value, now))
            {
                // Только для 5% случаев запрашиваем сущность целиком
                UserBan? ban = await GetActiveBanDetailsQuery(dbContext, userId.Value, now);
                return Result.Ok(AuthRegResponse.Banned(ban?.ExpiresAt));
            }

            success = true;
            memoryCache.Remove(floodKey);

            string accessToken = jwt.GenerateToken(userId.Value);
            Result<SessionResponseData> sessionResult = await sessionService.CreateSessionAsync(userId.Value, dto);

            return sessionResult.IsFailed
                ? Result.Ok(AuthRegResponse.InvalidResponse())
                : Result.Ok(AuthRegResponse.Success(accessToken, sessionResult.Value.RefreshToken, sessionResult.Value.ExpiresAt));
        }
        catch (Exception ex)
        {
            LogAuthEx(dto.Email, ex.Message, ex);
            return Result.Fail("Internal error");
        }
        finally
        {
            backgroundLoggerAuthentificationService.EnqueueLog(success, dto, userId, ip, true);
            await ApplyArtificialDelay(dtEndCheck);
        }
    }

    private static string GetFloodKey(string email) =>
        string.Create(6 + email.Length, email, static (span, s) =>
        {
            "flood:".AsSpan().CopyTo(span);
            _ = s.AsSpan().ToUpperInvariant(span[6..]);
        });

    private void IncrementFlood(string key)
    {
        int count = memoryCache.Get<int?>(key) ?? 0;
        _ = memoryCache.Set(key, count + 1, _FloodPeriod);
    }

    private static long GetSecondsLeft(DateTimeOffset? end) =>
        end.HasValue ? (long)Math.Max(0, (end.Value - DateTimeOffset.UtcNow).TotalSeconds) : 0;

    private static async Task ApplyArtificialDelay(DateTimeOffset end)
    {
        TimeSpan delay = end - DateTimeOffset.UtcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }
    }
}
//using FluentResults;
//using General.DTO.RestRequest;
//using General.DTO.RestResponse;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using Server.Jwt_NS;
//using Server_DB_Postgres;
//using Server_DB_Postgres.Entities.Users;
//using System.Net;

//namespace Server.Users.Authentication;

//public sealed class AuthService(
//    UserManager<User> userManager,
//    SignInManager<User> signInManager,
//    JwtService jwt,
//    IMemoryCache memoryCache,
//    DbContextGame dbContext,
//    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService,
//    SessionService sessionService,
//    ILogger<AuthService> logger)
//{
//    private static readonly Random _Random = new();

//    public async Task<DtoResponseAuthReg> LoginAsync(
//        DtoRequestAuthReg dto,
//        IPAddress? ip)
//    {
//        DateTimeOffset now = DateTimeOffset.UtcNow;
//        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

//        Guid? userId = null; // ID пользователя для логирования
//        bool success = false;

//        if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
//        {
//            return AuthRegResponse.InvalidCredentials();
//        }

//        // 1. Уровень: Защита от Flood (Memory Cache)
//        if (IsFloodDetected(dto.Email))
//        {
//            return AuthRegResponse.TooManyRequests((long)_FastFloodPeriod.TotalSeconds);
//        }

//        try
//        {


//            User? user = await userManager.FindByEmailAsync(dto.Email);
//            if (user == null)
//            {
//                return AuthRegResponse.InvalidCredentials();
//            }
//            userId = user.Id;

//            // 2. Уровень: Проверка Lockout в Identity (Persistent)
//            if (await userManager.IsLockedOutAsync(user))
//            {
//                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
//                return AuthRegResponse.TooManyRequests(GetSecondsRemaining(lockoutEnd));
//            }



//            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

//            if (signInResult.IsLockedOut)
//            {
//                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
//                return AuthRegResponse.TooManyRequests(GetSecondsRemaining(lockoutEnd));
//            }
//            if (!signInResult.Succeeded)
//            {
//                IncrementFloodAttempt(dto.Email);
//                return AuthRegResponse.InvalidCredentials();
//            }
//            if (signInResult.RequiresTwoFactor)
//            {
//                return AuthRegResponse.RequiresTwoFactor();
//            }


//            // Баны
//            if (await IsUserBannedAsync(userId.Value, now))
//            {
//                return await GetBanResponseAsync(userId.Value, now);
//            }

//            success = true;

//            ResetFloodAttempts(dto.Email);

//            string accessToken = jwt.GenerateToken(userId.Value);

//            Result<SessionResponseData> result = await sessionService.CreateSessionAsync(user.Id, dto);
//            if (result.IsFailed)
//            {
//                return AuthRegResponse.InvalidResponse();
//            }
//            SessionResponseData data = result.Value;
//            return AuthRegResponse.Success(accessToken, data.RefreshToken, data.ExpiresAt);
//        }
//        catch
//        {
//            IncrementFloodAttempt(dto.Email);
//            return AuthRegResponse.InvalidResponse();
//        }
//        finally
//        {
//            try
//            {
//                backgroundLoggerAuthentificationService.EnqueueLog(success, dto, userId, ip, true);
//                await Delay(dtEndCheck); // Задержка перед возвратом
//            }
//            catch (Exception ex)
//            {
//                if (logger.IsEnabled(LogLevel.Error))
//                {
//                    logger.LogError(ex, "Error in finally block during auth for email {Email}", dto.Email);
//                }
//            }
//        }
//    }



//    private static async Task Delay(DateTimeOffset end)
//    {
//        TimeSpan delay = end - DateTimeOffset.UtcNow;
//        if (delay > TimeSpan.Zero)
//        {
//            await Task.Delay(delay);
//        }
//    }


//    private record Attempt(int Count, DateTimeOffset ExpiresAt);

//    //private void IncrementFailedAttempt(string email)
//    //{
//    //    if (string.IsNullOrWhiteSpace(email))
//    //    {
//    //        return;
//    //    }

//    //    string key = $"login-attempts:{email.NormalizedValueGame03()}";
//    //    DateTimeOffset expires = DateTimeOffset.UtcNow + _LockoutPeriod;

//    //    Attempt? attempt = cache.GetOrCreate(key, e =>
//    //    {
//    //        e.AbsoluteExpiration = expires;
//    //        return new Attempt(1, expires);
//    //    });

//    //    _ = cache.Set(key,
//    //        new Attempt(attempt!.Count + 1, expires),
//    //        _LockoutPeriod);
//    //}

//    //private void ResetAttempts(string email)
//    //{
//    //    if (!string.IsNullOrWhiteSpace(email))
//    //    {
//    //        cache.Remove($"login-attempts:{email.NormalizedValueGame03()}");
//    //    }
//    //}

//    //private long GetRemainingLockoutTime(string email) => cache.TryGetValue(
//    //        $"login-attempts:{email.NormalizedValueGame03()}",
//    //        out Attempt? attempt)
//    //        && attempt!.ExpiresAt > DateTimeOffset.UtcNow
//    //        ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
//    //        : 0;









//    private static readonly TimeSpan _FastFloodPeriod = TimeSpan.FromSeconds(30);

//    private bool IsFloodDetected(string email)
//    {
//        return memoryCache.TryGetValue($"flood:{email.Trim().ToUpperInvariant()}", out int count) && count >= 10;
//    }

//    private void IncrementFloodAttempt(string email)
//    {
//        string key = $"flood:{email.Trim().ToUpperInvariant()}";
//        int current = memoryCache.Get<int?>(key) ?? 0;
//        _ = memoryCache.Set(key, current + 1, _FastFloodPeriod);
//    }
//    private void ResetFloodAttempts(string email)
//    {
//        memoryCache.Remove($"flood:{email.Trim().ToUpperInvariant()}");
//    }

//    private static long GetSecondsRemaining(DateTimeOffset? end)
//    {
//        return end.HasValue ? (long)Math.Max(0, (end.Value - DateTimeOffset.UtcNow).TotalSeconds) : 0;
//    }

//    private async Task<bool> IsUserBannedAsync(Guid uId, DateTimeOffset now)
//    {
//        return await dbContext.UserBans.AnyAsync(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now));
//    }

//    private async Task<DtoResponseAuthReg> GetBanResponseAsync(Guid uId, DateTimeOffset now)
//    {
//        UserBan? ban = await dbContext.UserBans
//            .Where(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
//            .OrderByDescending(b => b.ExpiresAt == null)
//            .ThenByDescending(b => b.ExpiresAt)
//            .FirstOrDefaultAsync();
//        return ban == null ? AuthRegResponse.InvalidResponse() : AuthRegResponse.Banned(ban.ExpiresAt);
//    }
//}
