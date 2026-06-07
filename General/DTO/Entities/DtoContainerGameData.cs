using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData
{
    public required IEnumerable<BaseEquipment> BaseEquipments { get; set; } = [];
    public required IEnumerable<BaseHero> BaseHeroes { get; set; } = [];
    public required IEnumerable<GameData.Battlefield> Battlefields { get; set; } = [];
    public required IEnumerable<CreatureType> CreatureTypes { get; set; } = [];
    public required IEnumerable<DamageType> DamageTypes { get; set; } = [];
    public required IEnumerable<EquipmentType> EquipmentTypes { get; set; } = [];
    public required IEnumerable<MaterialDamagePercent> MaterialDamagePercents { get; set; } = [];
    public required IEnumerable<SlotType> SlotTypes { get; set; } = [];
    public required IEnumerable<SmithingMaterial> SmithingMaterials { get; set; } = [];
    public required IEnumerable<X_EquipmentType_DamageType> XEquipmentTypesDamageTypes { get; set; } = [];
    public required IEnumerable<X_Hero_CreatureType> XHeroesCreatureTypes { get; set; } = [];
    public required IEnumerable<Slot> Slots { get; set; } = [];
    public required IEnumerable<X_Battlefield_BaseHero> XBattlefieldNpc { get; set; } = [];

}
