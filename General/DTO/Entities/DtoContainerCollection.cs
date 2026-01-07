using General.DTO.Entities.Collection;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerCollection(List<DtoEquipment> collectionEquipments, List<DtoHero> collectionHeroes)
{
    public IEnumerable<DtoEquipment> CollectionEquipments { get; set; } = collectionEquipments;
    public IEnumerable<DtoHero> CollectionHeroes { get; set; } = collectionHeroes;

}
