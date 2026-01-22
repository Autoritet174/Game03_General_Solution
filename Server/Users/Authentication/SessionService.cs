using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Security.Cryptography;

namespace Server.Users.Authentication;

public sealed class SessionService(DbContextGame dbContext, ILogger<SessionService> logger)
{
    private const int TOKEN_SIZE = 32;

    private static readonly TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(14);

    /// <summary>
    /// Создает новую сессию и возвращает refresh token в base64.
    /// </summary>
    public async Task<string?> CreateSessionAsync(Guid userId, DtoRequestAuthReg? dto = null)
    {
        if (dto == null)
        {
            return null;
        }

        Guid? UserDeviceId = await GetOrAddDeviceId(dto);
        if (UserDeviceId == null)
        {
            return null;
        }

        byte[] tokenBytes = RandomNumberGenerator.GetBytes(TOKEN_SIZE);
        var session = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenBytes,
            ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime),
            IsUsed = false,
            IsRevoked = false,
            UserDeviceId = UserDeviceId.Value,
        };

        _ = dbContext.UserSessions.Add(session);
        _ = await dbContext.SaveChangesAsync();

        return Convert.ToBase64String(tokenBytes);
    }

    /// <summary>
    /// Обновляет существующую сессию (rotation + reuse detection).
    /// </summary>
    /// <summary>
    /// Обновляет существующую сессию (rotation + reuse detection).
    /// </summary>
    public async Task<(Guid? UserId, string? NewRefreshToken)> RefreshSessionAsync(string oldTokenBase64)
    {
        // Валидация входных данных
        if (string.IsNullOrWhiteSpace(oldTokenBase64))
        {
            logger.LogDebug("Передан пустой refresh token");
            return (null, null);
        }

        // Декодирование токена с обработкой ошибок
        byte[] providedHash;
        try
        {
            providedHash = Convert.FromBase64String(oldTokenBase64);
            if (providedHash.Length != TOKEN_SIZE)
            {
                logger.LogWarning("Некорректный размер токена: {Length} байт (ожидается {ExpectedSize})",
                    providedHash.Length, TOKEN_SIZE);
                return (null, null);
            }
        }
        catch (FormatException)
        {
            logger.LogWarning("Некорректный формат base64 токена");
            return (null, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при декодировании токена");
            return (null, null);
        }

        // Используем транзакцию для обеспечения атомарности
        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.ReadCommitted); // Высокий уровень изоляции для предотвращения race condition


        try
        {
            // Ищем сессию по токену
            UserSession? session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.TokenHash == providedHash); // тут работает HasQueryFilter(s => !s.IsUsed && !s.IsRevoked)

            if (session == null)
            {
                logger.LogDebug("Сессия не найдена");
                await transaction.RollbackAsync();
                return (null, null);
            }


            // Проверка срока действия
            if (session.ExpiresAt < DateTimeOffset.UtcNow)
            {
                logger.LogDebug("Сессия {SessionId} истекла", session.Id);
                session.IsUsed = true;
                session.InactivatedAt = DateTimeOffset.UtcNow;
                session.InactivationReason = "EXPIRED";
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return (null, null);
            }

            // Деактивация старого токена
            session.IsUsed = true;
            session.InactivatedAt = DateTimeOffset.UtcNow;
            session.InactivationReason = "ROTATION";

            // Создание нового токена
            byte[] newTokenBytes = RandomNumberGenerator.GetBytes(TOKEN_SIZE);
            var newSession = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = session.UserId,
                TokenHash = newTokenBytes,
                ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime),
                IsUsed = false,
                IsRevoked = false,
                UserDeviceId = session.UserDeviceId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _ = dbContext.UserSessions.Add(newSession);
            _ = await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogDebug("Сессия обновлена. Старая: {OldId}, Новая: {NewId}",
            session.Id, newSession.Id);

            return (session.UserId, Convert.ToBase64String(newTokenBytes));
        }
        catch (NpgsqlException ex) when (ex.SqlState == "55P03") // lock_not_available
        {
            // Другой запрос уже блокировал эту строку
            logger.LogWarning("Не удалось получить блокировку на сессию");
            await transaction.RollbackAsync();
            return (null, null);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Race condition: другой поток уже обновил эту сессию
            // Это нормальная ситуация при параллельных запросах
            logger.LogError(ex, "Конфликт версий при обновлении сессии");
            await transaction.RollbackAsync();
            return (null, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении сессии");
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task RevokeAllUserSessionsAsync(Guid userId, string reason)
    {
        _ = await dbContext.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, reason));
    }



    /// <summary>
    /// Завершает конкретную сессию пользователя.
    /// </summary>
    /// <param name="refreshTokenBase64">Токен, который нужно аннулировать.</param>
    public async Task LogoutAsync(string refreshTokenBase64)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshTokenBase64);

        byte[] tokenHash = Convert.FromBase64String(refreshTokenBase64);

        // используем execute_update для мгновенного обновления без загрузки сущности в память
        int affectedRows = await dbContext.UserSessions
            .Where(s => s.TokenHash == tokenHash && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.InactivationReason, "USER_LOGOUT"));

        if (affectedRows == 0)
        {
            logger.LogWarning("Попытка выхода с несуществующим или уже отозванным токеном.");
        }
    }

    /// <summary>
    /// Принудительно завершает все сессии пользователя (например, при смене пароля).
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
            logger.LogInformation("Все сессии пользователя {userId} отозваны. причина: {reason}", userId, reason);
        }

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
                catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    if (logger.IsEnabled(LogLevel.Debug))
                    {
                        // В случае race condition, если другой поток уже добавил устройство
                        logger.LogDebug("Устройство {DeviceId} уже добавлено другим потоком", id);
                    }
                }
            }

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении/добавлении устройства {DeviceId}", id);
            // В случае ошибки возвращаем пустой GUID или бросаем исключение
            return null;
        }
    }
}
