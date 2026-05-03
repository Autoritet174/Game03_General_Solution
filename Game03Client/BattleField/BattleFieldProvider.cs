using General;
using General.DTO;
using General.DTO.Entities.GameData;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.BattleField;

public class BattleFieldProvider
{
    public static async Task<bool> LoadBattleFieldAsync(EBattleFiled eBattleFiled, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        //var guid = Guid.NewGuid();
        //DtoWSLoadBattleFiledC2S equipmentLoadBattleFiled = new(eBattleFiled, guid);
        //bool result = await WebSocketProvider.SendMessageAsync(equipmentLoadBattleFiled, cancellationToken).ConfigureAwait(false);
        //if (result) {
        //    DtoWSResponseS2C? response = await WebSocketProvider.WaitForResponseAsync(guid, cancellationToken).ConfigureAwait(false);

        //    if (response?.Success == true)
        //    {
        //        //DtoBaseEquipment? baseEquip = equipment.BaseEquipment;
        //        //ESlotType slotTypeId = baseEquip?.EquipmentType?.SlotType?.Id ?? 0;

        //        //// Обновляем локальное состояние
        //        //equipment.HeroId = null;
        //        //equipment.SlotId = null;
        //        //return true;
        //    }
        //}

        return false;
    }
}
