using FluentResults;
using General;
using General.DTO.Battlefield;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server_DB_Postgres;

namespace Server.Hubs;

public class Client(Guid userId, ILogger<Client> logger, IDbContextFactory<DbContextGame> dbContextFactory, CacheService cacheService)
{
    private readonly Collection.EquipmentManager equipmentManager = new(userId, dbContextFactory, logger, cacheService);
    private readonly BattleField.BattleFieldManager battleFieldManager = new(userId, dbContextFactory, logger, cacheService);

    public Guid UserId => userId;


    public async Task<bool> EquipmentTakeOnAsync(Guid heroId, Guid equipmentId, bool? inAltSlot, CancellationToken cancellationToken)
    {
        Result result = await equipmentManager.TakeOnAsync(heroId, equipmentId, inAltSlot, cancellationToken).ConfigureAwait(false);

        if (result.IsSuccess)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("TakeOn успешно: {Result}", result);
            }
            else
            {
                logger.LogError("TakeOn ошибка: {Result}", result);
            }
        }

        return result.IsSuccess;
    }

    public async Task<bool> EquipmentTakeOffAsync(Guid equipmentId, CancellationToken cancellationToken)
    {
        Result result = await equipmentManager.TakeOffAsync(equipmentId, cancellationToken).ConfigureAwait(false);

        if (result.IsSuccess)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("TakeOff успешно: {Result}", result);
            }
            else
            {
                logger.LogError("TakeOff ошибка: {Result}", result);
            }
        }

        return result.IsSuccess;
    }

    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        return await battleFieldManager.CombatStartAsync(eBattleFiled, spawnedHeroesId, cancellationToken).ConfigureAwait(false);
    }
}
