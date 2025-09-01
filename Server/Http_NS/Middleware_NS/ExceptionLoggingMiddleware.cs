namespace Server.Http_NS.Middleware_NS;

/// <summary>
/// Middleware для глобального перехвата исключений и логирования в файл.
/// </summary>
/// <remarks>
/// Конструктор.
/// </remarks>
public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{

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
            if (!context.Response.HasStarted)
            {
                logger.LogError(ex, "Произошло необработанное исключение.");
                await Common.WriterExceptionInLogFile.LogToFileAsync(ex);

                // Минимальный JSON-ответ
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("""{"error":"Internal Server Error"}""");
            }
            else
            {
                logger.LogError(ex, "Ошибка после начала Response");
            }
        }
    }

}
