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
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return AuthRegResponse.InvalidResponse();
            }

            User? user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return AuthRegResponse.InvalidCredentials();
            }
            userId = user.Id;

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                return AuthRegResponse.InvalidCredentials();
            }

            // БАН
            if (await dbContext.UserBans.AnyAsync(b => b.UserId == user.Id && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now)))
            {
                UserBan? userBan = await dbContext.UserBans.Where(b => b.UserId == user.Id && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
                    .OrderByDescending(b => b.ExpiresAt == null).ThenByDescending(b => b.ExpiresAt).FirstOrDefaultAsync();

                return userBan == null ? AuthRegResponse.InvalidResponse() : AuthRegResponse.Banned(userBan.ExpiresAt);
            }

            if (signInResult.IsLockedOut)
            {
                return AuthRegResponse.TooManyRequests(GetRemainingLockoutTime(dto.Email));
            }

            if (!signInResult.Succeeded)
            {
                return AuthRegResponse.InvalidCredentials();
            }

            // 2FA
            if (signInResult.RequiresTwoFactor)
            {
                return AuthRegResponse.RequiresTwoFactor();
            }
            success = true;
            ResetAttempts(dto.Email);

            string accessToken = jwt.GenerateToken(user.Id);

            TokenResult tokenResult = await tokenService.OnLoginAsync(user.Id);

            if (!tokenResult.IsSucceeded)
            {
                return AuthRegResponse.RefreshTokenErrorCreating();
            }
            // 
            return AuthRegResponse.Success(accessToken, tokenResult.Token);
        }
        catch
        {
            IncrementFailedAttempt(dto.Email);
            throw;
        }
        finally
        {
            try
            {
                backgroundLoggerAuthentificationService.EnqueueLog(success, dto, userId, ip, true);
                if (!success)
                {
                    IncrementFailedAttempt(dto.Email);
                }
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
}
