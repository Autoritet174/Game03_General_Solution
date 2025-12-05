using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities._Heroes;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Тип оружия.
/// </summary>
[Table("WeaponTypes", Schema = nameof(__Lists))]
[Index(nameof(Name), IsUnique = true)]
public class WeaponType
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


    /// <summary>
    /// Навигационное свойство к DamageTypes.
    /// </summary>
    public ICollection<X_WeaponType_DamageType> X_WeaponType_DamageType { get; set; } = [];

    /// <summary>
    /// Типы урона для этого типа оружия. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<DamageType> DamageTypes => X_WeaponType_DamageType?.Select(static x => x.DamageType).ToList() ?? [];
}
