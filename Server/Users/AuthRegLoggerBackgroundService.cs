
using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Collections.Concurrent;
using System.Net;

namespace Server.Users;

/// <summary> Фоновый сервис пакетной записи логов авторизации/регистрации и данных устройств с контролируемыми повторными попытками и корректной остановкой. </summary>
public sealed class AuthRegLoggerBackgroundService(
    ILogger<AuthRegLoggerBackgroundService> logger,
    IServiceProvider serviceProvider
    ) : IHostedService, IDisposable
{
    private const int MAX_QUEUE_SIZE = 100_000;
    private const int BATCH_SIZE = 1000;
    private const int MAX_RETRIES = 3;

    /// <summary>
    /// Лог авторизации с поддержкой повторной обработки и кэшированными данными устройства.
    /// </summary>
    private sealed record LogEntry(
        bool success,
        DtoRequestAuthReg dto,
        Guid? userId,
        IPAddress? ip,
        int retryCount,
        DateTimeOffset nextRetryAt,
        bool actionIsAuthentication,
        Guid userDeviceId);

    private readonly ConcurrentQueue<LogEntry> queue = new();
    private readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly CancellationTokenSource internalCts = new();
    private CancellationTokenSource? linkedCts;
    private Task? processingTask;

    /// <summary> Задача на очистку старых логов. </summary>
    private Task? cleanupTask;

    /// <summary>
    /// Добавляет лог в очередь с предварительным вычислением данных устройства.
    /// </summary>
    public void EnqueueLog(bool success, DtoRequestAuthReg dto, Guid? userId, IPAddress? ip, bool actionIsAuthentication)
    {
        if (queue.Count >= MAX_QUEUE_SIZE)
        {
            logger.LogWarning("Очередь логов переполнена. Запись отброшена.");
            return;
        }

        queue.Enqueue(new LogEntry(
            success,
            dto,
            userId,
            ip,
            retryCount: 0,
            nextRetryAt: DateTimeOffset.UtcNow,
            actionIsAuthentication,
            UserDeviceHelper.ComputeUUIDv8(dto)));
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken ct)
    {
        logger.LogInformation("Запуск сервиса фонового логирования.");
        linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, internalCts.Token);

        processingTask = Task.Run(() => ProcessingLoopAsync(linkedCts.Token), linkedCts.Token);

        cleanupTask = null;
        //_cleanupTask = Task.Run(() => CleanupLoopAsync(_linkedCts.Token), _linkedCts.Token);

        return Task.CompletedTask;
    }

    private async Task ProcessingLoopAsync(CancellationToken ct)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(ct).ConfigureAwait(false))
            {
                _ = await ProcessBatchAsync(ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Цикл обработки логов остановлен.");
        }
    }

    private async Task CleanupLoopAsync(CancellationToken ct)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(ct).ConfigureAwait(false))
            {
                await PerformCleanupAsync(ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Цикл очистки старых логов остановлен.");
        }
    }

    /// <summary>
    /// Обрабатывает один пакет логов из очереди.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>True, если пакет успешно записан или очередь была пуста; иначе false.</returns>
    private async Task<bool> ProcessBatchAsync(CancellationToken ct)
    {
        if (queue.IsEmpty)
        {
            return true;
        }

        if (!await semaphore.WaitAsync(TimeSpan.FromSeconds(5), ct).ConfigureAwait(false))
        {
            return false;
        }

        try
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            List<LogEntry> batch = [];

            // Оптимизированное извлечение с учетом времени повтора
            while (batch.Count < BATCH_SIZE && queue.TryPeek(out LogEntry? peekEntry))
            {
                if (peekEntry.nextRetryAt > now)
                {
                    break;
                }

                if (queue.TryDequeue(out LogEntry? entry))
                {
                    batch.Add(entry);
                }
                else
                {
                    break;
                }
            }

            if (batch.Count == 0)
            {
                return true;
            }

            bool success = await WriteBatchToDatabaseAsync(batch, ct).ConfigureAwait(false);

            if (!success)
            {
                foreach (LogEntry item in batch)
                {
                    if (item.retryCount + 1 < MAX_RETRIES)
                    {
                        var delay = TimeSpan.FromSeconds(Math.Pow(2, item.retryCount + 1));
                        queue.Enqueue(item with
                        {
                            retryCount = item.retryCount + 1,
                            nextRetryAt = DateTimeOffset.UtcNow.Add(delay)
                        });
                    }
                    else
                    {
                        if (logger.IsEnabled(LogLevel.Error))
                        {
                            logger.LogError("Лог отброшен после {Retries} попыток.", MAX_RETRIES);
                        }
                    }
                }
            }

            return success;
        }
        finally
        {
            _ = semaphore.Release();
        }
    }

    private async Task<bool> WriteBatchToDatabaseAsync(List<LogEntry> batch, CancellationToken ct)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();

        try
        {
            List<Guid> uniqueDevicesId = [.. batch.Where(l => l.userDeviceId != Guid.Empty)
                .DistinctBy(l => l.userDeviceId)
                .Select(a=>a.userDeviceId)];
            for (int i = uniqueDevicesId.Count - 1; i >= 0; i--)
            {
                if (db.userDevices.Any(a => a.id == uniqueDevicesId[i]))
                {
                    uniqueDevicesId.RemoveAt(i);
                }
            }

            List<UserDevice> uniqueDevices = [.. batch
                .Where(l => l.userDeviceId != Guid.Empty && uniqueDevicesId.Any(a=>a == l.userDeviceId))
                .DistinctBy(l => l.userDeviceId)
                .Select(item => UserDeviceHelper.DtoToUserDevice(item.dto, item.userDeviceId))];

            if (uniqueDevices.Count > 0)
            {
                List<Guid> deviceIds = [.. uniqueDevices.Select(d => d.id)];

                HashSet<Guid> existingIds = await db.userDevices
                    .Where(d => deviceIds.Contains(d.id))
                    .Select(d => d.id)
                    .ToHashSetAsync(ct).ConfigureAwait(false);

                foreach (UserDevice device in uniqueDevices)
                {
                    if (!existingIds.Contains(device.id))
                    {
                        _ = await db.userDevices.AddAsync(device, ct).ConfigureAwait(false);
                    }
                }
            }

            foreach (LogEntry item in batch)
            {
                _ = await db.authenticationLogs.AddAsync(new Server_DB_Postgres.Entities.Logs.AuthenticationLog
                {
                    id = UUID.CreateV7(),
                    email = item.dto.email,
                    success = item.success,
                    userId = item.userId,
                    userDeviceId = item.userDeviceId,
                    createdAt = DateTimeOffset.UtcNow,
                    ip = item.ip,
                }, ct).ConfigureAwait(false);
            }

            _ = await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(ex, "Ошибка записи батча ({Count} записей).", batch.Count);
            }

            return false;
        }
    }

    private async Task PerformCleanupAsync(CancellationToken ct)
    {
        //ct.ThrowIfCancellationRequested();

        using IServiceScope scope = serviceProvider.CreateScope();
        DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();

        try
        {
            DateTimeOffset cutoff = DateTimeOffset.UtcNow.AddMonths(-24);
            int deleted = await db.authenticationLogs
                .Where(a => a.createdAt < cutoff)
                .ExecuteDeleteAsync(ct).ConfigureAwait(false);

            if (deleted > 0)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Очистка завершена: удалено {Count} записей.", deleted);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при очистке старых логов.");
        }
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken ct)
    {
        logger.LogInformation("Запрос на остановку сервиса логирования...");
        await internalCts.CancelAsync().ConfigureAwait(false);

        // Дожидаемся graceful завершения основных циклов
        if (processingTask != null)
        {
            await processingTask.WaitAsync(ct).ContinueWith(_ => { }, TaskScheduler.Default).ConfigureAwait(false);
        }

        if (cleanupTask != null)
        {
            await cleanupTask.WaitAsync(ct).ContinueWith(_ => { }, TaskScheduler.Default).ConfigureAwait(false);
        }

        // "Умный" финальный flush: до 20 попыток, но не более 2 ошибок БД подряд
        int consecutiveFailures = 0;
        const int maxConsecutiveFailures = 2;

        while (!queue.IsEmpty && !ct.IsCancellationRequested && consecutiveFailures < maxConsecutiveFailures)
        {
            bool success = await ProcessBatchAsync(ct).ConfigureAwait(false);

            if (success)
            {
                consecutiveFailures = 0;
            }
            else
            {
                consecutiveFailures++;
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning("Сбой записи при остановке (ошибка {Count}/{Max}).", consecutiveFailures, maxConsecutiveFailures);
                }
            }

            if (!queue.IsEmpty && !ct.IsCancellationRequested)
            {
                await Task.Delay(success ? 100 : 1000, ct).ConfigureAwait(false);
            }
        }

        if (!queue.IsEmpty)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("При остановке осталось {Count} необработанных логов.", queue.Count);
            }
        }
        else
        {
            logger.LogInformation("Все логи успешно записаны при остановке.");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        linkedCts?.Dispose();
        internalCts.Dispose();
        semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
