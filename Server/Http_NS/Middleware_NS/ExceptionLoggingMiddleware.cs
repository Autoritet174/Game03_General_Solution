namespace Server.Http_NS.Middleware_NS;

/// <summary>
/// Middleware для глобального перехвата исключений и логирования в файл.
/// </summary>
/// <remarks>
/// Конструктор.
/// </remarks>
public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{

    private static readonly string logDir;

    static ExceptionLoggingMiddleware()
    {
        logDir = Path.Combine(AppContext.BaseDirectory, "logs");
        _ = Directory.CreateDirectory(logDir);
    }

    /// <summary>
    /// Основной метод middleware.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошло необработанное исключение.");
            await LogToFileAsync(ex);

            // Минимальный JSON-ответ
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("""{"error":"Internal Server Error"}""");
        }
    }

    /// <summary>
    /// Логирует исключение в файл.
    /// </summary>
    private static async Task LogToFileAsync(Exception exception)
    {
        DateTime now = DateTime.UtcNow;
        string logPath = Path.Combine(logDir, $"exceptions_[{now:yyyy-MM-dd}].log");
        string logEntry = $"[{now:yyyy.MM.dd HH:mm:ss.fff}] {exception}\n\n";
        await File.AppendAllTextAsync(logPath, logEntry);
    }
}
