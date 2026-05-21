namespace Server.Middlewares;

public class SignalRLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public SignalRLoggingMiddleware(RequestDelegate next, ILogger<SignalRLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/hub"))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("SignalR Request: {body}", body);
            }

            context.Request.Body.Position = 0;
        }

        await _next(context).ConfigureAwait(false);
    }
}
