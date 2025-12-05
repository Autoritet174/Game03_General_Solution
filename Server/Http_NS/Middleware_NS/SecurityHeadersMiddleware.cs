namespace Server.Http_NS.Middleware_NS;

/// <summary>
/// Middleware для добавления HTTP-заголовков безопасности.
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="SecurityHeadersMiddleware"/>.
/// </remarks>
/// <param name="next">Следующий middleware в конвейере обработки запроса.</param>
public class SecurityHeadersMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Обрабатывает HTTP-запрос, добавляя заголовки безопасности к ответу.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>Задача, представляющая асинхронную операцию обработки запроса.</returns>
    /// <remarks>
    /// Добавляет следующие заголовки безопасности:
    /// <list type="bullet">
    /// <item><description>Strict-Transport-Security: Принудительное использование HTTPS</description></item>
    /// <item><description>X-Content-Type-Options: Запрет MIME-sniffing</description></item>
    /// <item><description>X-Frame-Options: Запрет встраивания в iframe</description></item>
    /// <item><description>X-XSS-Protection: Защита от XSS-атак</description></item>
    /// <item><description>Content-Security-Policy: Политика безопасности контента</description></item>
    /// </list>
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        //// Добавление заголовков безопасности
        //IHeaderDictionary headers = context.Response.Headers;
        //headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
        //headers.XContentTypeOptions = "nosniff";
        //headers.XFrameOptions = "DENY";
        //headers.XXSSProtection = "1; mode=block";
        //headers.ContentSecurityPolicy = "default-src 'self'";

        //await next(context);


        IHeaderDictionary headers = context.Response.Headers;

        // Обязательные заголовки для API
        headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
        headers.XContentTypeOptions = "nosniff";
        headers.XFrameOptions = "DENY";

        // Устаревший, но безвредный
        headers.XXSSProtection = "1; mode=block";

        // Для API часто нужны более гибкие настройки
        // headers.ContentSecurityPolicy = "default-src 'self'; connect-src 'self' https://api.example.com";

        // Или удалите CSP для API, если не обслуживаете HTML
        // headers.Remove("Content-Security-Policy");

        await next(context);
    }
}
