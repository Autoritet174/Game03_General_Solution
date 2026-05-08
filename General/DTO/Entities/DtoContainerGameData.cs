using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData
{
    public IEnumerable<BaseEquipment> BaseEquipments { get; set; } = [];
    public IEnumerable<BaseHero> BaseHeroes { get; set; } = [];
    public IEnumerable<CreatureType> CreatureTypes { get; set; } = [];
    public IEnumerable<DamageType> DamageTypes { get; set; } = [];
    public IEnumerable<EquipmentType> EquipmentTypes { get; set; } = [];
    public IEnumerable<MaterialDamagePercent> MaterialDamagePercents { get; set; } = [];
    public IEnumerable<SlotType> SlotTypes { get; set; } = [];
    public IEnumerable<SmithingMaterial> SmithingMaterials { get; set; } = [];
    public IEnumerable<X_EquipmentType_DamageType> XEquipmentTypesDamageTypes { get; set; } = [];
    public IEnumerable<X_Hero_CreatureType> XHeroesCreatureTypes { get; set; } = [];
    public IEnumerable<Slot> Slots { get; set; } = [];
    public IEnumerable<X_Battlefield_BaseHero> XBattlefieldNpc { get; set; } = [];

}
