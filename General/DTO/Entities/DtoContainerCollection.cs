using General.DTO.Entities.Collection;
using System.Collections.Generic;

namespace General.DTO.Entities;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoContainerCollection
{
    public required List<Equipment> CollectionEquipments { get; init; }
    public required List<Hero> CollectionHeroes { get; init; }

}
