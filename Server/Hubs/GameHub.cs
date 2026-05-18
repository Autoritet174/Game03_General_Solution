
using General.DTO.Battlefield;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Server.Hubs;

[Authorize]
public class GameHub(ClientManager clientManager, IClientFactory clientFactory, ILogger<GameHub> logger) : Hub
{
    private static readonly Action<ILogger, Exception?> _logUserExtractionFailed =
       LoggerMessage.Define(LogLevel.Warning, new EventId(1, "UserExtractionFailed"),
           "Не удалось извлечь userId из токена. Отклоняем подключение.");

    private static readonly Action<ILogger, string, Exception?> _logClientAddFailed =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(2, "ClientAddFailed"),
            "Не удалось добавить клиента ConnectionId={ConnectionId} в ClientManager");

    private static readonly Action<ILogger, string, Guid, Exception?> _logClientConnected =
        LoggerMessage.Define<string, Guid>(LogLevel.Information, new EventId(3, "ClientConnected"),
            "Client: ConnectionId={ConnectionId}; userId: {UserId}; connected");

    private static readonly Action<ILogger, string, Guid, Exception?> _logClientDisconnected =
        LoggerMessage.Define<string, Guid>(LogLevel.Information, new EventId(4, "ClientDisconnected"),
            "Client: ConnectionId={ConnectionId}$ userId: {UserId}; disconnected");

    private static readonly Action<ILogger, string, Exception?> _logClientNotFoundOnDisconnect =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(5, "ClientNotFoundOnDisconnect"),
            "Client ConnectionId={ConnectionId} не найден в ClientManager при отключении");

    private static readonly Action<ILogger, string, Exception?> _logClientNotFoundForMethod =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(6, "ClientNotFoundForMethod"),
            "Client не найден для ConnectionId={ConnectionId}");

    public override async Task OnConnectedAsync()
    {
        string? userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
        {
            _logUserExtractionFailed(logger, null);
            Context.Abort();
            return;
        }

        Client client = clientFactory.Create(userId);
        bool added = clientManager.TryAdd(Context.ConnectionId, client);

        if (!added)
        {
            _logClientAddFailed(logger, Context.ConnectionId, null);
            Context.Abort();
            return;
        }

        await Clients.Caller.SendAsync("ReceiveLog", $"Добро пожаловать! Ваш userId: {userId}").ConfigureAwait(false);
        _logClientConnected(logger, Context.ConnectionId, userId, null);

        await base.OnConnectedAsync().ConfigureAwait(false);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (clientManager.TryRemove(Context.ConnectionId, out Client? client))
        {
            _logClientDisconnected(logger, Context.ConnectionId, client?.UserId ?? Guid.Empty, null);
        }
        else
        {
            _logClientNotFoundOnDisconnect(logger, Context.ConnectionId, null);
        }
        await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
    }

    private Client? GetClient()
    {
        Client? client = clientManager.Get(Context.ConnectionId);
        if (client == null)
        {
            _logClientNotFoundForMethod(logger, Context.ConnectionId, null);
        }
        return client;
    }

    [HubMethodName(HubMethodNames.PING)]
    public async Task<bool> PingAsync()
    {
        return true;
    }

    [HubMethodName(HubMethodNames.EQUIPMENT_TAKE_ON)]
    public async Task<bool> EquipmentTakeOnAsync(Guid heroId, Guid equipmentId, bool? inAltSlot)
    {
        Client? client = GetClient();
        return client != null && await client.EquipmentTakeOnAsync(heroId, equipmentId, inAltSlot, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.EQUIPMENT_TAKE_OFF)]
    public async Task<bool> EquipmentTakeOffAsync(Guid equipmentId)
    {
        Client? client = GetClient();
        return client != null && await client.EquipmentTakeOffAsync(equipmentId, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.COMBAT_START)]
    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId)
    {
        Client? client = GetClient();
        return client == null
            ? null
            : await client.CombatStartAsync(eBattleFiled, spawnedHeroesId, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.COMBAT_BREAK)]
    public async Task<bool> CombatBreakAsync() => GetClient()?.CombatBreak() ?? false;

    [HubMethodName(HubMethodNames.USE_ABILITY)]
    public async Task<bool> UseAbilityAsync(EAbility eAbility, Guid heroSpawnedId, Guid? target)
    {
        Client? client = GetClient();
        return client != null && await client.UseAbilityAsync(eAbility, heroSpawnedId, target).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.GET_BATTLE_LOG)]
    public async Task<List<BattlefieldLogRecord>?> GetBattleLogAsync() => GetClient()?.GetBattleLog();


    /*
    /// <summary>
    /// Отправляет данные конкретному клиенту по ConnectionId
    /// </summary>
    /// <typeparam name="T">Тип отправляемых данных</typeparam>
    /// <param name="connectionId">ID подключения получателя</param>
    /// <param name="methodName">Имя метода на клиенте</param>
    /// <param name="data">Данные для отправки</param>
    /// <returns>true - если отправка успешна, false - если клиент не найден</returns>
    public async Task<bool> SendToClientAsync<T>(string connectionId, string methodName, T data)
    {
        try
        {
            // Проверяем, существует ли клиент с таким ConnectionId
            Client? client = clientManager.Get(connectionId);
            if (client == null)
            {
                logger.LogWarning("Попытка отправить данные несуществующему клиенту. ConnectionId: {ConnectionId}", connectionId);
                return false;
            }

            // Отправляем данные
            await Clients.Client(connectionId).SendAsync(methodName, data).ConfigureAwait(false);
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Данные отправлены клиенту {ConnectionId}. Метод: {MethodName}, Тип: {DataType}",
                connectionId, methodName, typeof(T).Name);
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при отправке данных клиенту {ConnectionId}. Метод: {MethodName}",
                connectionId, methodName);
            return false;
        }
    }
    */
}
