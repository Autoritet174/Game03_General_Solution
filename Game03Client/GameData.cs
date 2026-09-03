using General.DTO.Entities;
using General.DTO.Entities.GameData;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client;

public static class GameData
{
    public static DtoContainerGameData Container = null!;
    private static readonly Dictionary<int, BaseHero> DictonaryBaseHero = [];

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
        DtoContainerGameData? c = JSON.Deserialize<DtoContainerGameData>(response);
        if (c == null)
        {
            return false;
        }

        foreach (BaseEquipment i in c.baseEquipments)
        {
            i.equipmentType = c.equipmentTypes.FirstOrDefault(a => a.id == i.equipmentTypeId);
        }

        foreach (EquipmentType i in c.equipmentTypes)
        {
            i.slotType = c.slotTypes.FirstOrDefault(a => a.id == i.slotTypeId);
        }

        foreach (MaterialDamagePercent i in c.materialDamagePercents)
        {
            i.smithingMaterials = c.smithingMaterials.FirstOrDefault(a => a.id == i.smithingMaterialsId);
            i.damageType = c.damageTypes.FirstOrDefault(a => a.id == i.damageTypeId);
        }

        foreach (Slot i in c.Slots)
        {
            i.slotType = c.slotTypes.FirstOrDefault(a => a.id == i.slotTypeId);
        }

        Container = c;

        DictonaryBaseHero.Clear();
        foreach (BaseHero i in c.baseHeroes)
        {
            DictonaryBaseHero.Add(i.id, i);
        }


        return true;
    }

    public static BaseHero? GetBaseHeroById(int id)
    {
        return DictonaryBaseHero.TryGetValue(id, out BaseHero baseHero) ? baseHero : null;
    }
}
