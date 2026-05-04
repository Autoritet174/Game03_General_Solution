using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_EquipmentTypes_DamageTypes), Schema = nameof(GameData))]
public class X_EquipmentType_DamageType
{
    public int Id { get; init; }

    public int EquipmentTypeId { get; set; }
    [ForeignKey(nameof(EquipmentTypeId))]
    public EquipmentType EquipmentType { get; set; } = null!;

    public int DamageTypeId { get; set; }
    [ForeignKey(nameof(DamageTypeId))]
    public DamageType DamageType { get; set; } = null!;

    /// <summary> Коэффициент типа урона у оружия. </summary>
    [Default(0)]
    public int DamageCoef { get; set; }
}
