using General.GameEntities;
using System.Collections.Generic;

namespace Game03Client.PlayerCollection;

/// <summary>
/// Группа героев.
/// </summary>
public class GroupHeroes(string name, IEnumerable<CollectionHero> list)
{
    /// <summary>
    /// Возвращает имя группы героев.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Получает коллекцию героев, содержащихся в этом экземпляре.
    /// </summary>
    public IEnumerable<CollectionHero> List { get; } = list;

    /// <summary>
    /// Возвращает или устанавливает уровень приоритета группы героев.
    /// </summary>
    public int Priority { get; set; } = 0;
}
