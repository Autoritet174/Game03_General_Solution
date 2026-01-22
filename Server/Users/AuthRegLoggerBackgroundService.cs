using General.DTO.RestRequest;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Server.Users;

/// <summary> Фоновый сервис пакетной записи логов авторизации/регистрации и данных устройств с контролируемыми повторными попытками и корректной остановкой. </summary>
public sealed class AuthRegLoggerBackgroundService(ILogger<AuthRegLoggerBackgroundService> logger, IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    /// <summary>
    /// Лог авторизации с поддержкой повторной обработки и кэшированными данными устройства.
    /// </summary>
    private sealed record LogEntry(
        bool Success,
        DtoRequestAuthReg dto,
        Guid? UserId,
        IPAddress? Ip,
        int RetryCount,
        DateTimeOffset NextRetryAt,
        bool ActionIsAuthentication,
        Guid UserDeviceId);

    private readonly ILogger<AuthRegLoggerBackgroundService> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly ConcurrentQueue<LogEntry> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly CancellationTokenSource _internalCts = new();
    private CancellationTokenSource? _linkedCts;
    private Task? _processingTask;
    private Task? _cleanupTask;

    private const int MAX_QUEUE_SIZE = 10_000;
    private const int BATCH_SIZE = 100;
    private const int MAX_RETRIES = 3;

    /// <summary>
    /// Добавляет лог в очередь с предварительным вычислением данных устройства.
    /// </summary>
    /// <exception cref="ArgumentNullException">Бросается, если obj равен null.</exception>
    public void EnqueueLog(bool success, DtoRequestAuthReg dto, Guid? userId, IPAddress? ip, bool actionIsAuthentication)
    {
        if (_queue.Count >= MAX_QUEUE_SIZE)
        {
            _logger.LogWarning("Очередь логов переполнена. Запись отброшена.");
            return;
        }

        _queue.Enqueue(new LogEntry(
            success,
            dto,
            userId,
            ip,
            RetryCount: 0,
            NextRetryAt: DateTimeOffset.UtcNow,
            actionIsAuthentication,
            UserDeviceHelper.ComputeId(dto)));
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken ct)
    {
        _logger.LogInformation("Запуск сервиса фонового логирования.");
        _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, _internalCts.Token);

        _processingTask = Task.Run(() => ProcessingLoopAsync(_linkedCts.Token), _linkedCts.Token);
        _cleanupTask = Task.Run(() => CleanupLoopAsync(_linkedCts.Token), _linkedCts.Token);

        return Task.CompletedTask;
    }

    private async Task ProcessingLoopAsync(CancellationToken ct)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(ct))
            {
                _ = await ProcessBatchAsync(ct);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Цикл обработки логов остановлен.");
        }
    }

    private async Task CleanupLoopAsync(CancellationToken ct)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

        try
        {
            while (await timer.WaitForNextTickAsync(ct))
            {
                await PerformCleanupAsync(ct);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Цикл очистки старых логов остановлен.");
        }
    }

    /// <summary>
    /// Обрабатывает один пакет логов из очереди.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>True, если пакет успешно записан или очередь была пуста; иначе false.</returns>
    private async Task<bool> ProcessBatchAsync(CancellationToken ct)
    {
        if (_queue.IsEmpty)
        {
            return true;
        }

        if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(5), ct))
        {
            return false;
        }

        try
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            List<LogEntry> batch = [];

            // Оптимизированное извлечение с учетом времени повтора
            while (batch.Count < BATCH_SIZE && _queue.TryPeek(out LogEntry? peekEntry))
            {
                if (peekEntry.NextRetryAt > now)
                {
                    break;
                }

                if (_queue.TryDequeue(out LogEntry? entry))
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

            bool success = await WriteBatchToDatabaseAsync(batch, ct);

            if (!success)
            {
                foreach (LogEntry item in batch)
                {
                    if (item.RetryCount + 1 < MAX_RETRIES)
                    {
                        var delay = TimeSpan.FromSeconds(Math.Pow(2, item.RetryCount + 1));
                        _queue.Enqueue(item with
                        {
                            RetryCount = item.RetryCount + 1,
                            NextRetryAt = DateTimeOffset.UtcNow.Add(delay)
                        });
                    }
                    else
                    {
                        if (_logger.IsEnabled(LogLevel.Error))
                        {
                            _logger.LogError("Лог отброшен после {Retries} попыток.", MAX_RETRIES);
                        }
                    }
                }
            }

            return success;
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    private async Task<bool> WriteBatchToDatabaseAsync(List<LogEntry> batch, CancellationToken ct)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();

        try
        {
            List<Guid> uniqueDevicesId = [.. batch.Where(l => l.UserDeviceId != Guid.Empty)
                .DistinctBy(l => l.UserDeviceId)
                .Select(a=>a.UserDeviceId)];
            for (int i = uniqueDevicesId.Count - 1; i >= 0; i--) {
                if (db.UserDevices.Any(a=>a.Id == uniqueDevicesId[i]))
                {
                    uniqueDevicesId.RemoveAt(i);
                }
            }

            List<UserDevice> uniqueDevices = [.. batch
                .Where(l => l.UserDeviceId != Guid.Empty && uniqueDevicesId.Any(a=>a == l.UserDeviceId))
                .DistinctBy(l => l.UserDeviceId)
                .Select(item => UserDeviceHelper.DtoToUserDevice(item.dto, item.UserDeviceId))];

            if (uniqueDevices.Count > 0)
            {
                List<Guid> deviceIds = [.. uniqueDevices.Select(d => d.Id)];

                HashSet<Guid> existingIds = await db.UserDevices
                    .Where(d => deviceIds.Contains(d.Id))
                    .Select(d => d.Id)
                    .ToHashSetAsync(ct);

                foreach (UserDevice device in uniqueDevices)
                {
                    if (!existingIds.Contains(device.Id))
                    {
                        _ = db.UserDevices.Add(device);
                    }
                }
            }

            foreach (LogEntry item in batch)
            {
                _ = db.AuthRegLogs.Add(new Server_DB_Postgres.Entities.Logs.AuthRegLog
                {
                    Email = item.dto.Email,
                    Success = item.Success,
                    UserId = item.UserId,
                    UserDeviceId = item.UserDeviceId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Ip = item.Ip,
                    ActionIsAuthentication = item.ActionIsAuthentication
                });
            }

            _ = await db.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Ошибка записи батча ({Count} записей).", batch.Count);
            }

            return false;
        }
    }

    private async Task PerformCleanupAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        using IServiceScope scope = _serviceProvider.CreateScope();
        DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();

        try
        {
            DateTimeOffset cutoff = DateTimeOffset.UtcNow.AddMonths(-24);
            int deleted = await db.AuthRegLogs
                .Where(a => a.CreatedAt < cutoff)
                .ExecuteDeleteAsync(ct);

            if (deleted > 0)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Очистка завершена: удалено {Count} записей.", deleted);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при очистке старых логов.");
        }
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken ct)
    {
        _logger.LogInformation("Запрос на остановку сервиса логирования...");
        await _internalCts.CancelAsync();

        // Дожидаемся graceful завершения основных циклов
        if (_processingTask != null)
        {
            await _processingTask.WaitAsync(ct).ContinueWith(_ => { }, TaskScheduler.Default);
        }

        if (_cleanupTask != null)
        {
            await _cleanupTask.WaitAsync(ct).ContinueWith(_ => { }, TaskScheduler.Default);
        }

        // "Умный" финальный flush: до 20 попыток, но не более 2 ошибок БД подряд
        int consecutiveFailures = 0;
        const int maxConsecutiveFailures = 2;

        while (!_queue.IsEmpty && !ct.IsCancellationRequested && consecutiveFailures < maxConsecutiveFailures)
        {
            bool success = await ProcessBatchAsync(ct);

            if (success)
            {
                consecutiveFailures = 0;
            }
            else
            {
                consecutiveFailures++;
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("Сбой записи при остановке (ошибка {Count}/{Max}).", consecutiveFailures, maxConsecutiveFailures);
                }
            }

            if (!_queue.IsEmpty && !ct.IsCancellationRequested)
            {
                await Task.Delay(success ? 100 : 1000, ct);
            }
        }

        if (!_queue.IsEmpty)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning("При остановке осталось {Count} необработанных логов.", _queue.Count);
            }
        }
        else
        {
            _logger.LogInformation("Все логи успешно записаны при остановке.");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _linkedCts?.Dispose();
        _internalCts.Dispose();
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
