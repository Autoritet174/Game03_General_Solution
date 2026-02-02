using General.DTO.RestRequest;
using Server_DB_Postgres.Entities.Users;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace Server.Users;

public static class UserDeviceHelper
{

    //private static readonly char SPLIT = '|';
    //public static Guid ComputeId(DtoRequestAuthReg dto)
    //{
    //    if (string.IsNullOrWhiteSpace(dto.DeviceUniqueIdentifier))
    //    {
    //        return Guid.Empty;
    //    }

    //    StringBuilder sb = new();
    //    _ = sb
    //        .Append(dto.System_Environment_UserName ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.TimeZoneInfo_Local_BaseUtcOffset_Minutes)
    //        .Append(SPLIT)
    //        .Append(dto.DeviceUniqueIdentifier)
    //        .Append(SPLIT)
    //        .Append(dto.DeviceModel ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.DeviceType ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.OperatingSystem ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.ProcessorType ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.ProcessorCount)
    //        .Append(SPLIT)
    //        .Append(dto.SystemMemorySize)
    //        .Append(SPLIT)
    //        .Append(dto.GraphicsDeviceName ?? "".Trim().ToUpperInvariant() ?? "")
    //        .Append(SPLIT)
    //        .Append(dto.GraphicsMemorySize)
    //        .Append(SPLIT)
    //        .Append(dto.SystemInfo_supportsInstancing == true ? "[TRUE]" : dto.SystemInfo_supportsInstancing == false ? "[FALSE]" : "[NULL]")
    //        .Append(SPLIT)
    //        .Append(dto.SystemInfo_npotSupport ?? "".Trim().ToUpperInvariant() ?? "");

    //    byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString().Trim().ToUpperInvariant()));
    //    byte[] guidBytes = new byte[16];
    //    Array.Copy(hash, guidBytes, 16);

    //    // UUID v5 (Fingerprint): Version 5 (0x50), Variant RFC 4122 (0x80)
    //    guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
    //    guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

    //    return new Guid(guidBytes);
    //}


    //private const char SPLIT = '|';

    ///// <summary>
    ///// Вычисляет идентификатор устройства, используя IncrementalHash и Span-форматирование примитивов.
    ///// </summary>
    ///// <param name="dto">Данные устройства.</param>
    ///// <returns>Детерминированный GUID на основе хеша характеристик.</returns>
    //public static Guid ComputeId(DtoRequestAuthReg dto)
    //{
    //    if (dto == null || string.IsNullOrWhiteSpace(dto.DeviceUniqueIdentifier))
    //    {
    //        return Guid.Empty;
    //    }

    //    using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
    //    Span<byte> separator = [(byte)SPLIT];

    //    // Используем статические локальные функции, чтобы исключить аллокации на замыканиях
    //    AppendString(dto.System_Environment_UserName, hasher, separator);
    //    AppendInt(dto.TimeZoneInfo_Local_BaseUtcOffset_Minutes, hasher, separator);
    //    AppendString(dto.DeviceUniqueIdentifier, hasher, separator);
    //    AppendString(dto.DeviceModel, hasher, separator);
    //    AppendString(dto.DeviceType, hasher, separator);
    //    AppendString(dto.OperatingSystem, hasher, separator);
    //    AppendString(dto.ProcessorType, hasher, separator);
    //    AppendInt(dto.ProcessorCount, hasher, separator);
    //    AppendInt(dto.SystemMemorySize, hasher, separator);
    //    AppendString(dto.GraphicsDeviceName, hasher, separator);
    //    AppendInt(dto.GraphicsMemorySize, hasher, separator);
    //    AppendBool(dto.SystemInfo_supportsInstancing, hasher, separator);
    //    AppendString(dto.SystemInfo_npotSupport, hasher, separator);

    //    Span<byte> hash = stackalloc byte[32];
    //    if (!hasher.TryGetHashAndReset(hash, out _))
    //    {
    //        return Guid.Empty;
    //    }

    //    // Формирование UUID v5
    //    Span<byte> guidBytes = stackalloc byte[16];
    //    hash[..16].CopyTo(guidBytes);
    //    guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
    //    guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

    //    return new Guid(guidBytes);

    //    // --- Статические локальные функции (Zero-allocation) ---

    //    static void AppendString(string? value, IncrementalHash h, Span<byte> sep)
    //    {
    //        if (string.IsNullOrEmpty(value))
    //        {
    //            return;
    //        }

    //        // Используем ArrayPool или stackalloc для кодирования строки
    //        int maxBytes = Encoding.UTF8.GetMaxByteCount(value.Length);
    //        if (maxBytes <= 512)
    //        {
    //            Span<byte> buffer = stackalloc byte[maxBytes];
    //            int written = Encoding.UTF8.GetBytes(value.Trim().ToUpperInvariant(), buffer);
    //            h.AppendData(buffer[..written]);
    //        }
    //        else
    //        {
    //            byte[] buffer = Encoding.UTF8.GetBytes(value.Trim().ToUpperInvariant());
    //            h.AppendData(buffer);
    //        }
    //        h.AppendData(sep);
    //    }

    //    static void AppendInt(int? value, IncrementalHash h, Span<byte> sep)
    //    {
    //        if (value is null)
    //        {
    //            return;
    //        }

    //        // Прямая запись int в UTF-8 буфер на стеке
    //        Span<byte> buffer = stackalloc byte[11]; // Max length of int32
    //        if (Utf8Formatter.TryFormat(value.Value, buffer, out int written))
    //        {
    //            h.AppendData(buffer[..written]);
    //        }
    //        h.AppendData(sep);
    //    }

    //    static void AppendBool(bool? value, IncrementalHash h, Span<byte> sep)
    //    {
    //        if (value is null)
    //        {
    //            return;
    //        }

    //        // Буфер для "[TRUE]" или "[FALSE]"
    //        ReadOnlySpan<byte> boolBytes = value.Value
    //                ? "[TRUE]"u8
    //                : "[FALSE]"u8;

    //        h.AppendData(boolBytes);
    //        h.AppendData(sep);
    //    }
    //}


    private const byte SEPARATOR_BYTE = 0x7C; // Символ '|'

    /// <summary>
    /// Генерирует детерминированный UUID v8 на основе SHA-256.
    /// </summary>
    public static Guid ComputeUUIDv8(DtoRequestAuthReg dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.DeviceUniqueIdentifier))
        {
            return Guid.Empty;
        }

        // Используем SHA-256 для UUID v8
        using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        ReadOnlySpan<byte> separator = [SEPARATOR_BYTE];

        // Последовательно добавляем данные в хешер
        AppendString(dto.System_Environment_UserName, hasher, separator);
        AppendInt(dto.TimeZoneInfo_Local_BaseUtcOffset_Minutes, hasher, separator);
        AppendString(dto.DeviceUniqueIdentifier, hasher, separator);
        AppendString(dto.DeviceModel, hasher, separator);
        AppendString(dto.DeviceType, hasher, separator);
        AppendString(dto.OperatingSystem, hasher, separator);
        AppendString(dto.ProcessorType, hasher, separator);
        AppendInt(dto.ProcessorCount, hasher, separator);
        AppendInt(dto.SystemMemorySize, hasher, separator);
        AppendString(dto.GraphicsDeviceName, hasher, separator);
        AppendInt(dto.GraphicsMemorySize, hasher, separator);
        AppendBool(dto.SystemInfo_supportsInstancing, hasher, separator);
        AppendString(dto.SystemInfo_npotSupport, hasher, separator);

        // Получаем хеш (32 байта для SHA-256)
        Span<byte> hash = stackalloc byte[32];
        if (!hasher.TryGetHashAndReset(hash, out int written))
        {
            return Guid.Empty;
        }

        // Берем первые 16 байт для UUID
        Span<byte> guidBytes = hash[..16];

        // Установка метаданных согласно RFC 9562 (UUID v8)
        // 1. Версия 8: старшая тетрада байта 6 должна быть 1000 (0x80)
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x80);

        // 2. Вариант RFC: старшие биты байта 8 должны быть 10 (0x80)
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

        // ВАЖНО: В .NET конструктор Guid(ReadOnlySpan<byte>) ожидает Little-Endian.
        // Чтобы строковое представление совпадало с хешем, используем флаг bigEndian (доступен в .NET 8+)
        return new Guid(guidBytes, bigEndian: true);
    }

    private static void AppendString(string? value, IncrementalHash h, ReadOnlySpan<byte> sep)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        // Нормализация к верхнему регистру важна для консистентности хеша
        string normalized = value.Trim().ToUpperInvariant();
        int maxBytes = Encoding.UTF8.GetMaxByteCount(normalized.Length);

        if (maxBytes <= 512)
        {
            Span<byte> buffer = stackalloc byte[maxBytes];
            int written = Encoding.UTF8.GetBytes(normalized, buffer);
            h.AppendData(buffer[..written]);
        }
        else
        {
            h.AppendData(Encoding.UTF8.GetBytes(normalized));
        }
        h.AppendData(sep);
    }

    private static void AppendInt(int? value, IncrementalHash h, ReadOnlySpan<byte> sep)
    {
        if (value.HasValue)
        {
            Span<byte> buffer = stackalloc byte[12];
            if (Utf8Formatter.TryFormat(value.Value, buffer, out int written))
            {
                h.AppendData(buffer[..written]);
            }
            h.AppendData(sep);
        }
    }

    private static void AppendBool(bool? value, IncrementalHash h, ReadOnlySpan<byte> sep)
    {
        if (value.HasValue)
        {
            h.AppendData(value.Value ? "true"u8 : "false"u8);
            h.AppendData(sep);
        }
    }


    public static UserDevice DtoToUserDevice(DtoRequestAuthReg dto, Guid userDeviceId) => new()
    {
        Id = userDeviceId,
        DeviceModel = dto.DeviceModel,
        DeviceType = dto.DeviceType,
        OperatingSystem = dto.OperatingSystem,
        ProcessorType = dto.ProcessorType,
        ProcessorCount = dto.ProcessorCount,
        SystemMemorySize = dto.SystemMemorySize,
        GraphicsDeviceName = dto.GraphicsDeviceName,
        DeviceUniqueIdentifier = dto.DeviceUniqueIdentifier,
        GraphicsMemorySize = dto.GraphicsMemorySize,
        SystemEnvironmentUserName = dto.System_Environment_UserName,
        SystemInfoSupportsInstancing = dto.SystemInfo_supportsInstancing,
        SystemInfoNpotSupport = dto.SystemInfo_npotSupport,
        TimeZoneMinutes = dto.TimeZoneInfo_Local_BaseUtcOffset_Minutes
    };

}
