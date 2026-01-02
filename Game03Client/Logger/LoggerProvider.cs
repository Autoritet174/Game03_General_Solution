using General;
using System;
using System.Diagnostics.CodeAnalysis;
using L = General.LocalizationKeys;

namespace Game03Client.Logger;


public class LoggerProvider<T>(LoggerOptions loggerOptions)
{
    public void Log(object message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }
        loggerOptions._loggerCallback?.Invoke($"[{typeof(T).Name}] {message}");
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
            Log(message, keyLocal);
        }
        catch { }
        throw new Exception(message);
    }
}
