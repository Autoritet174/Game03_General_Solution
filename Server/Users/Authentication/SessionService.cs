using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Server.Cache;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using System.Security.Cryptography;

namespace Server.Users.Authentication;

public sealed partial class SessionService(
    DbContextGame dbContext,
    ILogger<SessionService> logger,
    CacheService cache)
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
        Message = "LogSessionUserDeviceIdEmpty")]
    private partial void LogSessionUserDeviceIdEmpty();

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Предыдущий userDeviceId=[{prev}] не совпадает с новым [{now}]")]
    private partial void LogSessionUserDeviceIdOther(Guid prev, Guid now);

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
    private partial void LogAllSessionsRevoked(Guid userId, InactivationReason reason);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Устройство {deviceId} уже добавлено другим потоком")]
    private partial void LogDeviceAlreadyAdded(Guid deviceId);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Ошибка при получении/добавлении устройства {deviceId}")]
    private partial void LogDeviceError(Exception ex, Guid deviceId);

    #endregion

    public async Task<(string? refreshToken, DateTimeOffset? dtExpiration)> CreateSessionAsync(Guid userId, DtoRequestAuthReg dto)
    {
        Guid? userDeviceId = await GetOrAddDeviceId(dto);
        if (userDeviceId == null)
        {
            return (null, null);
        }

        byte[] refreshToken = RandomNumberGenerator.GetBytes(TOKEN_SIZE);

        DateTimeOffset dtExpiration = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);
        var session = new UserSession
        {
            Id = UuidHelper.NewV7(),
            UserId = userId,
            RefreshTokenHash = SHA256.HashData(refreshToken),
            ExpiresAt = dtExpiration,
            IsUsed = false,
            IsRevoked = false,
            UserDeviceId = userDeviceId.Value
        };

        _ = dbContext.UserSessions.Add(session);
        _ = await dbContext.SaveChangesAsync();

        return (Convert.ToBase64String(refreshToken), dtExpiration);
    }

    public async Task<(Guid? UserId, string? NewRefreshToken, DateTimeOffset? dtExpiration)> RefreshSessionAsync(DtoRequestAuthReg dto)
    {
        string? refreshTokenBase64 = dto.RefreshToken;
        (Guid? UserId, string? NewRefreshToken, DateTimeOffset? dtExpiration) resultNull = (null, null, null);
        if (string.IsNullOrWhiteSpace(refreshTokenBase64))
        {
            LogEmptyToken();
            return resultNull;
        }

        byte[] providedRefreshTokenHash;
        try
        {
            byte[] providedRefreshToken = Convert.FromBase64String(refreshTokenBase64);
            if (providedRefreshToken.Length != TOKEN_SIZE)
            {
                LogInvalidTokenSize(providedRefreshToken.Length, TOKEN_SIZE);
                return resultNull;
            }
            providedRefreshTokenHash = SHA256.HashData(providedRefreshToken);
        }
        catch (FormatException)
        {
            LogInvalidBase64Format();
            return resultNull;
        }
        catch (Exception ex)
        {
            LogExceptionRefreshSession(ex);
            return resultNull;
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
                return resultNull;
            }

            if (session.ExpiresAt < DateTimeOffset.UtcNow)
            {
                LogSessionExpired(session.Id);
                session.IsUsed = true;
                session.InactivatedAt = DateTimeOffset.UtcNow;
                session.UserSessionInactivationReasonId = cache.GetInactivationReasonIdByCode(InactivationReason.EXPIRED);
                _ = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return resultNull;
            }

            Guid UserDeviceId = UserDeviceHelper.ComputeId(dto);
            if (UserDeviceId == Guid.Empty)
            {
                LogSessionUserDeviceIdEmpty();
                return resultNull;
            }

            if (session.UserDeviceId != UserDeviceId)
            {
                LogSessionUserDeviceIdOther(session.UserDeviceId, UserDeviceId);
                await RevokeAllUserSessionsAsync(session.UserId, InactivationReason.OTHER_DEVICE);
                return resultNull;
            }

            session.IsUsed = true;
            session.InactivatedAt = DateTimeOffset.UtcNow;
            session.UserSessionInactivationReasonId = cache.GetInactivationReasonIdByCode(InactivationReason.ROTATION);

            byte[] newRefreshToken = RandomNumberGenerator.GetBytes(TOKEN_SIZE);
            DateTimeOffset dtExpiration = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);
            var newSession = new UserSession
            {
                Id = UuidHelper.NewV7(),
                UserId = session.UserId,
                RefreshTokenHash = SHA256.HashData(newRefreshToken),
                ExpiresAt = dtExpiration,
                IsUsed = false,
                IsRevoked = false,
                UserDeviceId = session.UserDeviceId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _ = dbContext.UserSessions.Add(newSession);
            _ = await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            LogSessionRefreshed(session.Id, newSession.Id);

            return (session.UserId, Convert.ToBase64String(newRefreshToken), dtExpiration);
        }
        catch (DbUpdateConcurrencyException)
        {
            LogConcurrencyConflict();
            await transaction.RollbackAsync();
            return resultNull;
        }
        catch (Exception ex)
        {
            LogExceptionRefreshSession(ex);
            await transaction.RollbackAsync();
            return resultNull;
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
        catch (Exception ex)
        {
            LogExceptionLogout(ex);
            return;
        }


        byte[] refreshTokenHash = SHA256.HashData(refreshTokenBytes);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        int inactivationReasonId = cache.GetInactivationReasonIdByCode(InactivationReason.USER_LOGOUT);
        int affectedRows = await dbContext.UserSessions
            .Where(s => s.RefreshTokenHash == refreshTokenHash && s.ExpiresAt > now)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.UserSessionInactivationReasonId, inactivationReasonId));

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
    public async Task RevokeAllUserSessionsAsync(Guid userId, InactivationReason reason)
    {
        LogAllSessionsRevoked(userId, reason);
        int inactivationReasonId = cache.GetInactivationReasonIdByCode(reason);
        _ = await dbContext.UserSessions
            .Where(s => s.UserId == userId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.UserSessionInactivationReasonId, inactivationReasonId));
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
