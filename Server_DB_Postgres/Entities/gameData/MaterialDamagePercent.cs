using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Коэфициенты урона для материалов. </summary>
[Table("MaterialDamagePercents", Schema = nameof(GameData))]
public class MaterialDamagePercent
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Возвращает или задает идентификатор <see cref="SmithingMaterial"/>, связанного с данным объектом. </summary>
    public int SmithingMaterialsId { get; set; }

    /// <summary> Навигационное свойство к <see cref="SmithingMaterial"/>. </summary>
    [ForeignKey(nameof(SmithingMaterialsId))]
    public required SmithingMaterial SmithingMaterials { get; set; }


    /// <summary> Возвращает или задает идентификатор <see cref="GameData.DamageType"/>, связанного с данным объектом. </summary>
    public int DamageTypeId { get; set; }

    /// <summary> Навигационное свойство к <see cref="GameData.DamageType"/>. </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }


    /// <summary> Урон оружия в процентах. </summary>
    public int Percent { get; set; }
}
