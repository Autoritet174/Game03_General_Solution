using General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Http_NS.Controllers_NS.Users_NS;
using Server.Jwt_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Text.Json.Nodes;

namespace Server.Users.Auth;

public sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    DbContext_Game dbContext,
    BackgroundLoggerAuthentificationService logger)
{
    private static readonly Random _randomDelay = new();
    private static readonly TimeSpan LockoutPeriod = TimeSpan.FromMinutes(2);

    public async Task<AuthResult> LoginAsync(
        string email,
        string password,
        JsonObject requestJson,
        IPAddress? ip)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_randomDelay.Next(600, 800));

        try
        {
            if (email.IsEmpty() || password.IsEmpty())
            {
                RegisterFail(email);
                return AuthResult.InvalidCredentials();
            }

            ApplicationUser? user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                RegisterFail(email);
                return await DelayAndFail(dtEndCheck);
            }

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                RegisterFail(email);
                return await DelayAndFail(dtEndCheck);
            }

            // БАН
            if (await dbContext.UserBans.AnyAsync(b => b.ApplicationUserId == user.Id && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now)))
            {
                UserBan? userBan = await dbContext.UserBans.Where(b => b.ApplicationUserId == user.Id && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
                    .OrderByDescending(b => b.ExpiresAt == null).ThenByDescending(b => b.ExpiresAt).FirstOrDefaultAsync();

                await Delay(dtEndCheck);
                return userBan == null ? AuthResult.BadRequestInvalidResponse() : AuthResult.Banned(userBan.ExpiresAt);
            }

            if (signInResult.IsLockedOut)
            {
                await Delay(dtEndCheck);
                return AuthResult.TooManyRequests(GetRemainingLockoutTime(email));
            }

            if (!signInResult.Succeeded)
            {
                RegisterFail(email);
                return await DelayAndFail(dtEndCheck);
            }

            // 2FA
            if (signInResult.RequiresTwoFactor)
            {
                await Delay(dtEndCheck);
                return AuthResult.RequiresTwoFactor(user.Id);
            }

            ResetAttempts(email);

            string token = jwt.GenerateToken(user.Id);

            logger.EnqueueLog(true, requestJson, email, user.Id, ip);

            await Delay(dtEndCheck);
            return AuthResult.SuccessResponse(token);
        }
        catch
        {
            RegisterFail(email);
            throw;
        }
    }


    private static async Task<AuthResult> DelayAndFail(DateTimeOffset end)
    {
        await Delay(end);
        return AuthResult.InvalidCredentials();
    }

    private static async Task Delay(DateTimeOffset end)
    {
        TimeSpan delay = end - DateTimeOffset.UtcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }
    }

    private void RegisterFail(string email)
    {
        IncrementFailedLoginAttempt(email);
    }

    private record LoginAttempt(int Count, DateTimeOffset ExpiresAt);

    private void IncrementFailedLoginAttempt(string email)
    {
        if (email.IsEmpty())
        {
            return;
        }

        string key = $"login-attempts:{email.ToLowerInvariant()}";
        DateTimeOffset expires = DateTimeOffset.UtcNow + LockoutPeriod;

        LoginAttempt? attempt = cache.GetOrCreate(key, e =>
        {
            e.AbsoluteExpiration = expires;
            return new LoginAttempt(1, expires);
        });

        _ = cache.Set(key,
            new LoginAttempt(attempt!.Count + 1, expires),
            LockoutPeriod);
    }

    private void ResetAttempts(string email)
    {
        if (!email.IsEmpty())
        {
            cache.Remove($"login-attempts:{email.ToLowerInvariant()}");
        }
    }

    private long GetRemainingLockoutTime(string email)
    {
        return cache.TryGetValue(
            $"login-attempts:{email.ToLowerInvariant()}",
            out LoginAttempt? attempt)
            && attempt!.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
            : 0;
    }
}
