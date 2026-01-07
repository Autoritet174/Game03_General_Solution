using General.DTO.RestResponse;
using L = General.LocalizationKeys;

namespace Server.Users;

public class AuthRegResponse
{
    public static DtoResponseAuthReg InvalidResponse() => new(errorKey: L.Error.Server.InvalidResponse);
    public static DtoResponseAuthReg InvalidCredentials() => new(errorKey: L.Error.Server.InvalidCredentials);
    public static DtoResponseAuthReg TooManyRequests(long seconds) => new(errorKey: L.Error.Server.TooManyRequests, extraLong: seconds);
    public static DtoResponseAuthReg RequiresTwoFactor() => new(errorKey: L.Error.Server.Required2FA);
    public static DtoResponseAuthReg RefreshTokenErrorCreating() => new(errorKey: L.Error.Server.RefreshTokenErrorCreating);
    public static DtoResponseAuthReg UserAlreadyExists() => new(errorKey: L.Error.Server.UserAlreadyExists);
    public static DtoResponseAuthReg Banned(DateTimeOffset? until) => new(errorKey: until == null ? L.Error.Server.AccountBannedPermanently: L.Error.Server.AccountBannedUntil, extraDateTimeOffset: until);
    public static DtoResponseAuthReg Success(string accessToken, string refreshToken) => new(accessToken: accessToken, refreshToken: refreshToken);
}
