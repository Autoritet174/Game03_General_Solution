using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Server_DB_Users;
using System.Collections.Concurrent;

namespace Server.Http_NS.Controllers_NS.Users;

public class BackgroundLoggerAuthentificationService : IHostedService, IDisposable
{
    private readonly ILogger<BackgroundLoggerAuthentificationService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;
    private readonly ConcurrentQueue<LogEntry> _queue = new();
    private readonly CancellationTokenSource _stoppingCts = new();

    public BackgroundLoggerAuthentificationService(
        ILogger<BackgroundLoggerAuthentificationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Вызывается при старте приложения.
    /// Запускает таймер, который каждые 5 секунд проверяет очередь.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background logger started.");

        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5)); // Каждые 5 секунд

        return Task.CompletedTask;
    }

    /// <summary>
    /// Вызывается при остановке приложения.
    /// Останавливает таймер и обрабатывает оставшиеся логи.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background logger is stopping.");

        _ = (_timer?.Change(Timeout.Infinite, 0));

        // Пытаемся обработать оставшиеся логи
        try
        {
            await Task.Run(ProcessBatch, _stoppingCts.Token)
                .WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Graceful shutdown timed out. Logs may be lost.");
        }

        _logger.LogInformation("Background logger stopped.");
    }

    /// <summary>
    /// Добавляет лог в очередь на фоновую запись.
    /// Вызывается из контроллера.
    /// </summary>
    public void EnqueueLog(
        Guid userId,
        string email,
        NpgsqlInet ipAddress,
        bool success,
        Guid deviceId)
    {
        _queue.Enqueue(new LogEntry(
            userId, email, ipAddress, success, deviceId));
    }

    private void DoWork(object? state)
    {
        ProcessBatch();
    }

    private void ProcessBatch()
    {
        if (_queue.IsEmpty)
        {
            return;
        }

        var batch = new List<LogEntry>();

        while (batch.Count < 100 && _queue.TryDequeue(out LogEntry? entry))
        {
            batch.Add(entry);
        }

        if (batch.Count == 0)
        {
            return;
        }

        _ = Task.Run(async () =>
        {
            try
            {
                await WriteBatchToDatabase(batch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write log batch to database");
            }
        });
    }

    private async Task WriteBatchToDatabase(List<LogEntry> batch)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        DbContext_Game03Users db = scope.ServiceProvider.GetRequiredService<DbContext_Game03Users>();

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await db.Database.BeginTransactionAsync();

        foreach (LogEntry log in batch)
        {
            _ = await db.Database.ExecuteSqlRawAsync($"""
                INSERT INTO users_authorization_logs 
                (id, user_id, success, email, ip_address, device_id, created_at)
                VALUES
                (@id, @user_id, @success, @email, @ip_address, @device_id, @created_at)
                """,
                new NpgsqlParameter("id", Guid.NewGuid()),
                new NpgsqlParameter("user_id", log.UserId),
                new NpgsqlParameter("success", log.Success),
                new NpgsqlParameter("email", log.Email),
                new NpgsqlParameter("ip_address", log.IpAddress),
                new NpgsqlParameter("device_id", log.DeviceId),
                new NpgsqlParameter("created_at", DateTime.UtcNow)
            );
        }

        await transaction.CommitAsync();
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _stoppingCts.Cancel();
    }

    private record LogEntry(
        Guid UserId,
        string Email,
        NpgsqlInet IpAddress,
        bool Success,
        Guid DeviceId);
}
