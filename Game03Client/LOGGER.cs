using L = General.LocalizationKeys;

namespace Game03Client;

public delegate void LoggerCallbackError(object message);
public delegate void LoggerCallbackInfo(object message);
public static class LOGGER<T>
{
    public static LoggerCallbackError loggerCallbackError = null!;
    public static LoggerCallbackInfo loggerCallbackInfo = null!;

    public static void LogError(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        loggerCallbackError?.Invoke($"[{typeof(T).Name}] {message}");
    }

    public static void LogInfo(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        loggerCallbackInfo?.Invoke($"[{typeof(T).Name}] {message}");
    }

}
