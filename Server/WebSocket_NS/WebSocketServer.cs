using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.WebSocket_NS
{
    public class WebSocketServer : BackgroundService
    {
        private readonly ILogger<WebSocketServer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private HttpListener _httpListener;
        private readonly string _url;
        private readonly int _maxConnections;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<Guid, DateTime> _activeConnections = new();
        int _activeConnections_Count_Last = 0;
        private Timer _monitoringTimer;

        public WebSocketServer(ILogger<WebSocketServer> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _url = "http://localhost:5001/ws/"; // Отдельный порт для WebSocket
            _maxConnections = configuration.GetValue<int>("WebSocketSettings:MaxConnections", 1000);
            _configuration = configuration;
            _monitoringTimer = new Timer(LogConnectionStats, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.33));
            //Task.Run(() => {
            //    while (true)
            //    {
                   
                   
            //        Thread.Sleep(333);
            //    }
            //});

        }
        private void LogConnectionStats(object state)
        {
            //_logger.LogInformation("Активных подключений: {Count}", _activeConnections.Count);
            if (_activeConnections_Count_Last != _activeConnections.Count)
            {
                _activeConnections_Count_Last = _activeConnections.Count;
                Console.WriteLine($"АКТИВНЫХ ПОДКЛЮЧЕНИЙ: {_activeConnections.Count}");
            }
        }
        public int GetCount() { 
            return _activeConnections.Count;
        }

        public void ActiveConnectionsAdd(Guid guid)
        {
            _activeConnections.TryAdd(guid, DateTime.UtcNow);
            Console.WriteLine($"Активных подключений: {_activeConnections.Count}");
        }
        public void ActiveConnectionsRemove(Guid guid)
        {
            _activeConnections.TryRemove(guid, out _);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _httpListener = new HttpListener();

            // Увеличиваем лимиты производительности
            _httpListener.TimeoutManager.IdleConnection = TimeSpan.FromMinutes(30);
            _httpListener.TimeoutManager.EntityBody = TimeSpan.FromMinutes(30);
            _httpListener.TimeoutManager.DrainEntityBody = TimeSpan.FromMinutes(30);
            _httpListener.TimeoutManager.RequestQueue = TimeSpan.FromMinutes(30);
            _httpListener.TimeoutManager.HeaderWait = TimeSpan.FromMinutes(30);

            _httpListener.Prefixes.Add(_url);

            // Увеличиваем лимиты на количество подключений
            ServicePointManager.DefaultConnectionLimit = _maxConnections;
            ServicePointManager.MaxServicePoints = _maxConnections;

            _httpListener.Start();

            _logger.LogInformation("WebSocket сервер запущен на {Url}, макс. подключений: {MaxConnections}", _url, _maxConnections);

            try
            {
                // Используем SemaphoreSlim для ограничения одновременной обработки
                using var semaphore = new SemaphoreSlim(_maxConnections, _maxConnections);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await semaphore.WaitAsync(stoppingToken);

                    var context = await _httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        _ = ProcessWebSocketRequest(context, stoppingToken, semaphore);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                        semaphore.Release();
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
        private async Task ProcessWebSocketRequest(HttpListenerContext context, CancellationToken stoppingToken, SemaphoreSlim semaphore)
        {
            try
            {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                var webSocket = webSocketContext.WebSocket;

                using var scope = _serviceProvider.CreateScope();
                var clientLogger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketClient>>();

                var client = new WebSocketClient(webSocket, clientLogger, _configuration, this);
                await client.HandleAsync(stoppingToken);
            }
            catch (Exception ex) when (WebSocketClient.IsExpectedDisconnectException(ex))
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
                semaphore.Release();
                // Удаляем из мониторинга
                //_activeConnections.Clear();
                semaphore.Release();
            }
        }
    }
}