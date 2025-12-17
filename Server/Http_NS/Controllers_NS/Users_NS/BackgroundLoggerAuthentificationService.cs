using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Server.Utilities;
using Server_DB_Postgres;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS.Users_NS;

/// <summary>
/// Фоновый сервис для асинхронной записи логов аутентификации и информации об устройствах пользователей.
/// Обрабатывает очередь логов с периодичностью и сохраняет данные в базу PostgreSQL.
/// </summary>
public class BackgroundLoggerAuthentificationService(
    ILogger<BackgroundLoggerAuthentificationService> logger,
    IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private record LogEntry(bool authorizationSuccess, JsonObject obj, string? email, Guid? userId, NpgsqlInet? ip);

    /// <summary>
    /// Логгер для записи диагностических сообщений.
    /// </summary>
    private readonly ILogger<BackgroundLoggerAuthentificationService> _logger = logger;

    /// <summary>
    /// Провайдер сервисов для создания scope-контекстов при работе с EF Core.
    /// </summary>
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Таймер для периодической обработки очереди логов.
    /// </summary>
    private Timer? _timer;

    /// <summary>
    /// Потокобезопасная очередь логов, ожидающих сохранения в БД.
    /// </summary>
    private readonly ConcurrentQueue<LogEntry> _queue = new();

    /// <summary>
    /// Токен для отмены фоновых операций при остановке сервиса.
    /// </summary>
    private readonly CancellationTokenSource _stoppingCts = new();

    /// <summary>
    /// Разделитель полей при формировании строки для хеширования характеристик устройства.
    /// Используется для создания уникального идентификатора устройства.
    /// </summary>
    private const char SPLIT = '|';

    /// <summary>
    /// Добавляет запись о попытке аутентификации в очередь на фоновую обработку.
    /// Вызывается из контроллеров при входе/регистрации.
    /// </summary>
    /// <param name="authorizationSuccess">Успешна ли была попытка аутентификации.</param>
    /// <param name="obj">JSON-объект с информацией об устройстве (например, ОС, модель, GPU и т.д.).</param>
    /// <param name="email">Email пользователя, если известен.</param>
    /// <param name="userId">Идентификатор пользователя в системе, если авторизован.</param>
    /// <param name="ip">IP-адрес клиента.</param>
    public void EnqueueLog(bool authorizationSuccess, JsonObject obj, string? email, Guid? userId, NpgsqlInet? ip)
    {
        _queue.Enqueue(new LogEntry(authorizationSuccess, obj, email, userId, ip));
    }

    /// <summary>
    /// Вызывается таймером каждые 5 секунд — запускает обработку накопленной партии логов.
    /// </summary>
    /// <param name="state">Не используется.</param>
    private void DoWork(object? state)
    {
        ProcessBatch();
    }

    /// <summary>
    /// Извлекает пачку логов из очереди (до 100 записей) и асинхронно сохраняет в БД.
    /// Если очередь пуста — немедленно завершается.
    /// </summary>
    private void ProcessBatch()
    {
        if (_queue.IsEmpty)
        {
            return;
        }

        var batch = new List<LogEntry>();

        // Сбор до 100 записей из очереди
        while (batch.Count < 100 && _queue.TryDequeue(out LogEntry? entry))
        {
            batch.Add(entry);
        }

        if (batch.Count == 0)
        {
            return;
        }

        // Обработка в фоне, чтобы не блокировать таймер
        _ = Task.Run(async () =>
        {
            try
            {
                await WriteBatchToDatabase(batch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось записать пакет логов в базу данных");
            }
        });
    }

    /// <summary>
    /// Сохраняет пачку логов в базу данных с использованием транзакции.
    /// Для каждого лога:
    /// - Извлекает параметры устройства из JSON.
    /// - Формирует уникальный идентификатор устройства через SHA256-хеш.
    /// - Вставляет запись в таблицу устройств (если ещё не существует).
    /// - Добавляет запись в лог аутентификаций, ссылаясь на устройство.
    /// </summary>
    /// <param name="batch">Пачка логов для сохранения.</param>
    /// <exception cref="Exception">Любое исключение приведёт к записи в лог, но не остановит сервис.</exception>
    private async Task WriteBatchToDatabase(List<LogEntry> batch)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        DbContext_Game db = scope.ServiceProvider.GetRequiredService<DbContext_Game>();

        // Начинаем транзакцию — либо всё, либо ничего
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await db.Database.BeginTransactionAsync();

        foreach (LogEntry log in batch)
        {
            JsonObject obj = log.obj;

            // Извлечение данных об устройстве из JSON с ограничением по длине (макс. 255 символов)
            string? system_Environment_UserName = JsonObjectExtension.GetStringN(obj, "system_Environment_UserName", maxLength: 255);
            int? timeZoneInfo_Local_BaseUtcOffset_Minutes = JsonObjectExtension.GetIntegerN(obj, "timeZoneInfo_Local_BaseUtcOffset_Minutes");
            string? deviceUniqueIdentifier = JsonObjectExtension.GetStringN(obj, "deviceUniqueIdentifier", maxLength: 255);
            string? deviceModel = JsonObjectExtension.GetStringN(obj, "deviceModel", maxLength: 255);
            string? deviceType = JsonObjectExtension.GetStringN(obj, "deviceType", maxLength: 255);
            string? operatingSystem = JsonObjectExtension.GetStringN(obj, "operatingSystem", maxLength: 255);
            string? processorType = JsonObjectExtension.GetStringN(obj, "processorType", maxLength: 255);
            int? processorCount = JsonObjectExtension.GetIntegerN(obj, "processorCount");
            int? systemMemorySize = JsonObjectExtension.GetIntegerN(obj, "systemMemorySize");
            string? graphicsDeviceName = JsonObjectExtension.GetStringN(obj, "graphicsDeviceName", maxLength: 255);
            int? graphicsMemorySize = JsonObjectExtension.GetIntegerN(obj, "graphicsMemorySize");
            bool? systemInfo_supportsInstancing = JsonObjectExtension.GetBoolN(obj, "systemInfo_supportsInstancing");
            string? systemInfo_npotSupport = JsonObjectExtension.GetStringN(obj, "systemInfo_npotSupport", maxLength: 255);


            // Сборка строки с данными устройства для последующего хеширования
            StringBuilder stringBuilder = new();
            // Последовательное добавление полей в строку. Используется null-коалесценция для замены null на пустую строку или 0
            _ = stringBuilder.Append(system_Environment_UserName ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(timeZoneInfo_Local_BaseUtcOffset_Minutes ?? 0); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(deviceUniqueIdentifier ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(deviceModel ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(deviceType ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(operatingSystem ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(processorType ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(processorCount ?? 0); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(systemMemorySize ?? 0); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(graphicsDeviceName ?? string.Empty); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(graphicsMemorySize ?? 0); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(systemInfo_supportsInstancing == true ? "[true]" : (systemInfo_supportsInstancing == false ? "[false]" : "[null]")); _ = stringBuilder.Append(SPLIT);
            _ = stringBuilder.Append(systemInfo_npotSupport ?? string.Empty);

            // Преобразование строки в байты и вычисление SHA256-хеша
            // Хеш используется как уникальный идентификатор комбинации параметров устройства, предотвращая дублирование записей
            byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString().ToLowerInvariant());
            byte[] hash_sha256 = System.Security.Cryptography.SHA256.HashData(bytes);

            // Берём первые 16 байт хеша
            byte[] guidBytes = [.. hash_sha256.Take(16)];

            var user_device_id = new Guid(guidBytes);

            // Единый запрос: сначала вставка в devices (с игнорированием дублей), затем лог аутентификации
            await db.Database.ExecuteSqlRawAsync($"""
                INSERT INTO _main.users_devices 
                (id, system_environment_username, timezone_minutes, device_unique_identifier, device_model, device_type, operating_system, processor_type, processor_count, system_memory_size, graphics_device_name, graphics_memory_size, system_info_supports_instancing, system_info_npot_support)
                VALUES
                (@param__user_device_id, @param__system_environment_username, @param__timezone_minutes, @param__device_unique_identifier, @param__device_model, @param__device_type, @param__operating_system, @param__processor_type, @param__processor_count, @param__system_memory_size, @param__graphics_device_name, @param__graphics_memory_size, @param__system_info_supports_instancing, @param__system_info_npot_support)
                ON CONFLICT (id) DO NOTHING;

                INSERT INTO _main.users_authorization_logs 
                (success, email, user_id, ip_address, user_device_id)
                VALUES
                (@param__success, @param__email, @param__user_id, @param__ip_address, @param__user_device_id);
                """,

                new NpgsqlParameter("param__user_device_id", user_device_id),
                new NpgsqlParameter("param__system_environment_username", system_Environment_UserName),
                new NpgsqlParameter("param__timezone_minutes", timeZoneInfo_Local_BaseUtcOffset_Minutes),
                new NpgsqlParameter("param__device_unique_identifier", deviceUniqueIdentifier),
                new NpgsqlParameter("param__device_model", deviceModel),
                new NpgsqlParameter("param__device_type", deviceType),
                new NpgsqlParameter("param__operating_system", operatingSystem),
                new NpgsqlParameter("param__processor_type", processorType),
                new NpgsqlParameter("param__processor_count", processorCount),
                new NpgsqlParameter("param__system_memory_size", systemMemorySize),
                new NpgsqlParameter("param__graphics_device_name", graphicsDeviceName),
                new NpgsqlParameter("param__graphics_memory_size", graphicsMemorySize),
                new NpgsqlParameter("param__system_info_supports_instancing", systemInfo_supportsInstancing),
                new NpgsqlParameter("param__system_info_npot_support", systemInfo_npotSupport),
                new NpgsqlParameter("param__success", log.authorizationSuccess),
                new NpgsqlParameter("param__email", log.email),
                new NpgsqlParameter("param__user_id", log.userId),
                new NpgsqlParameter("param__ip_address", log.ip)
            );
        }

        // Фиксация транзакции после успешной обработки всех записей
        await transaction.CommitAsync();
    }

    /// <summary>
    /// Освобождает управляемые ресурсы (таймер, токен отмены).
    /// </summary>
    public void Dispose()
    {
        _timer?.Dispose();
        _stoppingCts.Cancel();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Запускает фоновый сервис при старте приложения.
    /// Настраивает таймер на немедленный старт и последующий запуск каждые 5 секунд.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены запуска.</param>
    /// <returns>Задача, завершаемая немедленно.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Фоновый логгер запущен.");
        _timer = new Timer(
            callback: DoWork,
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Останавливает фоновый сервис при завершении приложения.
    /// Пытается обработать оставшиеся логи в течение 5 секунд.
    /// При превышении времени — принудительно завершает с предупреждением.
    /// </summary>
    /// <param name="cancellationToken">Токен, сигнализирующий внешнее завершение приложения.</param>
    /// <returns>Задача, завершающаяся после попытки обработки остатка.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Фоновый логгер останавливается.");

        // Останавливаем таймер
        _ = (_timer?.Change(Timeout.Infinite, 0));

        // Устанавливаем лимит на обработку остатка
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(5));

        try
        {
            await Task.Run(ProcessBatch, timeoutCts.Token);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            _logger.LogWarning("Мягкая остановка превысила лимит времени. Некоторые логи могут быть утеряны.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при остановке фонового логгера");
        }

        _stoppingCts.Cancel();
        _logger.LogInformation("Фоновый логгер остановлен.");
    }
}
