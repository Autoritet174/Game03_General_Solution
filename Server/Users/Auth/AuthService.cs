using General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Server.Jwt_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Text.Json.Nodes;

namespace Server.Users.Auth;

public sealed class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    DbContext_Game dbContext,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService)
{
    private static readonly Random _Random = new();
    private static readonly TimeSpan _LockoutPeriod = TimeSpan.FromMinutes(2);

    public async Task<AuthRegResponse> LoginAsync(
        string email,
        string password,
        JsonObject requestJson,
        IPAddress? ip)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

        Guid? userId = null; // ID пользователя для логирования
        bool success = false;

        try
        {
            if (email.IsEmpty() || password.IsEmpty())
            {   
                return AuthRegResponse.InvalidCredentials();   
            }

            User? user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AuthRegResponse.InvalidCredentials();
            }
            userId = user.Id;

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
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
                return AuthRegResponse.TooManyRequests(GetRemainingLockoutTime(email));
            }

            if (!signInResult.Succeeded)
            {
                return AuthRegResponse.InvalidCredentials();
            }

            // 2FA
            if (signInResult.RequiresTwoFactor)
            {
                return AuthRegResponse.RequiresTwoFactor(user.Id);
            }
            success = true;
            ResetAttempts(email);

            string token = jwt.GenerateToken(user.Id);

            return AuthRegResponse.SuccessResponse(token);
        }
        catch
        {
            IncrementFailedAttempt(email);
            throw;
        }
        finally
        {
            try
            {
                backgroundLoggerAuthentificationService.EnqueueLog(success, requestJson, email, userId, ip, true);
                if (!success)
                {
                    IncrementFailedAttempt(email);
                }
                await Delay(dtEndCheck); // Задержка перед возвратом
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in finally block during auth for email {Email}", email);
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
        if (email.IsEmpty())
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
        if (!email.IsEmpty())
        {
            cache.Remove($"login-attempts:{email.NormalizedValueGame03()}");
        }
    }

    private long GetRemainingLockoutTime(string email)
    {
        return cache.TryGetValue(
            $"login-attempts:{email.NormalizedValueGame03()}",
            out Attempt? attempt)
            && attempt!.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
            : 0;
    }
}
