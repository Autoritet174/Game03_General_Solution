using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Security;
using System.Security.Cryptography;

namespace Server.Users.Authentication;

public sealed class SessionService(DbContext_Game dbContext, ILogger<SessionService> logger)
{
    private const int TokenSize = 32;

    private static readonly TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(14);

    /// <summary>
    /// СОЗДАЕТ НОВУЮ СЕССИЮ И ВОЗВРАЩАЕТ REFRESH TOKEN В BASE64.
    /// </summary>
    public async Task<string> CreateSessionAsync(Guid userId, DtoRequestAuthReg? dto)
    {
        byte[] tokenBytes = RandomNumberGenerator.GetBytes(TokenSize);

        var session = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenBytes,
            ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime),
            IsUsed = false,
            IsRevoked = false,
            UserDeviceId = await GetOrAddDeviceId(dto)
        };

        _ = dbContext.UserSessions.Add(session);
        _ = await dbContext.SaveChangesAsync();

        return Convert.ToBase64String(tokenBytes);
    }

    /// <summary>
    /// ОБНОВЛЯЕТ СУЩЕСТВУЮЩУЮ СЕССИЮ (ROTATION + REUSE DETECTION).
    /// </summary>
    public async Task<(Guid UserId, string NewRefreshToken)> RefreshSessionAsync(string oldTokenBase64)
    {
        byte[] providedHash = Convert.FromBase64String(oldTokenBase64);

        UserSession? session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.TokenHash == providedHash) ?? throw new SecurityException("Invalid token.");

        // 2. REUSE DETECTION (GOOGLE/STEAM STYLE)
        if (session.IsUsed || session.IsRevoked)
        {
            await RevokeAllUserSessionsAsync(session.UserId, "POTENTIAL_REUSE_ATTEMPT");
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical("ОБНАРУЖЕНА ПОПЫТКА ПОВТОРНОГО ИСПОЛЬЗОВАНИЯ ТОКЕНА! USER: {Id}", session.UserId);
            }
            throw new SecurityException("Token compromise detected.");
        }

        // 3. ПРОВЕРКА СРОКА
        if (session.ExpiresAt < DateTimeOffset.UtcNow)
        {
            throw new SecurityException("Token expired.");
        }

        // 4. ДЕАКТИВАЦИЯ СТАРОГО ТОКЕНА
        session.IsUsed = true;
        session.InactivatedAt = DateTimeOffset.UtcNow;
        session.InactivationReason = "ROTATION";

        // 5. СОЗДАНИЕ НОВОГО ТОКЕНА В ТОЙ ЖЕ ТРАНЗАКЦИИ
        string newRefreshToken = await CreateSessionAsync(session.UserId, null);

        return (session.UserId, newRefreshToken);
    }

    private async Task RevokeAllUserSessionsAsync(Guid userId, string reason) => await dbContext.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, reason));



    /// <summary>
    /// ЗАВЕРШАЕТ КОНКРЕТНУЮ СЕССИЮ ПОЛЬЗОВАТЕЛЯ.
    /// </summary>
    /// <param name="refreshTokenBase64">ТОКЕН, КОТОРЫЙ НУЖНО АННУЛИРОВАТЬ.</param>
    public async Task LogoutAsync(string refreshTokenBase64)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshTokenBase64);

        byte[] tokenHash = Convert.FromBase64String(refreshTokenBase64);

        // ИСПОЛЬЗУЕМ EXECUTE_UPDATE ДЛЯ МГНОВЕННОГО ОБНОВЛЕНИЯ БЕЗ ЗАГРУЗКИ СУЩНОСТИ В ПАМЯТЬ
        int affectedRows = await dbContext.UserSessions
            .Where(s => s.TokenHash == tokenHash && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, "USER_LOGOUT"));

        if (affectedRows == 0)
        {
            logger.LogWarning("ПОПЫТКА ВЫХОДА С НЕСУЩЕСТВУЮЩИМ ИЛИ УЖЕ ОТОЗВАННЫМ ТОКЕНОМ.");
        }
    }

    /// <summary>
    /// ПРИНУДИТЕЛЬНО ЗАВЕРШАЕТ ВСЕ СЕССИИ ПОЛЬЗОВАТЕЛЯ (НАПРИМЕР, ПРИ СМЕНЕ ПАРОЛЯ).
    /// </summary>
    public async Task RevokeAllSessionsAsync(Guid userId, string reason = "MANUAL_REVOCATION")
    {
        _ = await dbContext.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, reason));
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("ВСЕ СЕССИИ ПОЛЬЗОВАТЕЛЯ {UserId} ОТОЗВАНЫ. ПРИЧИНА: {Reason}", userId, reason);
        }

    }

    private async Task<Guid> GetOrAddDeviceId(DtoRequestAuthReg dto)
    {
        Guid id = UserDeviceHelper.ComputeId(dto);
        if (!dbContext.UserDevices.Any(a => a.Id == id))
        {
            _ = dbContext.UserDevices.Add(UserDeviceHelper.DtoToUserDevice(dto, id));
            _ = await dbContext.SaveChangesAsync();
        }
        return id;
    }
}
