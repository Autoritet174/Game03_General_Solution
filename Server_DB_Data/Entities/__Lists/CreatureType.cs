using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities._Heroes;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Тип существа.
/// </summary>
[Table("CreatureTypes", Schema = nameof(__Lists))]
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
    [MaxLength(255)]
    public required string Name { get; set; }


    /// <summary>
    /// Навигационное свойство к Heroes.
    /// </summary>
    public ICollection<X_Hero_CreatureType> X_Hero_CreatureType { get; set; } = [];

    /// <summary>
    /// Герои этого типа существ. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<Hero> Heroes => X_Hero_CreatureType?.Select(static x => x.Hero).ToList() ?? [];
}
