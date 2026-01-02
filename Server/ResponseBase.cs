using L = General.LocalizationKeys;

namespace Server;

public abstract record ResponseBase<T> where T : ResponseBase<T>, new()
{
    public bool Success { get; init; }
    public string? ErrorKey;
    public object? Extra;

    public static T OK(string? value = null)
    {
        return value == null
            ? new T { Success = true }
            : new T { Success = true, Extra = value };
    }

    public static T InvalidCredentials()
    {
        return new T { ErrorKey = L.Error.Server.InvalidCredentials };
    }

    public static T TooManyRequests(long seconds)
    {
        return new T { ErrorKey = L.Error.Server.TooManyRequests, Extra = seconds };
    }

    public static T InvalidResponse()
    {
        return new T { ErrorKey = L.Error.Server.InvalidResponse };
    }
}
