
using General.DTO.Battlefield;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.BattleField;

public class BattleFieldProvider
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
        catch (OperationCanceledException)
        {
            // отмена операции
        }
        catch (Exception)
        {
            // Обработка ошибок (таймаут, разрыв соединения, ошибка хаба)
            // Логирование и проброс дальше или возврат false
        }

        return null;
    }
}
