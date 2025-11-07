using Server.Jwt_NS;
using Server_DB_UserData;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

namespace Server.WebSocket_NS;

public class WebSocketConnectionHandler : BackgroundService
{
    private readonly ILogger<WebSocketConnectionHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpListener _httpListener = new();
    private readonly string _url;
    private readonly int _maxConnections;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<Guid, DateTime> _activeConnections = new();
    private int _activeConnections_Count_Last = 0;
    private readonly Timer _monitoringTimer;
    private readonly MongoRepository _mongoRepository;
    private readonly JwtService _jwtService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="WebSocketConnectionHandler"/>.
    /// </summary>
    /// <param name="logger">Сервис логирования для записи информации и ошибок.</param>
    /// <param name="serviceProvider">Поставщик зависимостей для разрешения сервисов.</param>
    /// <param name="configuration">Конфигурация приложения. Используется для получения настроек WebSocket, включая URL сервера и максимальное количество подключений.</param>
    /// <param name="mongoRepository">Репозиторий для работы с MongoDB.</param>
    /// <param name="jwtService">JWT Сервис</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="configuration"/> или URL сервера равны null.</exception>
    public WebSocketConnectionHandler(ILogger<WebSocketConnectionHandler> logger, IServiceProvider serviceProvider, IConfiguration configuration, MongoRepository mongoRepository, JwtService jwtService)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;

        _logger = logger;
        _serviceProvider = serviceProvider;

        // Получаем URL из конфигурации
        var serverUrl = configuration["WebSocketSettings:ServerUrl"];
        ArgumentNullException.ThrowIfNull(serverUrl);
        _url = serverUrl;

        _maxConnections = configuration.GetValue<int>("WebSocketSettings:MaxConnections");
        _monitoringTimer = new Timer(LogConnectionStats, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.33));
        _mongoRepository = mongoRepository;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Процедура вызываемая таймером
    /// </summary>
    /// <param name="state"></param>
    private void LogConnectionStats(object? state)
    {
        //_logger.LogInformation("Активных подключений: {Count}", _activeConnections.Count);
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


    //private void ConfigureModernHttpLimits(int maxConnections)
    //{
    //    try
    //    {
    //        // Для .NET 5+ - настройка через AppContext
    //        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.MaxConnectionsPerServer", maxConnections);

    //        // Или через переменные окружения
    //        Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_MAXCONNECTIONSPERSERVER",
    //            maxConnections.ToString());

    //        Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_POOLEDCONNECTIONLIFETIME",
    //            "00:15:00"); // 15 минут

    //        _logger.LogInformation("Установлен лимит подключений: {MaxConnections}", maxConnections);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogWarning(ex, "Не удалось установить современные HTTP лимиты");
    //    }
    //}

    /// <summary>
    /// При старте сервера запускает веб сокет сервер.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //ConfigureTimeouts
        try
        {
            HttpListenerTimeoutManager timeoutManager = _httpListener.TimeoutManager;
            timeoutManager.IdleConnection = TimeSpan.FromMinutes(30);
            timeoutManager.DrainEntityBody = TimeSpan.FromMinutes(30);

            // Свойства, доступные только в Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                timeoutManager.EntityBody = TimeSpan.FromMinutes(30);
                timeoutManager.RequestQueue = TimeSpan.FromMinutes(30);
                timeoutManager.HeaderWait = TimeSpan.FromMinutes(30);
            }
        }
        catch (PlatformNotSupportedException)
        {
            _logger.LogInformation("Некоторые настройки таймаутов не поддерживаются на этой платформе");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при настройке таймаутов");
        }




        _httpListener.Prefixes.Add(_url);

        //ConfigureModernHttpLimits(_maxConnections);

        _httpListener.Start();

        _logger.LogInformation("WebSocket сервер запущен на {Url}, макс. подключений: {MaxConnections}", _url, _maxConnections);

        try
        {
            // Используем SemaphoreSlim для ограничения одновременной обработки
            using SemaphoreSlim semaphore = new(_maxConnections, _maxConnections);

            while (!stoppingToken.IsCancellationRequested)
            {
                await semaphore.WaitAsync(stoppingToken);

                HttpListenerContext context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    _ = ProcessWebSocketRequest(context, stoppingToken, semaphore);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                    _ = semaphore.Release();
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("WebSocket сервер остановлен");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка в WebSocket сервере");
        }
        finally
        {
            _httpListener.Stop();
            _httpListener.Close();
        }
    }

    /// <summary>
    /// Вызывается при запросе открытия веб сокета от клиента.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="stoppingToken"></param>
    /// <param name="semaphore"></param>
    /// <returns></returns>
    private async Task ProcessWebSocketRequest(HttpListenerContext context, CancellationToken stoppingToken, SemaphoreSlim semaphore)
    {
        try
        {
            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
            WebSocket webSocket = webSocketContext.WebSocket;

            using IServiceScope scope = _serviceProvider.CreateScope();
            ILogger<WebSocketConnection> clientLogger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketConnection>>();

            WebSocketConnection client = new(webSocket, clientLogger, _configuration, this, _mongoRepository, _jwtService);

            _ = _activeConnections.TryAdd(client.Id, DateTime.UtcNow);
            Console.WriteLine($"Активных подключений: {_activeConnections.Count}");

            await client.HandleAsync(stoppingToken);
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
        }
        finally
        {
            _ = semaphore.Release();
            _ = semaphore.Release();
        }
    }
}
