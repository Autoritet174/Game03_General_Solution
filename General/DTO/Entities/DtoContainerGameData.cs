using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData
{
    public required IEnumerable<BaseEquipment> baseEquipments { get; init; }
    public required IEnumerable<BaseHero> baseHeroes { get; init; }
    public required IEnumerable<GameData.Battlefield> battlefields { get; init; }
    public required IEnumerable<CreatureType> creatureTypes { get; init; }
    public required IEnumerable<Ability> abilities { get; init; }
    public required IEnumerable<DamageType> damageTypes { get; init; }
    public required IEnumerable<EquipmentType> equipmentTypes { get; init; }
    public required IEnumerable<MaterialDamagePercent> materialDamagePercents { get; init; }
    public required IEnumerable<SlotType> slotTypes { get; init; }
    public required IEnumerable<SmithingMaterial> smithingMaterials { get; init; }
    public required IEnumerable<X_EquipmentType_DamageType> xEquipmentTypesDamageTypes { get; init; }
    public required IEnumerable<X_Hero_CreatureType> xHeroesCreatureTypes { get; init; }
    public required IEnumerable<Slot> slots { get; init; }
    public required IEnumerable<X_Battlefield_BaseHero> xBattlefieldNpc { get; init; }
    public required IEnumerable<X_BaseHero_Ability> xBaseHeroesAbilities { get; init; }

}
