using General;
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

public static class GameData
{
    public static DtoContainerGameData Container = null!;

    public static async Task<bool> LoadGameDataAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        string? response = await HttpRequester.GetResponseAsync(Url.GAME_DATA, null, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(response))
        {
            return false;
        }
        DtoContainerGameData? c = JsonConvert.DeserializeObject<DtoContainerGameData>(response);
        if (c == null)
        {
            return false;
        }

        foreach (BaseEquipment i in c.BaseEquipments)
        {
            i.EquipmentType = c.EquipmentTypes.FirstOrDefault(a => a.Id == i.EquipmentTypeId);
        }

        foreach (EquipmentType i in c.EquipmentTypes)
        {
            i.SlotType = c.SlotTypes.FirstOrDefault(a => a.Id == i.SlotTypeId);
        }
        foreach (MaterialDamagePercent i in c.MaterialDamagePercents)
        {
            i.SmithingMaterials = c.SmithingMaterials.FirstOrDefault(a => a.Id == i.SmithingMaterialsId);
            i.DamageType = c.DamageTypes.FirstOrDefault(a => a.Id == i.DamageTypeId);
        }
        foreach (Slot i in c.Slots)
        {
            i.SlotType = c.SlotTypes.FirstOrDefault(a => a.Id == i.SlotTypeId);
        }

        Container = c;
        return true;
    }

}
