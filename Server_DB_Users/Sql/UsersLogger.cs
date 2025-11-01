// Server_DB_Users/Sql/UsersLogger.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Server_DB;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Server_DB_Users.Sql;

/// <summary>
/// Статический класс для записи логов авторизации пользователей в базу данных.
/// Содержит методы для обработки данных об устройстве, хеширования информации и сохранения в таблицу <c>users_authorization_logs</c>.
/// </summary>
public static class UsersLogger
{
    /// <summary>
    /// Асинхронно записывает лог авторизации пользователя в базу данных.
    /// Включает в себя данные об устройстве, IP-адресе, результате авторизации и уникальный хеш для предотвращения дубликатов.
    /// </summary>
    /// <param name="obj">Объект JSON, содержащий информацию об устройстве пользователя.</param>
    /// <param name="user_id">Уникальный идентификатор пользователя (GUID).</param>
    /// <param name="email">Электронная почта пользователя, использованная при авторизации.</param>
    /// <param name="ip_address">IP-адрес клиента, с которого была попытка авторизации.</param>
    /// <param name="authorizationSuccess">Флаг успешности авторизации: <c>true</c> — успех, <c>false</c> — ошибка.</param>
    /// <remarks>
    /// Перед вставкой данные об устройстве объединяются в строку, из которой вычисляется SHA256-хеш.
    /// Хеш используется для предотвращения дублирования записей при повторных попытках с одинаковыми параметрами.
    /// Все операции обёрнуты в блок try-catch для предотвращения падения приложения при ошибках БД.
    /// </remarks>
    public static async Task WriteLog(JsonObject obj, Guid? user_id, string? email, NpgsqlInet ip_address, bool authorizationSuccess,ILogger logger)
    {
        try
        {
            // Извлечение данных об устройстве из JSON с ограничением по длине (макс. 255 символов)
            string? deviceModel = JsonObjectHelper.GetStringN(obj, "deviceModel", 255);
            string? deviceType = JsonObjectHelper.GetStringN(obj, "deviceType", 255);
            string? operatingSystem = JsonObjectHelper.GetStringN(obj, "operatingSystem", 255);
            string? processorType = JsonObjectHelper.GetStringN(obj, "processorType", 255);
            int? processorCount = JsonObjectHelper.GetIntegerN(obj, "processorCount");
            int? systemMemorySize = JsonObjectHelper.GetIntegerN(obj, "systemMemorySize");
            string? graphicsDeviceName = JsonObjectHelper.GetStringN(obj, "graphicsDeviceName", 255);
            int? graphicsMemorySize = JsonObjectHelper.GetIntegerN(obj, "graphicsMemorySize");
            string? deviceUniqueIdentifier = JsonObjectHelper.GetStringN(obj, "deviceUniqueIdentifier", 255);

            // Сборка строки с данными устройства для последующего хеширования
            string split = "___"; // Разделитель между полями — позволяет легко разделять данные обратно при необходимости
            StringBuilder stringBuilder = new();

            // Последовательное добавление полей в строку. Используется null-коалесценция для замены null на пустую строку или 0
            _ = stringBuilder.Append(deviceModel ?? string.Empty); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(deviceType ?? string.Empty); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(operatingSystem ?? string.Empty); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(processorType ?? string.Empty); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(processorCount ?? 0); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(systemMemorySize ?? 0); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(graphicsDeviceName ?? string.Empty); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(graphicsMemorySize ?? 0); _ = stringBuilder.Append(split);
            _ = stringBuilder.Append(deviceUniqueIdentifier ?? string.Empty);

            // Преобразование строки в байты и вычисление SHA256-хеша
            // Хеш используется как уникальный идентификатор комбинации параметров устройства, предотвращая дублирование записей
            byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            byte[] hashBytes = SHA256.HashData(bytes);

            // Создание контекста базы данных для выполнения SQL-запроса
            DbContext_Game03Users db = new();

            // Подготовка SQL-запроса на вставку данных в таблицу логов авторизации
            string sql = $"""
            INSERT INTO users_authorization_logs (
                id,
                user_id,
                success,
                email,
                ip_address,
                device_model,
                device_type,
                operating_system,
                processor_type,
                processor_count,
                system_memory_size,
                graphics_device_name,
                graphics_memory_size,
                device_unique_identifier,
                hash_sha256
            ) VALUES (
                @id,
                @user_id,
                @success,
                @email,
                @ip_address,
                @deviceModel,
                @deviceType,
                @operatingSystem,
                @processorType,
                @processorCount,
                @systemMemorySize,
                @graphicsDeviceName,
                @graphicsMemorySize,
                @deviceUniqueIdentifier,
                @hash_sha256
            );
            """;

            // Выполнение SQL-запроса с параметрами
            // Используются именованные параметры для защиты от SQL-инъекций
            _ = await db.Database.ExecuteSqlRawAsync(sql,
                new NpgsqlParameter("id", Guid.NewGuid()), // Новый уникальный ID для каждой записи
                new NpgsqlParameter("user_id", user_id),
                new NpgsqlParameter("success", authorizationSuccess),
                new NpgsqlParameter("email", email),
                new NpgsqlParameter("ip_address", ip_address),
                new NpgsqlParameter("deviceModel", deviceModel),
                new NpgsqlParameter("deviceType", deviceType),
                new NpgsqlParameter("operatingSystem", operatingSystem),
                new NpgsqlParameter("processorType", processorType),
                new NpgsqlParameter("processorCount", processorCount),
                new NpgsqlParameter("systemMemorySize", systemMemorySize),
                new NpgsqlParameter("graphicsDeviceName", graphicsDeviceName),
                new NpgsqlParameter("graphicsMemorySize", graphicsMemorySize),
                new NpgsqlParameter("deviceUniqueIdentifier", deviceUniqueIdentifier),
                new NpgsqlParameter("hash_sha256", hashBytes) // Хеш сохраняется как бинарное значение
            );
        }
        catch (Exception ex)
        {
            // Логирование исключения в файл, если произошла ошибка при работе с БД
            //await Server_Common.WriterExceptionInLogFile.LogToFileAsync(ex);
            logger.LogError(ex, "");
        }
    }
}
