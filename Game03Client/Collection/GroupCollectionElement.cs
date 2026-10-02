using System.Collections.Generic;

namespace Game03Client.Collection;

/// <summary>
/// Группа элемента коллекции.
/// </summary>
public class GroupCollectionElement
{
    /// <summary>
    /// Возвращает имя группы элемента коллекции.
    /// </summary>
    public required string name { get; init; }

    /// <summary>
    /// Получает коллекцию, содержащихся в этом экземпляре.
    /// </summary>
    public required IEnumerable<CollectionElement> list { get; init; }

    /// <summary>
    /// Возвращает или устанавливает уровень приоритета группы.
    /// </summary>
    public int priority { get; set; } = 0;
}
