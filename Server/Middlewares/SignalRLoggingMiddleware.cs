namespace Server.Middlewares;

public class SignalRLoggingMiddleware(RequestDelegate next, ILogger<SignalRLoggingMiddleware> logger)
{
    private readonly ILogger logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/hub"))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("SignalR Request: {body}", body);
            }

            context.Request.Body.Position = 0;
        }

        await next(context).ConfigureAwait(false);
    }
}
