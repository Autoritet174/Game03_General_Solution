using EasyRefreshToken;
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

namespace Server.Users.Authentication;

public sealed class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    DbContext_Game dbContext,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService,
    ITokenService<Guid> tokenService,
    ILogger<AuthService> logger)
{
    private static readonly Random _Random = new();
    private static readonly TimeSpan _LockoutPeriod = TimeSpan.FromMinutes(2);
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
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                return AuthRegResponse.TooManyRequests(GetSecondsRemaining(lockoutEnd));
            }



            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
            {
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
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
            TokenResult tokenResult = await tokenService.OnLoginAsync(userId.Value);

            //if (!tokenResult.IsSucceeded)
            //{
            //    return AuthRegResponse.RefreshTokenErrorCreating();
            //}
            // 
            return AuthRegResponse.Success(accessToken, tokenResult.Token);
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

    private void IncrementFailedAttempt(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        string key = $"login-attempts:{email.NormalizedValueGame03()}";
        DateTimeOffset expires = DateTimeOffset.UtcNow + _LockoutPeriod;

        Attempt? attempt = cache.GetOrCreate(key, e =>
        {
            e.AbsoluteExpiration = expires;
            return new Attempt(1, expires);
        });

        _ = cache.Set(key,
            new Attempt(attempt!.Count + 1, expires),
            _LockoutPeriod);
    }

    private void ResetAttempts(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            cache.Remove($"login-attempts:{email.NormalizedValueGame03()}");
        }
    }

    private long GetRemainingLockoutTime(string email) => cache.TryGetValue(
            $"login-attempts:{email.NormalizedValueGame03()}",
            out Attempt? attempt)
            && attempt!.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
            : 0;









    private static readonly TimeSpan _FastFloodPeriod = TimeSpan.FromSeconds(30);

    private bool IsFloodDetected(string email) =>
        cache.TryGetValue($"flood:{email.NormalizedValueGame03()}", out int count) && count >= 10;

    private void IncrementFloodAttempt(string email)
    {
        string key = $"flood:{email.NormalizedValueGame03()}";
        int current = cache.Get<int?>(key) ?? 0;
        cache.Set(key, current + 1, _FastFloodPeriod);
    }
    private void ResetFloodAttempts(string email) => cache.Remove($"flood:{email.NormalizedValueGame03()}");
    private static long GetSecondsRemaining(DateTimeOffset? end) =>
        end.HasValue ? (long)Math.Max(0, (end.Value - DateTimeOffset.UtcNow).TotalSeconds) : 0;

    private async Task<bool> IsUserBannedAsync(Guid uId, DateTimeOffset now) =>
        await dbContext.UserBans.AnyAsync(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now));

    private async Task<DtoResponseAuthReg> GetBanResponseAsync(Guid uId, DateTimeOffset now)
    {
        var ban = await dbContext.UserBans
            .Where(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
            .OrderByDescending(b => b.ExpiresAt == null)
            .ThenByDescending(b => b.ExpiresAt)
            .FirstOrDefaultAsync();
        return ban == null ? AuthRegResponse.InvalidResponse() : AuthRegResponse.Banned(ban.ExpiresAt);
    }
}
