
using General.DTO.Battlefield;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.Battlefield;

public class BattlefieldProvider
{
    private static readonly Logger<BattlefieldProvider> logger = new();
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
        catch (HubException ex)
        {
            // Ошибка от сервера
            logger.LogError($"Hub error: {ex.Message}");
            throw;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError($"Cancelled: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Детальное логирование
            //logger.LogError($"Connection state: {WebSocketProvider.conn _hubConnection.State}");
            logger.LogError($"Error invoking GetBattleLog: {ex.Message}");
            logger.LogError($"Stack trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                logger.LogError($"Inner exception: {ex.InnerException.Message}");
            }

            throw;
        }

        //return null;
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

    public static async Task<bool> UseAbilityAsync(EBattlefieldLogAbility eAbility, Guid heroSpawnedId, Guid? target, CancellationToken cancellationToken)
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


    public static async Task<List<BattlefieldLogRecordBase>?> GetBattleLogAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            List<BattlefieldLogRecordBase>? result = await WebSocketProvider.InvokeAsync<List<BattlefieldLogRecordBase>?>(
                HubMethodNames.EMethod.GET_BATTLE_LOG,
                cancellationToken
            ).ConfigureAwait(false);
            logger.LogInfo(JSON.Serialize(result));

            return result;
        }
        catch (OperationCanceledException) { }
        catch (Exception) { }

        return null;
    }
}
