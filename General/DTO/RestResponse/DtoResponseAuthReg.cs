namespace General.DTO.RestResponse;

public class DtoResponseAuthReg(string? accessToken = null, string? refreshToken = null, string? errorKey = null, DateTimeOffset? extraDateTimeOffset = null, Guid? extraGuid = null, long? extraLong = null)
{
    public string? errorKey { get; } = errorKey;
    public string? accessToken { get; } = accessToken;
    public string? refreshToken { get; } = refreshToken;
    public DateTimeOffset? extraDateTimeOffset { get; } = extraDateTimeOffset;
    public Guid? extraGuid { get; } = extraGuid;
    public long? extraLong { get; } = extraLong;
}
