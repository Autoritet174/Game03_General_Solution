using General.GameEntities;
using System.Collections.Generic;

namespace Game03Client.PlayerCollection;

/// <summary>
/// Группа элемента коллекции.
/// </summary>
public class GroupCollectionElement(string name, IEnumerable<CollectionElement> list)
{
    /// <summary>
    /// Возвращает имя группы элемента коллекции.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Получает коллекцию, содержащихся в этом экземпляре.
    /// </summary>
    public IEnumerable<CollectionElement> List { get; } = list;

    /// <summary>
    /// Возвращает или устанавливает уровень приоритета группы.
    /// </summary>
    public int Priority { get; set; } = 0;
}
