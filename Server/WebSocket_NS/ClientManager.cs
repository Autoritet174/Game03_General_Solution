//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Concurrent;
//using System.Net.WebSockets;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Server.WebSocket_NS
//{
//    public class ClientManager : BackgroundService
//    {
//        private readonly ClientQueue _queue;
//        private readonly ILogger<ClientManager> _logger;
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IConfiguration _configuration;

//        public ClientManager(ClientQueue queue, ILogger<ClientManager> logger, IServiceProvider serviceProvider, IConfiguration configuration)
//        {
//            _queue = queue;
//            _logger = logger;
//            _serviceProvider = serviceProvider;
//            _configuration = configuration;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("ClientManager запущен");

//            try
//            {
//                await foreach (var socket in _queue.Reader.ReadAllAsync(stoppingToken))
//                {
//                    if (stoppingToken.IsCancellationRequested)
//                        break;

//                    // Обрабатываем каждого клиента в отдельной задаче
//                    _ = Task.Run(() => ProcessClientAsync(socket, stoppingToken), stoppingToken);
//                }
//            }
//            catch (OperationCanceledException)
//            {
//                _logger.LogInformation("ClientManager остановлен");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Ошибка в ClientManager");
//            }
//        }

//        private async Task ProcessClientAsync(WebSocket socket, CancellationToken stoppingToken)
//        {
//            using var scope = _serviceProvider.CreateScope();
//            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebSocketClient>>();

//            var client = new WebSocketClient(socket, logger, _configuration);

//            try
//            {
//                await client.HandleAsync(stoppingToken);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "Ошибка при обработке клиента {ClientId}", client.Id);
//            }
//        }
//    }
//}