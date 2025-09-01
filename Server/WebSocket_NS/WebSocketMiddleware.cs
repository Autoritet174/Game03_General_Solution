using Server.Jwt_NS;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Server.WebSocket_NS;

/// <summary>
/// Обрабатывает входящие WebSocket подключения и управляет клиентами.
/// </summary>
/// <remarks>
/// Конструктор.
/// </remarks>
public class WebSocketMiddleware(RequestDelegate next, ClientManager2 clientManager, JwtService jwtService) {
   

    /// <summary>
    /// Вызывается при входящем HTTP/WebSocket запросе.
    /// Обрабатывает HTTP-запросы, инициирующие подключение через WebSocket.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса</param>
    public async Task InvokeAsync(HttpContext context) {
        // Проверяем, что это запрос на установку WebSocket-соединения
        if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest) {
            // Извлекаем токен из query-параметра
            string? token = context.Request.Query["access_token"].FirstOrDefault();

            if (string.IsNullOrEmpty(token)) {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Missing access_token");
                return;
            }

            // Валидация токена через централизованный валидатор
            ClaimsPrincipal? principal = jwtService.ValidateToken(token);

            if (principal == null) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid or expired token");
                return;
            }

            // Извлечение userId из claim 'sub' или 'NameIdentifier'
            string? userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? principal.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId)) {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid userId");
                return;
            }

            // Принимаем WebSocket-соединение
            using System.Net.WebSockets.WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            // Регистрируем WebSocket у клиента
            clientManager.AddSocket(userId, webSocket);

            // Чтение сообщений от клиента в отдельном потоке
            await ReceiveLoop(userId, webSocket);

            // После завершения работы соединения удаляем сокет
            clientManager.RemoveSocket(userId, webSocket);
        }
        else {
            // Если это не WebSocket-запрос — передаем управление следующему middleware
            await next(context);
        }
    }


    /// <summary>
    /// Получает и валидирует JWT токен.
    /// </summary>
    //private ClaimsPrincipal? ValidateJwt(string token) {
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

    //    try {
    //        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidIssuer = _configuration["Jwt:Issuer"],
    //            ValidAudience = _configuration["Jwt:Audience"],
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateLifetime = true
    //        }, out _);

    //        return principal;
    //    }
    //    catch {
    //        return null;
    //    }
    //}

    /// <summary>
    /// Цикл чтения входящих сообщений от WebSocket клиента.
    /// </summary>
    private async Task ReceiveLoop(string userId, WebSocket socket) {
        byte[] buffer = new byte[4096];

        while (socket.State == WebSocketState.Open) {
            try {
                WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close) {
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                // Логирование
                clientManager.HandleClientMessage(userId, message);
            }
            catch (Exception) {

            }
        }
    }
}
