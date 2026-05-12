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
    private static readonly TimeSpan _FloodPeriod = TimeSpan.FromSeconds(30);

    #region Compiled Queries

    // 1. Быстрая проверка существования бана. Добавлен CancellationToken.
    private static readonly Func<DbContextGame, Guid, DateTimeOffset, CancellationToken, Task<bool>> IsUserBannedQuery =
        EF.CompileAsyncQuery(
            (DbContextGame db, Guid uId, DateTimeOffset now, CancellationToken ct) =>
            db.UserBans.Any(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
        );

    // 2. Получение деталей бана. Добавлен CancellationToken.
    private static readonly Func<DbContextGame, Guid, DateTimeOffset, CancellationToken, Task<UserBan?>> GetActiveBanDetailsQuery =
        EF.CompileAsyncQuery(
            (DbContextGame db, Guid uId, DateTimeOffset now, CancellationToken ct) =>
            db.UserBans
                .Where(b => b.UserId == uId && b.CreatedAt <= now && (b.ExpiresAt == null || b.ExpiresAt >= now))
                .OrderByDescending(b => b.ExpiresAt == null)
                .ThenByDescending(b => b.ExpiresAt)
                .FirstOrDefault()
        );

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
    public async Task<Result<DtoResponseAuthReg>> LoginAsync(DtoRequestAuthReg dto, IPAddress? ip, CancellationToken cancellationToken)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(Random.Shared.Next(600, 800));

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
            User? user = await userManager.FindByEmailAsync(dto.Email).ConfigureAwait(false);
            if (user == null)
            {
                return Result.Ok(AuthRegResponse.InvalidCredentials());
            }

            userId = user.Id;

            // Проверка Lockout (Identity)
            if (await userManager.IsLockedOutAsync(user).ConfigureAwait(false))
            {
                DateTimeOffset? lockoutEnd = await userManager.GetLockoutEndDateAsync(user).ConfigureAwait(false);
                return Result.Ok(AuthRegResponse.TooManyRequests(GetSecondsLeft(lockoutEnd)));
            }

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, true).ConfigureAwait(false);
            if (!signInResult.Succeeded)
            {
                IncrementFlood(floodKey);
                return Result.Ok(signInResult.IsLockedOut
                    ? AuthRegResponse.TooManyRequests(GetSecondsLeft(await userManager.GetLockoutEndDateAsync(user).ConfigureAwait(false)))
                    : AuthRegResponse.InvalidCredentials());
            }

            // --- ОПТИМИЗАЦИЯ ПРОВЕРКИ БАНА ---
            // Сначала легкий EXISTS для 95% случаев
            if (await IsUserBannedQuery(dbContext, userId.Value, now, cancellationToken).ConfigureAwait(false))
            {
                // Только для 5% случаев запрашиваем сущность целиком
                UserBan? ban = await GetActiveBanDetailsQuery(dbContext, userId.Value, now, cancellationToken).ConfigureAwait(false);
                return Result.Ok(AuthRegResponse.Banned(ban?.ExpiresAt));
            }

            success = true;
            memoryCache.Remove(floodKey);

            string accessToken = jwt.GenerateToken(userId.Value);
            Result<SessionResponseData> sessionResult = await sessionService.CreateSessionAsync(userId.Value, dto, cancellationToken).ConfigureAwait(false);

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
            await ApplyArtificialDelayAsync(dtEndCheck, cancellationToken).ConfigureAwait(false);
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

    private static async Task ApplyArtificialDelayAsync(DateTimeOffset end, CancellationToken cancellationToken)
    {
        TimeSpan delay = end - DateTimeOffset.UtcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
        }
    }
}
