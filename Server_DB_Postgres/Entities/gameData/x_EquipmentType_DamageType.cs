using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_EquipmentTypes_DamageTypes), Schema = nameof(GameData))]
[PrimaryKey(nameof(EquipmentTypeId), nameof(DamageTypeId))]
public class X_EquipmentType_DamageType
{
    /// <summary> Идентификатор <see cref="GameData.EquipmentType"/>. </summary>
    [Key, Column(Order = 0)]
    public int EquipmentTypeId { get; set; }
    /// <summary> Сущность <see cref="GameData.EquipmentType"/>. </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public EquipmentType EquipmentType { get; set; } = null!;

    /// <summary> Идентификатор <see cref="GameData.DamageType"/>. </summary>
    [Key, Column(Order = 1)]
    public int DamageTypeId { get; set; }
    /// <summary> Сущность <see cref="GameData.DamageType"/>. </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public DamageType DamageType { get; set; } = null!;

    /// <summary> Коэффициент типа урона у оружия. </summary>
    [HasDefaultValue(0)]
    public int DamageCoef { get; set; }
}
