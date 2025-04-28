namespace Server.Http_NS.Middleware_NS;
public class SecurityHeadersMiddleware(RequestDelegate next) {
    public async Task InvokeAsync(HttpContext context) {
        // Добавление заголовков безопасности
        IHeaderDictionary headers = context.Response.Headers;
        headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
        headers.XContentTypeOptions = "nosniff";
        headers.XFrameOptions = "DENY";
        headers.XXSSProtection = "1; mode=block";
        headers.ContentSecurityPolicy = "default-src 'self'";

        await next(context);
    }
}
