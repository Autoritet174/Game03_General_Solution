using FluentResults;

using General.DTO.Battlefield;
using Microsoft.EntityFrameworkCore;
using Server.Battlefield;
using Server.Cache;
using Server_DB_Postgres;

namespace Server.Hubs;

public class Client(
    Guid userId,
    ILogger<Client> logger,
    IDbContextFactory<DbContextGame> dbContextFactory,
    CacheService cacheService,
    BattlefieldManager? battlefieldManagerOtherPlayer = null
    )
{
    private readonly Collection.EquipmentManager equipmentManager = new(userId, dbContextFactory, logger, cacheService);
    public BattlefieldManager battleFieldManager { get; } = battlefieldManagerOtherPlayer ?? new(userId, dbContextFactory, logger, cacheService);

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

    public bool CombatBreak() => battleFieldManager.CombatBreak();

    public async Task<bool> UseAbilityAsync(EBattlefieldLogAbility eAbility, Guid heroSpawnedId, Guid? target)
    {
        return await battleFieldManager.UseAbilityAsync(eAbility, heroSpawnedId, target).ConfigureAwait(false);
    }

    public List<BattlefieldLogRecordBase> GetBattleLog()
    {
        return battleFieldManager.GetBattleLog();
    }
}
