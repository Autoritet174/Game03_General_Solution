using General.DTO.Entities.Collection;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerCollection()
{
    public List<DtoEquipment> DtoCollectionEquipments { get; set; } = [];
    public List<DtoHero> DtoCollectionHeroes { get; set; } = [];

}
