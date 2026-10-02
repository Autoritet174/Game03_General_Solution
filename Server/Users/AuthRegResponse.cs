using General.DTO.RestResponse;
using L = General.LocalizationKeys;

namespace Server.Users;

public class AuthRegResponse
{
    public static DtoResponseAuthReg InvalidResponse() =>
        new(errorKey: L.Error.Server.invalidResponse);

    public static DtoResponseAuthReg InvalidCredentials() =>
        new(errorKey: L.Error.Server.invalidCredentials);

    public static DtoResponseAuthReg TooManyRequests(long seconds) =>
        new(errorKey: L.Error.Server.tooManyRequests, extraLong: seconds);

    public static DtoResponseAuthReg RequiresTwoFactor() =>
        new(errorKey: L.Error.Server.required2FA);

    public static DtoResponseAuthReg RefreshTokenErrorCreating() =>
        new(errorKey: L.Error.Server.refreshTokenErrorCreating);

    public static DtoResponseAuthReg UserAlreadyExists() =>
        new(errorKey: L.Error.Server.userAlreadyExists);

    public static DtoResponseAuthReg Banned(DateTimeOffset? until) =>
        new(errorKey: until == null ? L.Error.Server.accountBannedPermanently : L.Error.Server.accountBannedUntil,
            extraDateTimeOffset: until);

    public static DtoResponseAuthReg Success(string accessToken, string refreshToken, DateTimeOffset refreshTokenDtExpiration) =>
        new(accessToken: accessToken, refreshToken: refreshToken, extraDateTimeOffset: refreshTokenDtExpiration);
}
