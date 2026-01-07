using Microsoft.IdentityModel.Tokens;
using RTools_NTS.Util;
using Server.Jwt_NS;
using System.Collections.Concurrent;
using System.Net; // Добавлено для HttpStatusCode
using System.Net.WebSockets;
using System.Security.Claims;

namespace Server.WebSocket_NS;

/// <summary>
/// Обработчик подключений WebSocket. Работает как Singleton-сервис и интегрирован 
/// в пайплайн ASP.NET Core Middleware через app.Map().
/// </summary>
public class WebSocketConnectionHandler
{
    private readonly ILogger<WebSocketConnectionHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<Guid, DateTime> _activeConnections = new();
    private int _activeConnections_Count_Last = 0;
    private readonly Timer _monitoringTimer;
    private readonly JwtService _jwtService;
    private readonly int _maxConnections;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="WebSocketConnectionHandler"/>.
    /// </summary>
    /// <param name="logger">Сервис логирования для записи информации и ошибок.</param>
    /// <param name="serviceProvider">Поставщик зависимостей для разрешения сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="jwtService">JWT Сервис</param>
    public WebSocketConnectionHandler(ILogger<WebSocketConnectionHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration, JwtService jwtService)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;

        _logger = logger;
        _serviceProvider = serviceProvider;

        _maxConnections = configuration.GetValue<int>("WebSocketSettings:MaxConnections");

        _monitoringTimer = new Timer(LogConnectionStats, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.33));
        _jwtService = jwtService;
    }

    // protected override async Task ExecuteAsync(CancellationToken stoppingToken) - УДАЛЕН

    private void LogConnectionStats(object? state)
    {
        if (_activeConnections_Count_Last != _activeConnections.Count)
        {
            _activeConnections_Count_Last = _activeConnections.Count;
            Console.WriteLine($"АКТИВНЫХ ПОДКЛЮЧЕНИЙ: {_activeConnections.Count}");
        }
    }

    public int GetCount()
    {
        return _activeConnections.Count;
    }

    public void ActiveConnectionsRemove(Guid guid)
    {
        _ = _activeConnections.TryRemove(guid, out _);
    }

    /// <summary>
    /// Вызывается при запросе открытия веб сокета от клиента через пайплайн ASP.NET Core.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса от Kestrel.</param>
    /// <param name="stoppingToken">Токен отмены.</param>
    /// <returns>Асинхронная задача обработки соединения.</returns>
    /// <exception cref="ArgumentNullException">Если контекст равен null.</exception>
    public async Task ProcessKestrelWebSocketRequest(HttpContext context, CancellationToken stoppingToken)
    {
        //Console.WriteLine("Запрос на вебсокет. Обработка Kestrel."); // Ваш отладочный вывод
        ArgumentNullException.ThrowIfNull(context);

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Запрос должен быть WebSocket-запросом.", cancellationToken: stoppingToken);
            return;
        }



        string? token = null;

        // 1. Пытаемся из Authorization: Bearer <token>
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
            authHeader.Count > 0 &&
            authHeader[0]?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            token = authHeader[0]!["Bearer ".Length..].Trim();
        }

        // 2. Если не найден в заголовке — из query string (для совместимости с клиентами, где header не поддерживается)
        if (string.IsNullOrWhiteSpace(token))
        {
            token = context.Request.Query["token"].FirstOrDefault()
                    ?? context.Request.Query["access_token"].FirstOrDefault();
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Missing authentication token", cancellationToken: stoppingToken);
            return;
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Missing authentication token", cancellationToken: stoppingToken);
            return;
        }

        Guid? userId;
        try
        {
            ClaimsPrincipal claims = _jwtService.ValidateToken(token);
            userId = claims.GetGuid(); // Метод расширения из ClaimsExtensions.cs

            if (!userId.HasValue)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid user ID in token", cancellationToken: stoppingToken);
                return;
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Pre-authenticated WebSocket connection for user {UserId}", userId.Value);
            }
        }
        catch (SecurityTokenException ex)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(ex, "Invalid JWT token in WebSocket upgrade request");
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid token", cancellationToken: stoppingToken);
            return;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Error validating token in WebSocket upgrade");
            }
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        try
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync((string?)null);

            using IServiceScope scope = _serviceProvider.CreateScope();
            ILogger<WebSocketConnection> clientLogger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketConnection>>();

            // Создание и инициализация нового WebSocketConnection
            WebSocketConnection webSocketConnection = new(webSocket, clientLogger, _configuration, this, _serviceProvider, userId.Value);

            _ = _activeConnections.TryAdd(webSocketConnection.Id, DateTime.UtcNow);
            Console.WriteLine($"Активных подключений: {_activeConnections.Count}");

            // CancellationToken передаётся в HandleAsync
            await webSocketConnection.HandleAsync(stoppingToken);
        }
        catch (Exception ex) when (WebSocketConnection.IsExpectedDisconnectException(ex))
        {
            _logger.LogInformation("WebSocket соединение было прервано клиентом");
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("WebSocket обработка прервана");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке WebSocket запроса");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
