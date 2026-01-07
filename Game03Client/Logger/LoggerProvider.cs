using General;
using System;
using System.Diagnostics.CodeAnalysis;
using L = General.LocalizationKeys;

namespace Game03Client.Logger;


public class LoggerProvider<T>(LoggerOptions loggerOptions)
{
    public void LogError(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        loggerOptions._loggerCallbackError?.Invoke($"[{typeof(T).Name}] {message}");
    }
    public void LogInfo(object message, string? keyLocal = null)
    {
        if (!string.IsNullOrWhiteSpace(keyLocal))
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        loggerOptions._loggerCallbackInfo?.Invoke($"[{typeof(T).Name}] {message}");
    }

    /// <summary>
    /// Отображает сообщение об ошибке и создает новое исключение останавливая выполнение кода.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="keyLocal"></param>
    /// <exception cref="Exception"></exception>
    [DoesNotReturn]
    public void LogAndThrow(string message, string? keyLocal = null)
    {
        try
        {
            LogError(message, keyLocal);
        }
        catch { }
        throw new Exception(message);
    }
}
