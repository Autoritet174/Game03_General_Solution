using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(X_EquipmentType_DamageType), Schema = nameof(GameData))]
[PrimaryKey(nameof(EquipmentTypeId), nameof(DamageTypeId))]
public class X_EquipmentType_DamageType
{
    /// <summary> Идентификатор <see cref="GameData.EquipmentType"/>. </summary>
    [Key, Column(Order = 0)]
    public required int EquipmentTypeId { get; set; }
    /// <summary> Сущность <see cref="GameData.EquipmentType"/>. </summary>
    [ForeignKey(nameof(EquipmentTypeId))]

    public required EquipmentType EquipmentType { get; set; }
    /// <summary> Идентификатор <see cref="GameData.DamageType"/>. </summary>
    [Key, Column(Order = 1)]
    public required int DamageTypeId { get; set; }
    /// <summary> Сущность <see cref="GameData.DamageType"/>. </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }

    /// <summary> Коэффициент типа урона у оружия. </summary>
    [Column(name: nameof(DamageCoef)), HasDefaultValue(0)]
    public required int DamageCoef { get; set; }
}
