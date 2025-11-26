using General.GameEntities;
using System.Collections.Generic;

namespace Game03Client.GameData;

/// <summary>
/// Класс для кэширования глобальных данных, используемых поставщиком функций.
/// </summary>
internal class GameDataCache
{
    /// <summary>
    /// Коллекция, хранящая кэшированный список всех базовых сущностей героев.
    /// Инициализируется пустым списком.
    /// </summary>
    internal IEnumerable<HeroBase> _allHeroes = [];
}
