using System;
using L = General.LocalizationKeys;

namespace Game03Client;

public delegate void LoggerCallbackError(object message);
public delegate void LoggerCallbackInfo(object message);

public static class LoggerProvider
{
    public static LoggerCallbackError? LoggerCallbackError { get; set; }
    public static LoggerCallbackInfo? LoggerCallbackInfo { get; set; }
}

public class Logger<T>
{

    private readonly string _className;
    public Logger()
    {
        _className = typeof(T).Name;
    }

    public void LogError(string message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        if (LoggerProvider.LoggerCallbackError is null)
        {
            throw new InvalidOperationException("LoggerCallbackError is not set.");
        }
        LoggerProvider.LoggerCallbackError.Invoke($"[{_className}] {message}");
    }
    public void LogException(Exception ex, string? keyLocal = null) {
        LogError(ex.Message, keyLocal);
    }

    public void LogInfo(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        if (LoggerProvider.LoggerCallbackInfo is null)
        {
            throw new InvalidOperationException("LoggerCallbackInfo is not set.");
        }
        LoggerProvider.LoggerCallbackInfo.Invoke($"[{_className}] {message}");
    }

}
