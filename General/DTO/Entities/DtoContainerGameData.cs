using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData(
    IEnumerable<DtoBaseEquipment> baseEquipments,
    IEnumerable<DtoBaseHero> baseHeroes,
    IEnumerable<DtoCreatureType> creatureTypes,
    IEnumerable<DtoDamageType> damageTypes,
    IEnumerable<DtoEquipmentType> equipmentTypes,
    IEnumerable<DtoMaterialDamagePercent> materialDamagePercents,
    IEnumerable<DtoSlotType> slotTypes,
    IEnumerable<DtoSmithingMaterial> smithingMaterials,
    IEnumerable<DtoXEquipmentTypeDamageType> xEquipmentTypesDamageTypes,
    IEnumerable<DtoXHeroCreatureType> xHeroesCreatureTypes,
    IEnumerable<DtoSlot> slots,
    IEnumerable<DtoXBattlefieldNpc> xBattlefieldNpc
    )
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
    public IEnumerable<DtoXBattlefieldNpc> XBattlefieldNpc { get; } = xBattlefieldNpc;

}
