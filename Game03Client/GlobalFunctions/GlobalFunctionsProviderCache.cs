using General.GameEntities;
using System.Collections.Generic;

namespace Game03Client.GlobalFunctions;

/// <summary>
/// Класс для кэширования глобальных данных, используемых поставщиком функций.
/// </summary>
internal class GlobalFunctionsProviderCache
{
    /// <summary>
    /// Коллекция, хранящая кэшированный список всех базовых сущностей героев.
    /// Инициализируется пустым списком.
    /// </summary>
    internal IEnumerable<HeroBaseEntity> _allHeroes = [];
}
