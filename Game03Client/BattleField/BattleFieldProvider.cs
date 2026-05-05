using General;
using General.DTO;
using General.DTO.Entities.GameData;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.BattleField;

public class BattleFieldProvider
{
    public static async Task<bool> LoadBattleFieldAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }
        try
        {
            bool success = await WebSocketProvider.InvokeAsync<bool>(
                HubMethodNames.EMethod.COMBAT_START,
                cancellationToken,
                eBattleFiled,
                spawnedHeroesId
            ).ConfigureAwait(false);

            if (success)
            {
                return true;
            }
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

        return false;
    }
}
