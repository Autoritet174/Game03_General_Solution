using General.DTO.Entities.Collection;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerCollection(List<Equipment> collectionEquipments, List<Hero> collectionHeroes)
{
    public IEnumerable<Equipment> CollectionEquipments { get; set; } = collectionEquipments;
    public IEnumerable<Hero> CollectionHeroes { get; set; } = collectionHeroes;

}
