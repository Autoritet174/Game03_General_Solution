using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Тип существа.
/// </summary>
[Table("SmithingMaterials", Schema = nameof(gameData))]
[Index(nameof(Name), IsUnique = true)]
public class SmithingMaterials
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Наименование на русском.
    /// </summary>
    [MaxLength(255)]
    public string? NameRu { get; set; }

    /// <summary>
    /// Навигационное свойство к <see cref="__Lists.MaterialDamagePercent"/>.
    /// </summary>
    public ICollection<MaterialDamagePercent> MaterialDamagePercent { get; set; } = [];
}
