using static General.Enums;

namespace Server_DB_Data.Entities;

/// <summary>
/// Тип урона.
/// </summary>
public class TypeDamage
{
    public required short Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Уникальное наименование на русском.
    /// </summary>
    public required string NameRu { get; set; }
}
