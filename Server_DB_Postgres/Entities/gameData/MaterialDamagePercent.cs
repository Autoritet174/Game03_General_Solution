using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Коэфициенты урона для материалов. </summary>
[Table(nameof(DbContextGame.MaterialDamagePercents), Schema = nameof(GameData))]
public class MaterialDamagePercent
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Возвращает или задает идентификатор <see cref="SmithingMaterial"/>, связанного с данным объектом. </summary>
    public int SmithingMaterialsId { get; set; }

    /// <summary> Навигационное свойство к <see cref="SmithingMaterial"/>. </summary>
    [ForeignKey(nameof(SmithingMaterialsId))]
    public SmithingMaterial SmithingMaterials { get; set; } = null!;


    /// <summary> Возвращает или задает идентификатор <see cref="GameData.DamageType"/>, связанного с данным объектом. </summary>
    public int DamageTypeId { get; set; }

    /// <summary> Навигационное свойство к <see cref="GameData.DamageType"/>. </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public DamageType DamageType { get; set; } = null!;


    /// <summary> Усиление урона оружия в процентах сделанного из этого материала. </summary>
    public int Percent { get; set; }
}
