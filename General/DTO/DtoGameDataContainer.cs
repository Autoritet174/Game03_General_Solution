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
    [JsonProperty("baseHeroes")]
    public List<DtoBaseHero> BaseHeroes { get; } = BaseHeroes;

    [JsonProperty("slotTypes")]
    public List<DtoSlotType> SlotTypes { get; } = SlotTypes;
}
