using FluentResults;
using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using Server.Cache;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using System.Security.Cryptography;
using System.Threading;
using static General.LocalizationKeys.Error;

namespace Server.Users.Authentication;

public record SessionResponseData(Guid UserId, string RefreshToken, DateTimeOffset ExpiresAt);

/// <summary>
/// Сервис управления игровыми сессиями и ротацией токенов.
/// </summary>
public sealed partial class SessionService(
    DbContextGame dbContext,
    ILogger<SessionService> logger,
    CacheService cacheService,
    IMemoryCache memoryCache)
{
    private const int TOKEN_SIZE = 32;
    private const int BASE_64_TOKEN_LENGTH = 44;
    private static readonly TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(14);

    private readonly int inactivationReasonIdRotation = cacheService.GetInactivationReasonIdByCode(InactivationReason.Rotation);
    private readonly int inactivationReasonIdUserLogout = cacheService.GetInactivationReasonIdByCode(InactivationReason.UserLogout);
    private readonly int inactivationReasonIdServerRevoke = cacheService.GetInactivationReasonIdByCode(InactivationReason.ServerRevoke);
    private readonly int inactivationReasonIdExpired = cacheService.GetInactivationReasonIdByCode(InactivationReason.Expired);

    #region Compiled Queries
    // Предварительно скомпилированный запрос для поиска сессии по хешу
    private static readonly Func<DbContextGame, byte[], CancellationToken, Task<UserSession?>> GetSessionByHashQuery =
        EF.CompileAsyncQuery((DbContextGame db, byte[] hash, CancellationToken ct) =>
            db.UserSessions.FirstOrDefault(s => s.RefreshTokenHash == hash));

    // Предварительно скомпилированный запрос для проверки существования устройства
    private static readonly Func<DbContextGame, Guid, CancellationToken, Task<bool>> DeviceExistsQuery =
        EF.CompileAsyncQuery((DbContextGame db, Guid id, CancellationToken ct) =>
            db.UserDevices.Any(d => d.Id == id));
    #endregion

    #region LoggerMessages
    [LoggerMessage(Level = LogLevel.Warning, Message = "Token Reuse detected! UserId: {userId}, Session: {sessionId}")]
    private partial void LogTokenReuse(Guid userId, Guid sessionId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Refresh error: {msg}")]
    private partial void LogRefreshError(string msg, Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Device mismatch for session {sessionId}")]
    private partial void LogDeviceMismatch(Guid sessionId);
    #endregion

    /// <summary>
    /// Создает новую сессию с оптимизированным кэшированием устройств.
    /// </summary>
    public async Task<Result<SessionResponseData>> CreateSessionAsync(Guid userId, DtoRequestAuthReg dto, CancellationToken cancellationToken)
    {
        Guid deviceId = UserDeviceHelper.ComputeId(dto);
        if (deviceId == Guid.Empty)
        {
            return Result.Fail("Invalid device data");
        }

        Result deviceResult = await EnsureDeviceExistsCachedAsync(deviceId, dto, cancellationToken).ConfigureAwait(false);
        if (deviceResult.IsFailed)
        {
            return deviceResult.ToResult<SessionResponseData>();
        }

        byte[] hash;
        string refreshTokenBase64;
        DateTimeOffset expiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);

        // Ограничиваем область видимости Span, чтобы он не попал в State Machine асинхронного метода
        {
            Span<byte> rawToken = stackalloc byte[TOKEN_SIZE];
            RandomNumberGenerator.Fill(rawToken);

            // Конвертируем данные в типы, которые можно хранить в куче, ДО await
            hash = SHA256.HashData(rawToken);
            refreshTokenBase64 = Convert.ToBase64String(rawToken);
        }

        UserSession session = new()
        {
            Id = UuidHelper.NewV7(),
            UserId = userId,
            RefreshTokenHash = hash,
            ExpiresAt = expiresAt,
            UserDeviceId = deviceId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _ = dbContext.UserSessions.Add(session);

        _ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result.Ok(new SessionResponseData(userId, refreshTokenBase64, expiresAt));
    }

    /// <summary>
    /// Обновляет сессию с использованием Compiled Queries и Span-валидации.
    /// </summary>
    public async Task<Result<SessionResponseData>> RefreshSessionAsync(DtoRequestAuthReg dto, CancellationToken cancellationToken)
    {
        if (dto.RefreshToken?.Length != BASE_64_TOKEN_LENGTH)
        {
            return Result.Fail("Invalid token length");
        }

        Span<byte> rawToken = stackalloc byte[TOKEN_SIZE];
        if (!Convert.TryFromBase64String(dto.RefreshToken, rawToken, out _))
        {
            return Result.Fail("Invalid Base64");
        }

        Span<byte> hashBuffer = stackalloc byte[32];
        _ = SHA256.HashData(rawToken, hashBuffer);
        byte[] hashArray = hashBuffer.ToArray();

        UserSession? session = await GetSessionByHashQuery(dbContext, hashArray, cancellationToken).ConfigureAwait(false);

        if (session == null)
        {
            return Result.Fail("Session not found");
        }

        // Проверка безопасности (Token Reuse Detection)
        if (session.IsUsed || session.IsRevoked)
        {
            LogTokenReuse(session.UserId, session.Id);
            await RevokeAllUserSessionsAsync(session.UserId, cancellationToken).ConfigureAwait(false);
            return Result.Fail("Security risk: Token reuse");
        }

        if (session.ExpiresAt < DateTimeOffset.UtcNow)
        {
            _ = await dbContext.UserSessions
            .Where(s => s.Id == session.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.UserSessionInactivationReasonId, inactivationReasonIdExpired),
                cancellationToken).ConfigureAwait(false);
            return Result.Fail("Expired");
        }

        Guid currentDeviceId = UserDeviceHelper.ComputeId(dto);
        if (session.UserDeviceId != currentDeviceId)
        {
            LogDeviceMismatch(session.Id);
            return Result.Fail("Device mismatch");
        }

        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            session.IsUsed = true;
            session.InactivatedAt = DateTimeOffset.UtcNow;
            session.UserSessionInactivationReasonId = inactivationReasonIdRotation;

            DateTimeOffset nextExpiry = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);
            string nextRawTokenBase64;
            byte[] nextTokenHash;

            // Ограничиваем область жизни Span этим блоком. Внутри блока НЕТ вызовов await.
            {
                Span<byte> nextRawToken = stackalloc byte[TOKEN_SIZE];
                RandomNumberGenerator.Fill(nextRawToken);

                // Сразу вычисляем хеш и Base64, которые могут пережить await
                nextTokenHash = SHA256.HashData(nextRawToken);
                nextRawTokenBase64 = Convert.ToBase64String(nextRawToken);

                // Как только мы вышли за скобку, Span "умирает", и await ниже становится безопасным
            }

            UserSession nextSession = new()
            {
                Id = UuidHelper.NewV7(),
                UserId = session.UserId,
                RefreshTokenHash = nextTokenHash,
                ExpiresAt = nextExpiry,
                UserDeviceId = session.UserDeviceId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _ = dbContext.UserSessions.Add(nextSession);

            // Теперь await безопасен, так как Span больше не существует в контексте метода
            _ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

            return Result.Ok(new SessionResponseData(session.UserId, nextRawTokenBase64, nextExpiry));
        }
        catch (Exception ex)
        {
            LogRefreshError(ex.Message, ex);
            return Result.Fail("Internal error");
        }
    }

    public async Task<Result> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Result.Fail("Empty token");
        }

        Span<byte> tokenBytes = stackalloc byte[TOKEN_SIZE];
        if (!Convert.TryFromBase64String(refreshToken, tokenBytes, out _))
        {
            return Result.Fail("Invalid token format");
        }

        byte[] hash = SHA256.HashData(tokenBytes);

        int affected = await dbContext.UserSessions
            .Where(s => s.RefreshTokenHash == hash)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.UserSessionInactivationReasonId, inactivationReasonIdUserLogout), cancellationToken: cancellationToken).ConfigureAwait(false);

        return affected > 0 ? Result.Ok() : Result.Fail("Session not found");
    }

    /// <summary>
    /// Оптимизированная проверка устройства через IMemoryCache и string.Create.
    /// </summary>
    private async Task<Result> EnsureDeviceExistsCachedAsync(Guid deviceId, DtoRequestAuthReg dto, CancellationToken cancellationToken)
    {
        // Оптимизация ключа кэша через string.Create (40 символов: "dev_" + 36 символов GUID)
        string cacheKey = string.Create(40, deviceId, static (span, id) =>
        {
            "dev_".AsSpan().CopyTo(span);
            _ = id.TryFormat(span[4..], out _, "D");
        });

        if (memoryCache.TryGetValue(cacheKey, out _))
        {
            return Result.Ok();
        }

        // Используем Compiled Query для БД
        if (!await DeviceExistsQuery(dbContext, deviceId,cancellationToken).ConfigureAwait(false))
        {
            UserDevice newDevice = UserDeviceHelper.DtoToUserDevice(dto, deviceId);
            _ = dbContext.UserDevices.Add(newDevice);
            try
            {
                _ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
            {
                dbContext.Entry(newDevice).State = EntityState.Detached;
            }
        }

        _ = memoryCache.Set(cacheKey, true, TimeSpan.FromMinutes(30));
        return Result.Ok();
    }

    /// <summary>
    /// Отзывает все сессии.
    /// </summary>
    private async Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        _ = await dbContext.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.IsRevoked, true)
                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
                .SetProperty(s => s.UserSessionInactivationReasonId, inactivationReasonIdServerRevoke)
                , cancellationToken: cancellationToken).ConfigureAwait(false);
    }
    
}
//using FluentResults;
//using General.DTO.RestRequest;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.Caching.Memory;
//using Npgsql;
//using Server.Cache;
//using Server.Utilities;
//using Server_DB_Postgres;
//using Server_DB_Postgres.Entities.Server;
//using Server_DB_Postgres.Entities.Users;
//using System.Security.Cryptography;

//namespace Server.Users.Authentication;

//public record SessionResponseData(Guid UserId, string RefreshToken, DateTimeOffset ExpiresAt);
//public sealed partial class SessionService(
//    DbContextGame dbContext,
//    ILogger<SessionService> logger,
//    CacheService cacheService,
//    IMemoryCache memoryCache)
//{
//    private const int TOKEN_SIZE = 32;
//    private const int BASE64_TOKEN_LENGTH = 44;
//    private static readonly TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(14);

//    #region LoggerMessages

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Передан пустой refresh token")]
//    private partial void LogEmptyToken();

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Некорректный размер токена: {length} байт (ожидается {expectedSize})")]
//    private partial void LogInvalidTokenSize(int length, int expectedSize);

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Некорректный формат base64 токена")]
//    private partial void LogInvalidBase64Format();

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Сессия не найдена или недействительна")]
//    private partial void LogSessionNotFound();

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Сессия {sessionId} истекла")]
//    private partial void LogSessionExpired(Guid sessionId);


//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Предыдущий userDeviceId=[{prev}] не совпадает с новым [{now}]")]
//    private partial void LogSessionUserDeviceIdOther(Guid prev, Guid now);

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Сессия обновлена. Старая: {oldSessionId}, Новая: {newSessionId}")]
//    private partial void LogSessionRefreshed(Guid oldSessionId, Guid newSessionId);

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Конфликт параллельного обновления сессии")]
//    private partial void LogConcurrencyConflict();

//    [LoggerMessage(
//        Level = LogLevel.Error,
//        Message = "Ошибка при обновлении сессии")]
//    private partial void LogExceptionRefreshSession(Exception ex);

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Попытка выхода с несуществующим или уже отозванным токеном")]
//    private partial void LogInvalidLogout();

//    [LoggerMessage(Level = LogLevel.Warning)]
//    private partial void LogExceptionLogout(Exception ex);

//    [LoggerMessage(
//        Level = LogLevel.Information,
//        Message = "Все сессии пользователя {userId} отозваны. Причина: {reason}")]
//    private partial void LogAllSessionsRevoked(Guid userId, InactivationReason reason);

//    [LoggerMessage(
//        Level = LogLevel.Debug,
//        Message = "Устройство {deviceId} уже добавлено другим потоком")]
//    private partial void LogDeviceAlreadyAdded(Guid deviceId);

//    [LoggerMessage(
//        Level = LogLevel.Error,
//        Message = "Ошибка при получении/добавлении устройства {deviceId}")]
//    private partial void LogDeviceError(Exception ex, Guid deviceId);

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Попытка повторного использования токена (Token Reuse)! Пользователь: {userId}, Сессия: {sessionId}. Все сессии будут отозваны.")]
//    private partial void LogTokenReuseAttempt(Guid userId, Guid sessionId);

//    [LoggerMessage(Level = LogLevel.Error, Message = "Ошибка при обновлении сессии")]
//    private partial void LogRefreshError(Exception ex);

//    #region LoggerMessages

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Попытка повторного использования токена! User: {userId}, Session: {sessionId}. Все сессии пользователя отозваны.")]
//    private partial void LogTokenReuseDetected(Guid userId, Guid sessionId);

//    [LoggerMessage(
//        Level = LogLevel.Error,
//        Message = "Ошибка при создании сессии для пользователя {userId}")]
//    private partial void LogSessionCreateError(Exception ex, Guid userId);

//    [LoggerMessage(
//        Level = LogLevel.Error,
//        Message = "Ошибка при регистрации устройства {deviceId}")]
//    private partial void LogDeviceCreateError(Exception ex, Guid deviceId);


//    [LoggerMessage(
//        Level = LogLevel.Information,
//        Message = "Создана новая сессия {sessionId} для пользователя {userId}")]
//    private partial void LogSessionCreated(Guid sessionId, Guid userId);

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Не удалось вычислить ID устройства. Поля DTO пусты.")]
//    private partial void LogSessionUserDeviceIdEmpty();

//    [LoggerMessage(
//        Level = LogLevel.Warning,
//        Message = "Некорректный формат Base64 или размер токена.")]
//    private partial void LogInvalidTokenFormat();

//    #endregion LoggerMessages
//    #endregion

//    #region Compiled Queries
//    /// <summary>
//    /// Предварительно скомпилированный запрос для поиска сессии по хешу
//    /// </summary>
//    private static readonly Func<DbContextGame, byte[], Task<UserSession?>> GetSessionByHashQuery =
//        EF.CompileAsyncQuery((DbContextGame db, byte[] hash) =>
//            db.UserSessions.FirstOrDefault(s => s.RefreshTokenHash == hash));

//    /// <summary>
//    /// Предварительно скомпилированный запрос для проверки существования устройства
//    /// </summary>
//    private static readonly Func<DbContextGame, Guid, Task<bool>> DeviceExistsQuery =
//        EF.CompileAsyncQuery((DbContextGame db, Guid id) =>
//            db.UserDevices.Any(d => d.Id == id));
//    #endregion

//    /// <summary>
//    /// Создает новую сессию для пользователя.
//    /// </summary>
//    /// <param name="userId">ID пользователя.</param>
//    /// <param name="dto">DTO с данными об устройстве.</param>
//    /// <returns>Результат с данными сессии.</returns>
//    public async Task<Result<SessionResponseData>> CreateSessionAsync(Guid userId, DtoRequestAuthReg dto)
//    {
//        // 1. Работа с устройством (Zero-allocation идентификация)
//        Guid deviceId = UserDeviceHelper.ComputeId(dto);
//        if (deviceId == Guid.Empty) return Result.Fail("Invalid device data");

//        // Проверяем существование устройства. Если нет — добавляем.
//        // Метод GetOrAddDeviceId оптимизирован внутри (см. ниже).
//        Result deviceResult = await EnsureDeviceExistsAsync(deviceId, dto);
//        if (deviceResult.IsFailed)
//        {
//            // Явно приводим неудачный Result к Result<SessionResponseData>
//            return deviceResult.ToResult<SessionResponseData>();
//        }
//        // 2. Генерация нового токена на стеке
//        Span<byte> rawTokenBytes = stackalloc byte[TOKEN_SIZE];
//        RandomNumberGenerator.Fill(rawTokenBytes);

//        // Хешируем для хранения в БД
//        byte[] tokenHash = SHA256.HashData(rawTokenBytes);

//        DateTimeOffset expiration = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);

//        // 3. Сохранение в БД
//        UserSession session = new()
//        {
//            Id = UuidHelper.NewV7(),
//            UserId = userId,
//            RefreshTokenHash = tokenHash,
//            ExpiresAt = expiration,
//            UserDeviceId = deviceId,
//            IsUsed = false,
//            IsRevoked = false,
//            CreatedAt = DateTimeOffset.UtcNow
//        };

//        try
//        {
//            _ = dbContext.UserSessions.Add(session);
//            _ = await dbContext.SaveChangesAsync();

//            LogSessionCreated(session.Id, userId);

//            // Возвращаем результат. Base64 создается один раз при выходе.
//            return Result.Ok(new SessionResponseData(
//                userId,
//                Convert.ToBase64String(rawTokenBytes),
//                expiration));
//        }
//        catch (Exception ex)
//        {
//            LogSessionCreateError(ex, userId);
//            return Result.Fail("Failed to save session to database");
//        }
//    }

//    /// <summary>
//    /// Проверяет наличие устройства и добавляет его, если оно отсутствует.
//    /// </summary>
//    private async Task<Result> EnsureDeviceExistsAsync(Guid deviceId, DtoRequestAuthReg dto)
//    {
//        // Быстрая проверка без создания сущности
//        if (await dbContext.UserDevices.AnyAsync(d => d.Id == deviceId))
//        {
//            return Result.Ok();
//        }

//        UserDevice newDevice = UserDeviceHelper.DtoToUserDevice(dto, deviceId);
//        _ = dbContext.UserDevices.Add(newDevice);

//        try
//        {
//            _ = await dbContext.SaveChangesAsync();
//            return Result.Ok();
//        }
//        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
//        {
//            // Обработка гонки потоков (Race condition): устройство уже добавлено другим запросом
//            dbContext.Entry(newDevice).State = EntityState.Detached;
//            return Result.Ok();
//        }
//        catch (Exception ex)
//        {
//            LogDeviceCreateError(ex, deviceId);
//            return Result.Fail("Failed to register device");
//        }
//    }

//    public async Task<Result<SessionResponseData>> RefreshSessionAsync(DtoRequestAuthReg dto)
//    {
//        // 1. Валидация формата без аллокаций в куче
//        if (string.IsNullOrWhiteSpace(dto.RefreshToken) || dto.RefreshToken.Length != BASE64_TOKEN_LENGTH)
//        {
//            //LogInvalidTokenFormat();
//            return Result.Fail("Invalid token format");
//        }

//        Span<byte> providedTokenBytes = stackalloc byte[TOKEN_SIZE];
//        if (!Convert.TryFromBase64String(dto.RefreshToken, providedTokenBytes, out int bytesWritten) || bytesWritten != TOKEN_SIZE)
//        {
//            //LogInvalidTokenFormat();
//            return Result.Fail("Invalid base64 or size");
//        }


//        Span<byte> hashBuffer = stackalloc byte[32];// Выделяем 32 байта под хеш на стеке
//        _ = SHA256.TryHashData(providedTokenBytes, hashBuffer, out _);// Хешируем напрямую из Span в Span. Ноль аллокаций.
//        byte[] providedHash = hashBuffer.ToArray();
//        //byte[] providedHash = SHA256.HashData(providedTokenBytes);

//        // 2. Транзакция для атомарности ротации
//        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
//        try
//        {
//            UserSession? session = await GetSessionByHashQuery(dbContext, providedHash);
//            if (session == null)
//            {
//                return Result.Fail("Session not found");
//            }

//            if (session.IsRevoked)
//            {
//                return Result.Fail("Revoked");
//            }

//            if (session.IsUsed)
//            {
//                return Result.Fail("ReuseDetected");
//            }

//            // 3. Проверка на Token Reuse (Атака повторного воспроизведения)
//            if (session.IsUsed || session.IsRevoked)
//            {
//                LogTokenReuseDetected(session.UserId, session.Id);
//                await RevokeAllUserSessionsAsync(session.UserId, InactivationReason.ROTATION);
//                _ = await dbContext.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return Result.Fail("Security risk: Token already used");
//            }

//            if (session.ExpiresAt < DateTimeOffset.UtcNow)
//            {
//                return Result.Fail("Token expired");
//            }

//            // 4. Проверка привязки к устройству
//            Guid requestDeviceId = UserDeviceHelper.ComputeId(dto);
//            if (session.UserDeviceId != requestDeviceId)
//            {
//                await RevokeAllUserSessionsAsync(session.UserId, InactivationReason.OTHER_DEVICE);
//                _ = await dbContext.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return Result.Fail("Device mismatch");
//            }

//            // 5. Ротация токена
//            session.IsUsed = true;
//            session.InactivatedAt = DateTimeOffset.UtcNow;
//            session.UserSessionInactivationReasonId = cacheService.GetInactivationReasonIdByCode(InactivationReason.ROTATION);

//            byte[] newRawToken = RandomNumberGenerator.GetBytes(TOKEN_SIZE);
//            DateTimeOffset expiration = DateTimeOffset.UtcNow.Add(RefreshTokenLifeTime);

//            UserSession newSession = new()
//            {
//                Id = UuidHelper.NewV7(),
//                UserId = session.UserId,
//                RefreshTokenHash = SHA256.HashData(newRawToken),
//                ExpiresAt = expiration,
//                UserDeviceId = session.UserDeviceId,
//                CreatedAt = DateTimeOffset.UtcNow
//            };

//            _ = dbContext.UserSessions.Add(newSession);
//            _ = await dbContext.SaveChangesAsync();
//            await transaction.CommitAsync();

//            return Result.Ok(new SessionResponseData(
//                session.UserId,
//                Convert.ToBase64String(newRawToken),
//                expiration));
//        }
//        catch (Exception ex)
//        {
//            LogRefreshError(ex);
//            return Result.Fail(new Error("Internal error during refresh").CausedBy(ex));
//        }

//    }



//    /// <summary>
//    /// Отзывает все сессии пользователя.
//    /// </summary>
//    public async Task RevokeAllUserSessionsAsync(Guid userId, InactivationReason reason)
//    {
//        int reasonId = cacheService.GetInactivationReasonIdByCode(reason);

//        _ = await dbContext.UserSessions
//            .Where(s => s.UserId == userId && !s.IsRevoked)
//            .ExecuteUpdateAsync(set => set
//                .SetProperty(s => s.IsRevoked, true)
//                .SetProperty(s => s.InactivatedAt, DateTimeOffset.UtcNow)
//                .SetProperty(s => s.UserSessionInactivationReasonId, reasonId));
//    }

//    private async Task<Guid?> GetOrAddDeviceId(DtoRequestAuthReg dto)
//    {
//        Guid id = UserDeviceHelper.ComputeId(dto);

//        try
//        {
//            string cacheKey = string.Create(45, id, static (span, guid) =>
//            {
//                "deviceId_".AsSpan().CopyTo(span);
//                // Записываем Guid напрямую в Span строки в формате "D" (с дефисами)
//                _ = guid.TryFormat(span[9..], out _, "D");
//            });
//            if (!memoryCache.TryGetValue(cacheKey, out _))
//            {
//                if (await DeviceExistsQuery(dbContext, id))
//                {
//                    _ = memoryCache.Set(cacheKey, true, TimeSpan.FromMinutes(30));
//                }
//                else
//                {
//                    UserDevice newDevice = UserDeviceHelper.DtoToUserDevice(dto, id);
//                    _ = dbContext.UserDevices.Add(newDevice);

//                    try
//                    {
//                        _ = await dbContext.SaveChangesAsync();
//                    }
//                    catch (DbUpdateException ex)
//                        when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
//                    {
//                        dbContext.Entry(newDevice).State = EntityState.Detached;
//                        LogDeviceAlreadyAdded(id);
//                    }
//                }
//            }

//            return id;
//        }
//        catch (Exception ex)
//        {
//            LogDeviceError(ex, id);
//            return null;
//        }
//    }
//}
