using static General.Enums;

namespace Server_DB_Data.Entities;

/// <summary>
/// Базовый класс от которого наследуем все сущности.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// UUID уникальный идентификатор.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Редкость.
    /// </summary>
    public required RarityLevel Rarity { get; set; }

    /// <summary>
    /// Уникальный в игре, выпадет только один раз.
    /// </summary>
    public required bool IsUnique { get; set; }
}
