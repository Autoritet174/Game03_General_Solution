using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Security.Cryptography;

namespace Server.Users.Authentication;

public sealed partial class SessionService(DbContextGame dbContext, ILogger<SessionService> logger
    )
{
    private const int TOKEN_SIZE = 32;
    private static readonly TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(14);

    #region LoggerMessages

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Передан пустой refresh token")]
    private partial void LogEmptyToken();

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Некорректный размер токена: {length} байт (ожидается {expectedSize})")]
    private partial void LogInvalidTokenSize(int length, int expectedSize);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Некорректный формат base64 токена")]
    private partial void LogInvalidBase64Format();

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Сессия не найдена или недействительна")]
    private partial void LogSessionNotFound();

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Сессия {sessionId} истекла")]
    private partial void LogSessionExpired(Guid sessionId);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Сессия обновлена. Старая: {oldSessionId}, Новая: {newSessionId}")]
    private partial void LogSessionRefreshed(Guid oldSessionId, Guid newSessionId);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Конфликт параллельного обновления сессии")]
    private partial void LogConcurrencyConflict();

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Ошибка при обновлении сессии")]
    private partial void LogExceptionRefreshSession(Exception ex);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Попытка выхода с несуществующим или уже отозванным токеном")]
    private partial void LogInvalidLogout();
    [LoggerMessage(Level = LogLevel.Warning)]
    private partial void LogExceptionLogout(Exception ex);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Все сессии пользователя {userId} отозваны. Причина: {reason}")]
    private partial void LogAllSessionsRevoked(Guid userId, string reason);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Устройство {deviceId} уже добавлено другим потоком")]
    private partial void LogDeviceAlreadyAdded(Guid deviceId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Ошибка при получении/добавлении устройства {deviceId}")]
    private partial void LogDeviceError(Exception ex, Guid deviceId);

    #endregion

    public async Task<string?> CreateSessionAsync(Guid userId, DtoRequestAuthReg dto)
    {
        Guid? userDeviceId = await GetOrAddDeviceId(dto);
        if (userDeviceId == null)
        {
            return null;
        }

        byte[] refreshToken = RandomNumberGenerator.GetBytes(TOKEN_SIZE);

        var session = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RefreshTokenHash = SHA256.HashData(refreshToken),
            ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime),
            IsUsed = false,
            IsRevoked = false,
            UserDeviceId = userDeviceId.Value,
        };

        _ = dbContext.UserSessions.Add(session);
        _ = await dbContext.SaveChangesAsync();

        return Convert.ToBase64String(refreshToken);
    }

    public async Task<(Guid? UserId, string? NewRefreshToken)> RefreshSessionAsync(string refreshTokenBase64)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenBase64))
        {
            LogEmptyToken();
            return (null, null);
        }

        byte[] providedRefreshTokenHash;
        try
        {
            byte[] providedRefreshToken = Convert.FromBase64String(refreshTokenBase64);
            if (providedRefreshToken.Length != TOKEN_SIZE)
            {
                LogInvalidTokenSize(providedRefreshToken.Length, TOKEN_SIZE);
                return (null, null);
            }
            providedRefreshTokenHash = SHA256.HashData(providedRefreshToken);
        }
        catch (FormatException)
        {
            LogInvalidBase64Format();
            return (null, null);
        }
        catch (Exception ex)
        {
            LogExceptionRefreshSession(ex);
            return (null, null);
        }

        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.ReadCommitted);

        try
        {
            UserSession? session = await dbContext.UserSessions
                .FirstOrDefaultAsync(s =>
                s.RefreshTokenHash == providedRefreshTokenHash
                && s.ExpiresAt > DateTimeOffset.UtcNow);

            if (session == null)
            {
                LogSessionNotFound();
                await transaction.RollbackAsync();
                return (null, null);
            }

            if (session.ExpiresAt < DateTimeOffset.UtcNow)
            {
                LogSessionExpired(session.Id);
                session.IsUsed = true;
                session.InactivatedAt = DateTimeOffset.UtcNow;
                session.InactivationReason = "EXPIRED";
                _ = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return (null, null);
            }

            session.IsUsed = true;
            session.InactivatedAt = DateTimeOffset.UtcNow;
            session.InactivationReason = "ROTATION";

            byte[] newRefreshToken = RandomNumberGenerator.GetBytes(TOKEN_SIZE);
            var newSession = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = session.UserId,
                RefreshTokenHash = SHA256.HashData(newRefreshToken),
                ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime),
                IsUsed = false,
                IsRevoked = false,
                UserDeviceId = session.UserDeviceId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _ = dbContext.UserSessions.Add(newSession);
            _ = await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            LogSessionRefreshed(session.Id, newSession.Id);

            return (session.UserId, Convert.ToBase64String(newRefreshToken));
        }
        catch (DbUpdateConcurrencyException)
        {
            LogConcurrencyConflict();
            await transaction.RollbackAsync();
            return (null, null);
        }
        catch (Exception ex)
        {
            LogExceptionRefreshSession(ex);
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task LogoutAsync(string refreshTokenBase64)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenBase64))
        {
            LogEmptyToken();
            return;
        }

        byte[] refreshTokenBytes;
        try
        {
            refreshTokenBytes = Convert.FromBase64String(refreshTokenBase64);
            if (refreshTokenBytes.Length != TOKEN_SIZE)
            {
                LogInvalidTokenSize(refreshTokenBytes.Length, TOKEN_SIZE);
                return;
            }
        }
        catch (FormatException)
        {
            LogInvalidBase64Format();
            return;
        }
        catch (Exception ex) {
            LogExceptionLogout(ex);
            return;
        }


        byte[] refreshTokenHash = SHA256.HashData(refreshTokenBytes);
       
        DateTimeOffset now = DateTimeOffset.UtcNow;
        int affectedRows = await dbContext.UserSessions
            .Where(s => s.RefreshTokenHash == refreshTokenHash && s.ExpiresAt > now)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, "USER_LOGOUT"));

        if (affectedRows == 0)
        {
            LogInvalidLogout();
        }
    }

    /// <summary>
    /// Полностью разлогинить пользователя, например при смене пароля или выдаче бана.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public async Task RevokeAllUserSessionsAsync(Guid userId, string reason = "MANUAL_REVOCATION")
    {
        _ = await dbContext.UserSessions
            .Where(s => s.UserId == userId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, reason));

        LogAllSessionsRevoked(userId, reason);
    }

    private async Task<Guid?> GetOrAddDeviceId(DtoRequestAuthReg dto)
    {
        Guid id = UserDeviceHelper.ComputeId(dto);

        try
        {
            if (!await dbContext.UserDevices.AnyAsync(d => d.Id == id))
            {
                UserDevice newDevice = UserDeviceHelper.DtoToUserDevice(dto, id);
                _ = dbContext.UserDevices.Add(newDevice);

                try
                {
                    _ = await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                    when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    dbContext.Entry(newDevice).State = EntityState.Detached;
                    LogDeviceAlreadyAdded(id);
                }
            }

            return id;
        }
        catch (Exception ex)
        {
            LogDeviceError(ex, id);
            return null;
        }
    }
}
