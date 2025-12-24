namespace Server;

/// <summary>
/// Предоставляет методы-расширения для <see cref="ILogger"/>, обеспечивающие безопасное и стандартизированное логирование ошибок, критических ошибок и предупреждений с поддержкой исключений.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Логирует ошибку с полным стек-трейсом, но без немедленного форматирования исключения.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    /// <param name="ex">Исключение.</param>
    /// <param name="message">Основное сообщение.</param>
    public static void LError(this ILogger logger, Exception ex, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            // Шаблон сообщения должен быть константой для устранения CA2254
            const string errorTemplate = "{Message}";
            logger.LogError(ex, errorTemplate, message);
        }
    }

    /// <summary>
    /// Логирует информационное сообщение с использованием стандартизированного шаблона.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    /// <param name="message">Информационное сообщение.</param>
    public static void LInfo(this ILogger logger, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            // Шаблон сообщения должен быть константой для устранения CA2254
            const string errorTemplate = "{Message}";
            logger.LogInformation(errorTemplate, message);
        }
    }

    /// <summary>
    /// Логирует предупреждение с использованием стандартизированного шаблона.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    /// <param name="message">Предупреждающее сообщение.</param>
    public static void LWarning(this ILogger logger, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Warning))
        {
            // Шаблон сообщения должен быть константой для устранения CA2254
            const string errorTemplate = "{Message}";
            logger.LogWarning(errorTemplate, message);
        }
    }
}
