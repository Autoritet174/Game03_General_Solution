
using General.DTO.Battlefield;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.Battlefield;

public class BattlefieldProvider
{
    public static async Task<SpawnedBattlefield?> LoadBattleFieldAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            SpawnedBattlefield? spawnedBattlefield = await WebSocketProvider.InvokeAsync<SpawnedBattlefield?>(
                HubMethodNames.EMethod.COMBAT_START,
                cancellationToken,
                eBattleFiled,
                spawnedHeroesId
            ).ConfigureAwait(false);

            return spawnedBattlefield;
        }
        catch (OperationCanceledException) { }
        catch (Exception) { }

        return null;
    }

    public static async Task<bool> CombatBreakAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        try
        {
            bool result = await WebSocketProvider.InvokeAsync<bool>(
                HubMethodNames.EMethod.COMBAT_BREAK,
                cancellationToken
            ).ConfigureAwait(false);

            return result;
        }
        catch (OperationCanceledException) { }
        catch (Exception) { }

        return false;
    }

    public static async Task<bool> UseAbilityAsync(EAbility eAbility, Guid heroSpawnedId, Guid? target, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        try
        {
            bool result = await WebSocketProvider.InvokeAsync<bool>(
                HubMethodNames.EMethod.USE_ABILITY,
                cancellationToken,
                eAbility,
                heroSpawnedId,
                target
            ).ConfigureAwait(false);

            return result;
        }
        catch (OperationCanceledException) { }
        catch (Exception) { }

        return false;
    }


    public static async Task<List<BattlefieldLogRecord>?> GetBattleLogAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            List<BattlefieldLogRecord>? result = await WebSocketProvider.InvokeAsync<List<BattlefieldLogRecord>?>(
                HubMethodNames.EMethod.GET_BATTLE_LOG,
                cancellationToken
            ).ConfigureAwait(false);

            return result;
        }
        catch (OperationCanceledException) { }
        catch (Exception) { }

        return null;
    }
}
