using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Text.Json.Nodes;
namespace Server.DB.Users.Sql;
public static class UsersLogger
{
    public static async Task WriteLog(JsonObject obj, Guid user_id, string email, string password_hash, NpgsqlInet ip_address)
    {

        string? deviceModel = JsonObjectHelper.GetStringN(obj, "deviceModel", 255);
        string? deviceType = JsonObjectHelper.GetStringN(obj, "deviceType", 255);
        string? operatingSystem = JsonObjectHelper.GetStringN(obj, "operatingSystem", 255);
        string? processorType = JsonObjectHelper.GetStringN(obj, "processorType", 255);
        int? processorCount = JsonObjectHelper.GetIntegerN(obj, "processorCount");
        int? systemMemorySize = JsonObjectHelper.GetIntegerN(obj, "systemMemorySize");
        string? graphicsDeviceName = JsonObjectHelper.GetStringN(obj, "graphicsDeviceName", 255);
        int? graphicsMemorySize = JsonObjectHelper.GetIntegerN(obj, "graphicsMemorySize");

        Db db = new();
        string sql = $"""
            INSERT INTO users_authorization_logs (
                id,
                user_id,
                success,
                created_at,
                email,
                password_hash,
                ip_address,
                device_model,
                device_type,
                operating_system,
                processor_type,
                processor_count,
                system_memory_size,
                graphics_device_name,
                graphics_memory_size
            ) VALUES (
                @id,
                @user_id,
                @success,
                NOW(),
                @email,
                @password_hash,
                @ip_address,
                @deviceModel,
                @deviceType,
                @operatingSystem,
                @processorType,
                @processorCount,
                @systemMemorySize,
                @graphicsDeviceName,
                @graphicsMemorySize
            );
            """;

        _ = await db.Database.ExecuteSqlRawAsync(sql,
            new NpgsqlParameter("id", DatabaseHelpers.CreateGuidPostgreSql()),
            new NpgsqlParameter("user_id", user_id),
            new NpgsqlParameter("success", true),
            new NpgsqlParameter("email", email),
            new NpgsqlParameter("password_hash", password_hash),
            new NpgsqlParameter("ip_address", ip_address),
            new NpgsqlParameter("deviceModel", deviceModel),
            new NpgsqlParameter("deviceType", deviceType),
            new NpgsqlParameter("operatingSystem", operatingSystem),
            new NpgsqlParameter("processorType", processorType),
            new NpgsqlParameter("processorCount", processorCount),
            new NpgsqlParameter("systemMemorySize", systemMemorySize),
            new NpgsqlParameter("graphicsDeviceName", graphicsDeviceName),
            new NpgsqlParameter("graphicsMemorySize", graphicsMemorySize)
        );

    }
}
