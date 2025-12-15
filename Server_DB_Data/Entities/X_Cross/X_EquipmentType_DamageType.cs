using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.__Lists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_Common.Attributes;

namespace Server_DB_Data.Entities.X_Cross;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table(nameof(X_EquipmentType_DamageType), Schema = nameof(X_Cross))]
[PrimaryKey(nameof(EquipmentTypeId), nameof(DamageTypeId))]
public class X_EquipmentType_DamageType
{
    /// <summary>
    /// Идентификатор <see cref="__Lists.Equip"/>.
    /// </summary>
    [Key, Column(Order = 0)]
    public required int EquipmentTypeId { get; set; }

    /// <summary>
    /// Идентификатор <see cref="__Lists.DamageType"/>.
    /// </summary>
    [Key, Column(Order = 1)]
    public required int DamageTypeId { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.Equip"/>.
    /// </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public required Equip EquipmentType { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.DamageType"/>.
    /// </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }

    /// <summary>
    /// Коэффициент типа урона у оружия.
    /// </summary>
    [Column(name: nameof(DamageCoef)), HasDefaultValue(0)]
    public required int DamageCoef { get; set; }
}
