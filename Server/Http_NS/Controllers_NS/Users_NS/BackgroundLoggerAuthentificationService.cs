using Microsoft.EntityFrameworkCore;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Collections.Concurrent;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS.Users_NS;

/// <summary>
/// Фоновый сервис пакетной записи логов авторизации и данных устройств
/// с контролируемыми повторными попытками и корректной остановкой.
/// </summary>
public sealed class BackgroundLoggerAuthentificationService(
    ILogger<BackgroundLoggerAuthentificationService> logger,
    IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    /// <summary>
    /// Лог авторизации с поддержкой повторной обработки и кэшированными данными устройства.
    /// </summary>
    private sealed record LogEntry(
        bool AuthorizationSuccess,
        JsonObject Obj,
        string? Email,
        Guid? UserId,
        IPAddress? Ip,
        int RetryCount,
        DateTimeOffset NextRetryAt,
        DeviceParsedData? CachedDeviceData);

    /// <summary>
    /// Распарсенные данные устройства.
    /// </summary>
    private sealed record DeviceParsedData(
        Guid Id,
        string? DeviceModel,
        string? DeviceType,
        string? OperatingSystem,
        string? ProcessorType,
        int? ProcessorCount,
        int? SystemMemorySize,
        string? GraphicsDeviceName,
        string? DeviceUniqueIdentifier,
        int? GraphicsMemorySize,
        string? SystemEnvironmentUserName,
        bool? SystemInfoSupportsInstancing,
        string? SystemInfoNpotSupport,
        int? TimeZoneMinutes);

    private readonly ILogger<BackgroundLoggerAuthentificationService> _logger = logger;
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
    private static readonly char SPLIT = '|';

    /// <summary>
    /// Добавляет лог в очередь с предварительным вычислением данных устройства.
    /// </summary>
    /// <param name="authorizationSuccess">Успешность авторизации.</param>
    /// <param name="obj">JSON объект с данными устройства.</param>
    /// <param name="email">Email пользователя.</param>
    /// <param name="userId">ID пользователя.</param>
    /// <param name="ip">IP-адрес запроса.</param>
    /// <exception cref="ArgumentNullException">Бросается, если obj равен null.</exception>
    public void EnqueueLog(
        bool authorizationSuccess,
        JsonObject obj,
        string? email,
        Guid? userId,
        IPAddress? ip)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (_queue.Count >= MAX_QUEUE_SIZE)
        {
            _logger.LogWarning("Очередь логов переполнена. Запись отброшена.");
            return;
        }

        DeviceParsedData? deviceData = ParseAndComputeId(obj);

        _queue.Enqueue(new LogEntry(
            authorizationSuccess,
            obj,
            email,
            userId,
            ip,
            RetryCount: 0,
            NextRetryAt: DateTimeOffset.UtcNow,
            CachedDeviceData: deviceData));
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
        DbContext_Game db = scope.ServiceProvider.GetRequiredService<DbContext_Game>();

        try
        {
            HashSet<Guid> addedInBatch = [];

            var uniqueDevices = batch
                .Where(l => l.CachedDeviceData != null)
                .Select(l => l.CachedDeviceData!)
                .Where(d => addedInBatch.Add(d.Id))
                .ToList();

            if (uniqueDevices.Count > 0)
            {
                var deviceIds = uniqueDevices.Select(d => d.Id).ToList();

                HashSet<Guid> existingIds = await db.UserDevices
                    .Where(d => deviceIds.Contains(d.Id))
                    .Select(d => d.Id)
                    .ToHashSetAsync(ct);

                foreach (DeviceParsedData device in uniqueDevices)
                {
                    if (!existingIds.Contains(device.Id))
                    {
                        _ = db.UserDevices.Add(MapToEntity(device));
                    }
                }
            }

            foreach (LogEntry item in batch)
            {
                _ = db.UserAuthorizations.Add(new Server_DB_Postgres.Entities.Logs.UserAuthorization
                {
                    Email = item.Email,
                    Success = item.AuthorizationSuccess,
                    UserId = item.UserId,
                    UserDeviceId = item.CachedDeviceData?.Id,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Ip = item.Ip
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
        DbContext_Game db = scope.ServiceProvider.GetRequiredService<DbContext_Game>();

        try
        {
            DateTimeOffset cutoff = DateTimeOffset.UtcNow.AddMonths(-24);
            int deleted = await db.UserAuthorizations
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

    private static DeviceParsedData? ParseAndComputeId(JsonObject obj)
    {
        string? uid = obj.GetStringN("deviceUniqueIdentifier");
        if (string.IsNullOrWhiteSpace(uid))
        {
            return null;
        }

        var data = new DeviceParsedData(
            Guid.Empty,
            obj.GetStringN("deviceModel", 255),
            obj.GetStringN("deviceType", 255),
            obj.GetStringN("operatingSystem", 255),
            obj.GetStringN("processorType", 255),
            obj.GetIntegerN("processorCount"),
            obj.GetIntegerN("systemMemorySize"),
            obj.GetStringN("graphicsDeviceName", 255),
            uid,
            obj.GetIntegerN("graphicsMemorySize"),
            obj.GetStringN("system_Environment_UserName", 255),
            obj.GetBoolN("systemInfo_supportsInstancing"),
            obj.GetStringN("systemInfo_npotSupport", 255),
            obj.GetIntegerN("timeZoneInfo_Local_BaseUtcOffset_Minutes"));

        StringBuilder sb = new();
        _ = sb.Append(data.SystemEnvironmentUserName ?? "").Append(SPLIT)
          .Append(data.TimeZoneMinutes ?? 0).Append(SPLIT)
          .Append(data.DeviceUniqueIdentifier).Append(SPLIT)
          .Append(data.DeviceModel ?? "").Append(SPLIT)
          .Append(data.DeviceType ?? "").Append(SPLIT)
          .Append(data.OperatingSystem ?? "").Append(SPLIT)
          .Append(data.ProcessorType ?? "").Append(SPLIT)
          .Append(data.ProcessorCount ?? 0).Append(SPLIT)
          .Append(data.SystemMemorySize ?? 0).Append(SPLIT)
          .Append(data.GraphicsDeviceName ?? "").Append(SPLIT)
          .Append(data.GraphicsMemorySize ?? 0).Append(SPLIT)
          .Append(data.SystemInfoSupportsInstancing == true ? "[true]" :
                  data.SystemInfoSupportsInstancing == false ? "[false]" : "[null]")
          .Append(SPLIT)
          .Append(data.SystemInfoNpotSupport ?? "");

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString().ToLowerInvariant()));
        byte[] guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);

        // UUID v5 (Fingerprint): Version 5 (0x50), Variant RFC 4122 (0x80)
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

        return data with { Id = new Guid(guidBytes) };
    }

    private static UserDevice MapToEntity(DeviceParsedData d)
    {
        return new()
        {
            Id = d.Id,
            DeviceModel = d.DeviceModel,
            DeviceType = d.DeviceType,
            OperatingSystem = d.OperatingSystem,
            ProcessorType = d.ProcessorType,
            ProcessorCount = d.ProcessorCount,
            SystemMemorySize = d.SystemMemorySize,
            GraphicsDeviceName = d.GraphicsDeviceName,
            DeviceUniqueIdentifier = d.DeviceUniqueIdentifier,
            GraphicsMemorySize = d.GraphicsMemorySize,
            SystemEnvironmentUserName = d.SystemEnvironmentUserName,
            SystemInfoSupportsInstancing = d.SystemInfoSupportsInstancing,
            SystemInfoNpotSupport = d.SystemInfoNpotSupport,
            TimeZoneMinutes = d.TimeZoneMinutes
        };
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
