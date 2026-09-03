using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData
{
    public required IEnumerable<BaseEquipment> baseEquipments { get; set; } = [];
    public required IEnumerable<BaseHero> baseHeroes { get; set; } = [];
    public required IEnumerable<GameData.Battlefield> battlefields { get; set; } = [];
    public required IEnumerable<CreatureType> creatureTypes { get; set; } = [];
    public required IEnumerable<DamageType> damageTypes { get; set; } = [];
    public required IEnumerable<EquipmentType> equipmentTypes { get; set; } = [];
    public required IEnumerable<MaterialDamagePercent> materialDamagePercents { get; set; } = [];
    public required IEnumerable<SlotType> slotTypes { get; set; } = [];
    public required IEnumerable<SmithingMaterial> smithingMaterials { get; set; } = [];
    public required IEnumerable<X_EquipmentType_DamageType> xEquipmentTypesDamageTypes { get; set; } = [];
    public required IEnumerable<X_Hero_CreatureType> xHeroesCreatureTypes { get; set; } = [];
    public required IEnumerable<Slot> Slots { get; set; } = [];
    public required IEnumerable<X_Battlefield_BaseHero> xBattlefieldNpc { get; set; } = [];

}
