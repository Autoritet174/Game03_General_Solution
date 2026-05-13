
using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
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

    private Client? GetClientOrLog()
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
        Client? client = GetClientOrLog();
        return client != null && await client.EquipmentTakeOnAsync(heroId, equipmentId, inAltSlot, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.EQUIPMENT_TAKE_OFF)]
    public async Task<bool> EquipmentTakeOffAsync(Guid equipmentId)
    {
        Client? client = GetClientOrLog();
        return client != null && await client.EquipmentTakeOffAsync(equipmentId, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.COMBAT_START)]
    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId)
    {
        Client? client = GetClientOrLog();
        if (client == null)
        {
            return null;
        }
        return await client.CombatStartAsync(eBattleFiled, spawnedHeroesId, Context.ConnectionAborted).ConfigureAwait(false);
    }

    [HubMethodName(HubMethodNames.COMBAT_BREAK)]
    public async Task<bool> CombatBreakAsync() {
        Client? client = GetClientOrLog();
        return client != null && await client.CombatBreakAsync().ConfigureAwait(false);
    }
}
