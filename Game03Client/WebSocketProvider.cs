using General;
using General.DTO;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

public class WebSocketProvider
{
    private static readonly Logger<WebSocketProvider> logger = new();
    private const string SERVER_URL = "wss://localhost:7227/ws/";
    private static ClientWebSocket _webSocket = new();
    private static readonly Uri _serverUri = new(SERVER_URL);
    private static bool _isReceiving = false;

    public static async Task<bool> ConnectAsync(CancellationToken cancellationTokenOpen, CancellationToken cancellationTokenReceive)
    {
        if (cancellationTokenOpen.IsCancellationRequested || cancellationTokenReceive.IsCancellationRequested)
        {
            logger.LogError("Подключение отменено");
            return false;
        }

        try
        {
            _webSocket = new();
            if (!string.IsNullOrWhiteSpace(Auth.AccessToken))
            {
                // Добавляем JWT токен в заголовки, если он предоставлен
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {Auth.AccessToken}");
            }
            await _webSocket.ConnectAsync(_serverUri, cancellationTokenOpen).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        if (_webSocket.State == WebSocketState.Open)
        {
            _isReceiving = true;
            _ = Task.Run(async () => { await ReceiveMessagesAsync(cancellationTokenReceive).ConfigureAwait(false); });
            return true;
        }

        logger.LogError("Не удалось подключиться к серверу WebSocket");
        return false;
    }

    /// <summary>
    /// Приём сообщений от сервера.
    /// </summary>
    /// <returns></returns>
    private static async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[4096];

        try
        {
            while (_isReceiving && _webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationToken
                ).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                try
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    logger.LogInfo($"Получено от сервера: {message}");
                    //DtoWebSocket? dtoWebSocket = JsonSerializer.Deserialize<DtoWebSocket>(message);
                    //if (dtoWebSocket != null)
                    //{
                    //    if (dtoWebSocket.Command == WebSocketCommand.EquipmentTakeOn)
                    //    {

                    //    }
                    //}
                }
                catch (Exception ex) {
                    logger.LogError($"Ошибка обработки сообщения от сервера: {ex}");
                }
                //ProcessReceivedMessage(message);
            }
        }
        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
        {
            logger.LogInfo("Соединение закрыто сервером");
        }
        catch (Exception ex)
        {
            if (_isReceiving) // Логируем только если не было запланированного отключения
            {
                logger.LogError($"Ошибка приема: {ex.Message}");
            }
        }
    }


    /// <summary>
    /// Отправить сообщение на сервер.
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        if (_webSocket.State != WebSocketState.Open)
        {
            logger.LogError("WebSocket не подключен");
            return false;
        }

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            ).ConfigureAwait(false);
            return true;
        }
        catch (OperationCanceledException)
        {
            logger.LogError("Отправка отменена");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка отправки: {ex.Message}");
            return false;
        }
    }

    public static async Task<bool> SendMessageAsync(EquipmentTakeOnMessage equipmentTakeOnMessage, CancellationToken cancellationToken)
    {
        string message = JsonSerializer.Serialize(equipmentTakeOnMessage);
        return await SendMessageAsync(message, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Корректно отключает клиента от сервера.
    /// </summary>
    /// <returns>Задача асинхронного отключения.</returns>
    public static async Task DisconnectAsync()
    {
        try
        {
            _isReceiving = false;

            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                // CloseAsync отправляет фрейм закрытия и ждет подтверждения от сервера.
                // Это корректный способ завершить соединение, который сервер поймет как CloseMessageType.
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Закрытие клиентом",
                    timeoutCts.Token).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка при отключении: {ex.Message}");
        }
        finally
        {
            _webSocket.Dispose();
            logger.LogInfo("Соединение закрыто");
        }
    }
}
