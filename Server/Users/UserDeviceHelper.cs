using General.DTO.RestRequest;
using Server_DB_Postgres.Entities.Users;
using System.Security.Cryptography;
using System.Text;

namespace Server.Users;

public static class UserDeviceHelper
{

    private static readonly char SPLIT = '|';
    public static Guid ComputeId(DtoRequestAuthReg dto)
    {
        if (string.IsNullOrWhiteSpace(dto.DeviceUniqueIdentifier))
        {
            return Guid.Empty;
        }

        StringBuilder sb = new();
        _ = sb
            .Append(dto.System_Environment_UserName.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.TimeZoneInfo_Local_BaseUtcOffset_Minutes)
            .Append(SPLIT)
            .Append(dto.DeviceUniqueIdentifier)
            .Append(SPLIT)
            .Append(dto.DeviceModel.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.DeviceType.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.OperatingSystem.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.ProcessorType.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.ProcessorCount)
            .Append(SPLIT)
            .Append(dto.SystemMemorySize)
            .Append(SPLIT)
            .Append(dto.GraphicsDeviceName.Trim().ToUpperInvariant() ?? "")
            .Append(SPLIT)
            .Append(dto.GraphicsMemorySize)
            .Append(SPLIT)
            .Append(dto.SystemInfo_supportsInstancing == true ? "[TRUE]" : dto.SystemInfo_supportsInstancing == false ? "[FALSE]" : "[NULL]")
            .Append(SPLIT)
            .Append(dto.SystemInfo_npotSupport.Trim().ToUpperInvariant() ?? "");

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString().Trim().ToUpperInvariant()));
        byte[] guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);

        // UUID v5 (Fingerprint): Version 5 (0x50), Variant RFC 4122 (0x80)
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

        return new Guid(guidBytes);
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
