using System;

namespace General.DTO.RestResponse;

public class DtoResponseAuthReg(string? accessToken = null, string? refreshToken = null, string? errorKey = null, DateTimeOffset? extraDateTimeOffset = null, Guid? extraGuid = null, long? extraLong = null)
{
    public string? ErrorKey { get; } = errorKey;
    public string? AccessToken { get; } = accessToken;
    public string? RefreshToken { get; } = refreshToken;
    public DateTimeOffset? ExtraDateTimeOffset { get; } = extraDateTimeOffset;
    public Guid? ExtraGuid { get; } = extraGuid;
    public long? ExtraLong { get; } = extraLong;
}
