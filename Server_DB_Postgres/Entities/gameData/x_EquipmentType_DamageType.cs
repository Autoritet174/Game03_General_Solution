using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table(nameof(x_EquipmentType_DamageType), Schema = nameof(gameData))]
[PrimaryKey(nameof(EquipmentTypeId), nameof(DamageTypeId))]
public class x_EquipmentType_DamageType
{
    /// <summary>
    /// Идентификатор <see cref="__Lists.EquipmentType"/>.
    /// </summary>
    [Key, Column(Order = 0)]
    public required int EquipmentTypeId { get; set; }

    /// <summary>
    /// Идентификатор <see cref="GameData__Lists.DamageType"/>.
    /// </summary>
    [Key, Column(Order = 1)]
    public required int DamageTypeId { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.EquipmentType"/>.
    /// </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public required EquipmentType EquipmentType { get; set; }

    /// <summary>
    /// Сущность <see cref="GameData__Lists.DamageType"/>.
    /// </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }

    /// <summary>
    /// Коэффициент типа урона у оружия.
    /// </summary>
    [Column(name: nameof(DamageCoef)), HasDefaultValue(0)]
    public required int DamageCoef { get; set; }
}
