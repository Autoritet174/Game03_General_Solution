using Server.Jwt_NS;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Net; // Добавлено для HttpStatusCode

namespace Server.WebSocket_NS;

/// <summary>
/// Обработчик подключений WebSocket. Работает как Singleton-сервис и интегрирован 
/// в пайплайн ASP.NET Core Middleware через app.Map().
/// </summary>
public class WebSocketConnectionHandler // УДАЛЕНО: : BackgroundService
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
    /// <param name="mongoRepository">Репозиторий для работы с MongoDB.</param>
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
            await context.Response.WriteAsync("Запрос должен быть WebSocket-запросом.");
            return;
        }

        try
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync((string?)null);

            using IServiceScope scope = _serviceProvider.CreateScope();
            ILogger<WebSocketConnection> clientLogger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketConnection>>();

            // Создание и инициализация нового WebSocketConnection
            WebSocketConnection webSocketConnection = new(webSocket, clientLogger, _configuration, this, _jwtService);

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
