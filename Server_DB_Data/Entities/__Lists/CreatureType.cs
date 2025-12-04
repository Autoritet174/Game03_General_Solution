using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.Directory;

/// <summary>
/// Тип существа.
/// </summary>
[Table("creature_types", Schema = "__lists")]
[Index(nameof(Name), IsUnique = true)]
public class CreatureType
{
    /// <summary>
    /// Первичный ключ. Назначается вручную.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Герои этого типа существа.
    /// </summary>
    public ICollection<X_HeroCreatureType> Heroes { get; set; } = [];
}
