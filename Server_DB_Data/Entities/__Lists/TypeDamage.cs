using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Тип урона.
/// </summary>
[Table("types_damage", Schema = "__lists")]
[Index(nameof(Name), IsUnique = true)]
public class TypeDamage
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Name { get; set; }
}
