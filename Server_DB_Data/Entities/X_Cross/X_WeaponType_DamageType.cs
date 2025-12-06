using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.__Lists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.X_Cross;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table(nameof(X_WeaponType_DamageType), Schema = nameof(X_Cross))]
[Index(nameof(WeaponTypeId))]
[Index(nameof(DamageTypeId))]
[PrimaryKey(nameof(WeaponTypeId), nameof(DamageTypeId))]
public class X_WeaponType_DamageType
{
    /// <summary>
    /// Идентификатор <see cref="__Lists.WeaponType"/>.
    /// </summary>
    [Key, Column(Order = 0)]
    public required int WeaponTypeId { get; set; }

    /// <summary>
    /// Идентификатор <see cref="__Lists.DamageType"/>.
    /// </summary>
    [Key, Column(Order = 1)]
    public required int DamageTypeId { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.WeaponType"/>.
    /// </summary>
    [Required, ForeignKey(nameof(WeaponTypeId))]
    public required WeaponType WeaponType { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.DamageType"/>.
    /// </summary>
    [Required, ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }

    /// <summary>
    /// Коэффициент типа урона у оружия.
    /// </summary>
    [Required, Column(name: nameof(DamageCoef))]
    public required int DamageCoef { get; set; } = 0;
}
