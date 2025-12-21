using General;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
namespace General.DTO;

/// <summary>
/// Data Transfer Object. Корневой контейнер для данных.
/// </summary>
public class DtoGameDataContainer(List<DtoBaseHero> BaseHeroes, List<DtoSlotType> SlotTypes)
{
    public List<DtoBaseHero> BaseHeroes { get; } = BaseHeroes;

    public List<DtoSlotType> SlotTypes { get; } = SlotTypes;
}
