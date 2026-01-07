using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData(
    List<DtoBaseEquipment> baseEquipments,
    List<DtoBaseHero> baseHeroes,
    List<DtoCreatureType> creatureTypes,
    List<DtoDamageType> damageTypes,
    List<DtoEquipmentType> equipmentTypes,
    List<DtoMaterialDamagePercent> materialDamagePercents,
    List<DtoSlotType> slotTypes,
    List<DtoSmithingMaterial> smithingMaterials,
    List<DtoXEquipmentTypeDamageType> xEquipmentTypesDamageTypes,
    List<DtoXHeroCreatureType> xHeroesCreatureTypes,
    List<DtoSlot> slots)
{
    public IEnumerable<DtoBaseEquipment> BaseEquipments { get; } = baseEquipments;
    public IEnumerable<DtoBaseHero> BaseHeroes { get; } = baseHeroes;
    public IEnumerable<DtoCreatureType> CreatureTypes { get; } = creatureTypes;
    public IEnumerable<DtoDamageType> DamageTypes { get; } = damageTypes;
    public IEnumerable<DtoEquipmentType> EquipmentTypes { get; } = equipmentTypes;
    public IEnumerable<DtoMaterialDamagePercent> MaterialDamagePercents { get; } = materialDamagePercents;
    public IEnumerable<DtoSlotType> SlotTypes { get; } = slotTypes;
    public IEnumerable<DtoSmithingMaterial> SmithingMaterials { get; } = smithingMaterials;
    public IEnumerable<DtoXEquipmentTypeDamageType> XEquipmentTypesDamageTypes { get; } = xEquipmentTypesDamageTypes;
    public IEnumerable<DtoXHeroCreatureType> XHeroesCreatureTypes { get; } = xHeroesCreatureTypes;
    public IEnumerable<DtoSlot> Slots { get; } = slots;

}
