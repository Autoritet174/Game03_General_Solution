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

    public static async Task LoadGameDataAsync(string jwtToken, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        string response = await HttpRequester.GetResponseAsync(Url.GameData, null, cancellationToken) ?? throw new Exception();
        DtoContainerGameData c = JsonConvert.DeserializeObject<DtoContainerGameData>(response) ?? throw new Exception();

        foreach (DtoBaseEquipment i in c.BaseEquipments)
        {
            i.EquipmentType = c.EquipmentTypes.FirstOrDefault(a => a.Id == i.EquipmentTypeId);
        }

        foreach (DtoEquipmentType i in c.EquipmentTypes)
        {
            i.SlotType = c.SlotTypes.FirstOrDefault(a => a.Id == i.SlotTypeId);
        }
        foreach (DtoMaterialDamagePercent i in c.MaterialDamagePercents)
        {
            i.SmithingMaterial = c.SmithingMaterials.FirstOrDefault(a => a.Id == i.SmithingMaterialId);
            i.DamageType = c.DamageTypes.FirstOrDefault(a => a.Id == i.DamageTypeId);
        }
        foreach (DtoSlot i in c.Slots)
        {
            i.SlotType = c.SlotTypes.FirstOrDefault(a => a.Id == i.SlotTypeId);
        }

        Container = c;
    }

}
