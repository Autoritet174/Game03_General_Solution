namespace Server_DB_Data.Entities;

/// <summary>
/// Тип существа.
/// </summary>
public class CreatureType
{
    /// <summary>
    /// UUID уникальный идентификатор.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    public required string Name { get; set; }
}
