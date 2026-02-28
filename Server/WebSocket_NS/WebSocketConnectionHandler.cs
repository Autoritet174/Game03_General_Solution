using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Server.Cache;
using Server.Game;
using Server.Jwt_NS;
using Server_DB_Postgres;
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
    private readonly ILogger<WebSocketConnectionHandler> _Logger;
    private readonly IServiceProvider _ServiceProvider;
    private readonly IConfiguration _Configuration;
    private readonly ConcurrentDictionary<Guid, DateTime> _ActiveConnections = new();
    private int _ActiveConnections_Count_Last = 0;
    private readonly Timer _MonitoringTimer;
    private readonly JwtService _JwtService;
    private readonly int _MaxConnections;
    private readonly IDbContextFactory<DbContextGame> _DbContextFactory;
    private readonly CacheService _CacheService;
    private readonly LootGenerator _LootGenerator;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="WebSocketConnectionHandler"/>.
    /// </summary>
    public WebSocketConnectionHandler(
        ILogger<WebSocketConnectionHandler> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        JwtService jwtService,
        IDbContextFactory<DbContextGame> dbContextFactory,
        CacheService cacheService,
        LootGenerator lootGenerator)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _Configuration = configuration;

        _Logger = logger;
        _ServiceProvider = serviceProvider;

        _MaxConnections = configuration.GetValue<int>("WebSocketSettings:MaxConnections");

        _MonitoringTimer = new Timer(LogConnectionStats, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.33));
        _JwtService = jwtService;
        _DbContextFactory = dbContextFactory;
        _CacheService = cacheService;
        _LootGenerator = lootGenerator;
    }

    private void LogConnectionStats(object? state)
    {
        if (_ActiveConnections_Count_Last != _ActiveConnections.Count)
        {
            _ActiveConnections_Count_Last = _ActiveConnections.Count;
            if (_Logger.IsEnabled(LogLevel.Information))
            {
                _Logger.LogInformation("Активных подключений: {Count}", _ActiveConnections.Count);
            }
        }
    }

    public int GetCount()
    {
        return _ActiveConnections.Count;
    }

    public void ActiveConnectionsRemove(Guid guid)
    {
        _ = _ActiveConnections.TryRemove(guid, out _);
    }

    /// <summary>
    /// Вызывается при запросе открытия веб сокета от клиента через пайплайн ASP.NET Core.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса от Kestrel.</param>
    /// <param name="stoppingToken">Токен отмены.</param>
    /// <returns>Асинхронная задача обработки соединения.</returns>
    /// <exception cref="ArgumentNullException">Если контекст равен null.</exception>
    public async Task ProcessKestrelWebSocketRequestAsync(HttpContext context, CancellationToken stoppingToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Запрос должен быть WebSocket-запросом.", cancellationToken: stoppingToken).ConfigureAwait(false);
            return;
        }



        string? token = null;

        // Пытаемся из Authorization: Bearer <token>
        if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader) &&
            authHeader.Count > 0 &&
            authHeader[0]?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            token = authHeader[0]!["Bearer ".Length..].Trim();
        }

        // Если не найден в заголовке — из query string (для совместимости с клиентами, где header не поддерживается)
        if (string.IsNullOrWhiteSpace(token))
        {
            token = context.Request.Query["token"].FirstOrDefault() ?? context.Request.Query["access_token"].FirstOrDefault();
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Missing authentication token", cancellationToken: stoppingToken).ConfigureAwait(false);
            return;
        }

        Guid? userId;
        try
        {
            ClaimsPrincipal claims = _JwtService.ValidateToken(token);
            userId = claims.GetGuid(); // Метод расширения из ClaimsExtensions.cs

            if (!userId.HasValue)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid user ID in token", cancellationToken: stoppingToken).ConfigureAwait(false);
                return;
            }

            if (_Logger.IsEnabled(LogLevel.Information))
            {
                _Logger.LogInformation("Pre-authenticated WebSocket connection for user {UserId}", userId.Value);
            }
        }
        catch (SecurityTokenException ex)
        {
            if (_Logger.IsEnabled(LogLevel.Warning))
            {
                _Logger.LogWarning(ex, "Invalid JWT token in WebSocket upgrade request");
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid token", cancellationToken: stoppingToken).ConfigureAwait(false);
            return;
        }
        catch (Exception ex)
        {
            if (_Logger.IsEnabled(LogLevel.Error))
            {
                _Logger.LogError(ex, "Error validating token in WebSocket upgrade");
            }
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        try
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync((string?)null).ConfigureAwait(false);

            using IServiceScope scope = _ServiceProvider.CreateScope();
            ILogger<WebSocketConnection> clientLogger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketConnection>>();

            // Создание и инициализация нового WebSocketConnection
            WebSocketConnection webSocketConnection = new(webSocket, clientLogger, _Configuration, this//, _serviceProvider
                , userId.Value, _DbContextFactory, _CacheService);

            _ = _ActiveConnections.TryAdd(webSocketConnection.Id, DateTime.UtcNow);
            if (_Logger.IsEnabled(LogLevel.Information))
            {
                _Logger.LogInformation("Активных подключений: {Count}", _ActiveConnections.Count);
            }
            // CancellationToken передаётся в HandleAsync
            await webSocketConnection.HandleAsync(stoppingToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (WebSocketConnection.IsExpectedDisconnectException(ex))
        {
            _Logger.LogInformation("WebSocket соединение было прервано клиентом");
        }
        catch (OperationCanceledException)
        {
            _Logger.LogInformation("WebSocket обработка прервана");
        }
        catch (Exception ex)
        {
            _Logger.LogError(ex, "Ошибка при обработке WebSocket запроса");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
