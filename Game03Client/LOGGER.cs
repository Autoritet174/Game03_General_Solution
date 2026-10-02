using System;
using L = General.LocalizationKeys;

namespace Game03Client;

public delegate void LoggerCallbackError(object message);
public delegate void LoggerCallbackInfo(object message);

public static class LoggerProvider
{
    public static LoggerCallbackError? loggerCallbackError { get; set; }
    public static LoggerCallbackInfo? loggerCallbackInfo { get; set; }
}

public class Logger<T>
{

    private readonly string className;
    public Logger()
    {
        className = typeof(T).Name;
    }

    public void LogError(string message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.keyLocalization}:<{keyLocal}>";
        }
        if (LoggerProvider.loggerCallbackError is null)
        {
            throw new InvalidOperationException("LoggerCallbackError is not set.");
        }
        LoggerProvider.loggerCallbackError.Invoke($"[{className}] {message}");
    }
    public void LogException(Exception ex, string? keyLocal = null)
    {
        LogError(ex.Message, keyLocal);
    }

    public void LogInfo(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.keyLocalization}:<{keyLocal}>";
        }
        if (LoggerProvider.loggerCallbackInfo is null)
        {
            throw new InvalidOperationException("LoggerCallbackInfo is not set.");
        }
        LoggerProvider.loggerCallbackInfo.Invoke($"[{className}] {message}");
    }

}
