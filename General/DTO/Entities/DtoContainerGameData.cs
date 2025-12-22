using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerGameData()
{
    public List<DtoBaseEquipment> DtoBaseEquipments { get; set; } = [];
    public List<DtoBaseHero> DtoBaseHeroes { get; set; } = [];
    public List<DtoCreatureType> DtoCreatureTypes { get; set; } = [];
    public List<DtoDamageType> DtoDamageTypes { get; set; } = [];
    public List<DtoEquipmentType> DtoEquipmentTypes { get; set; } = [];
    public List<DtoMaterialDamagePercent> DtoMaterialDamagePercents { get; set; } = [];
    public List<DtoSlotType> DtoSlotTypes { get; set; } = [];
    public List<DtoSmithingMaterial> DtoSmithingMaterials { get; set; } = [];
    public List<DtoXEquipmentTypeDamageType> DtoXEquipmentTypesDamageTypes { get; set; } = [];
    public List<DtoXHeroCreatureType> DtoXHeroesCreatureTypes { get; set; } = [];

}
