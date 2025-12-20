using Game03Client.DTO;
using General.DTO;
using System.Collections.Generic;

namespace Game03Client.GameData;

/// <summary>
/// Класс для кэширования глобальных данных, используемых поставщиком функций.
/// </summary>
internal class GameDataCache
{
    internal IEnumerable<DtoBaseHero> BaseHeroes = [];
    internal IEnumerable<DtoBaseEquipment> BaseEquipments = [];
    internal IEnumerable<DtoSlotType> SlotTypes = [];
    internal IEnumerable<DtoEquipmentType> EquipmentTypes = [];
}
