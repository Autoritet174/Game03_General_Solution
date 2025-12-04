using static General.Enums;

namespace Server_DB_Data.Entities;

/// <summary>
/// Базовый класс от которого наследуем все сущности.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Редкость.
    /// </summary>
    RarityLevel Rarity { get; set; }

    /// <summary>
    /// Уникальный в игре, выпадет только один раз.
    /// </summary>
    bool IsUnique { get; set; }
}
