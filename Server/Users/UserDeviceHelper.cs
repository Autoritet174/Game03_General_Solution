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

 
        private const char SPLIT = '|';

        /// <summary>
        /// Вычисляет идентификатор устройства, используя IncrementalHash и Span-форматирование примитивов.
        /// </summary>
        /// <param name="dto">Данные устройства.</param>
        /// <returns>Детерминированный GUID на основе хеша характеристик.</returns>
        public static Guid ComputeId(DtoRequestAuthReg dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.DeviceUniqueIdentifier))
            {
                return Guid.Empty;
            }

            using IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            Span<byte> separator = [(byte)SPLIT];

            // Используем статические локальные функции, чтобы исключить аллокации на замыканиях
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

            Span<byte> hash = stackalloc byte[32];
            if (!hasher.TryGetHashAndReset(hash, out _)) return Guid.Empty;

            // Формирование UUID v5
            Span<byte> guidBytes = stackalloc byte[16];
            hash[..16].CopyTo(guidBytes);
            guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
            guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

            return new Guid(guidBytes);

            // --- Статические локальные функции (Zero-allocation) ---

            static void AppendString(string? value, IncrementalHash h, Span<byte> sep)
            {
                if (string.IsNullOrEmpty(value)) return;

                // Используем ArrayPool или stackalloc для кодирования строки
                int maxBytes = Encoding.UTF8.GetMaxByteCount(value.Length);
                if (maxBytes <= 512)
                {
                    Span<byte> buffer = stackalloc byte[maxBytes];
                    int written = Encoding.UTF8.GetBytes(value.Trim().ToUpperInvariant(), buffer);
                    h.AppendData(buffer[..written]);
                }
                else
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(value.Trim().ToUpperInvariant());
                    h.AppendData(buffer);
                }
                h.AppendData(sep);
            }

            static void AppendInt(int? value, IncrementalHash h, Span<byte> sep)
            {
                if (value is null) return;

                // Прямая запись int в UTF-8 буфер на стеке
                Span<byte> buffer = stackalloc byte[11]; // Max length of int32
                if (Utf8Formatter.TryFormat(value.Value, buffer, out int written))
                {
                    h.AppendData(buffer[..written]);
                }
                h.AppendData(sep);
            }

            static void AppendBool(bool? value, IncrementalHash h, Span<byte> sep)
            {
                if (value is null) return;

                // Буфер для "[TRUE]" или "[FALSE]"
                ReadOnlySpan<byte> boolBytes = value.Value
                    ? "[TRUE]"u8
                    : "[FALSE]"u8;

                h.AppendData(boolBytes);
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
