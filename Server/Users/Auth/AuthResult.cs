using L = General.LocalizationKeys;
namespace Server.Users.Auth;

public sealed record AuthResult
{
    public bool Success { get; init; }
    public string? Token { get; init; }
    public string? ErrorKey { get; init; }
    public object? Extra { get; init; }

    public static AuthResult SuccessResponse(string token)
    {
        return new() { Success = true, Token = token };
    }

    public static AuthResult InvalidCredentials()
    {
        return new() { ErrorKey = L.Error.Server.InvalidCredentials };
    }

    public static AuthResult TooManyRequests(long seconds)
    {
        return new() { ErrorKey = L.Error.Server.TooManyRequests, Extra = seconds };
    }

    public static AuthResult Banned(DateTimeOffset? until)
    {
        return new()
        {
            ErrorKey = until == null
                    ? L.Error.Server.AccountBannedPermanently
                    : L.Error.Server.AccountBannedUntil,
            Extra = until
        };
    }

    public static AuthResult RequiresTwoFactor(Guid userId)
    {
        return new() { ErrorKey = "2fa_required", Extra = userId };
    }

    public static AuthResult BadRequestInvalidResponse()
    {
        return new() { ErrorKey = L.Error.Server.InvalidResponse };
    }
}
