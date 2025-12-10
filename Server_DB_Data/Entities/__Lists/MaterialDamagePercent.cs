using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Коэфициенты урона для материалов.
/// </summary>
[Table("MaterialDamagePercent", Schema = nameof(__Lists))]
[Index(nameof(SmithingMaterialsId))]
[Index(nameof(SmithingMaterialsId))]
public class MaterialDamagePercent
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Возвращает или задает идентификатор <see cref="__Lists.SmithingMaterials"/>, связанного с данным объектом.
    /// </summary>
    public required int SmithingMaterialsId { get; set; }

    /// <summary>
    /// Навигационное свойство к <see cref="__Lists.SmithingMaterials"/>.
    /// </summary>
    [ForeignKey(nameof(SmithingMaterialsId))]
    public required SmithingMaterials SmithingMaterials { get; set; }


    /// <summary>
    /// Возвращает или задает идентификатор <see cref="__Lists.DamageType"/>, связанного с данным объектом.
    /// </summary>
    public required int DamageTypeId { get; set; }

    /// <summary>
    /// Навигационное свойство к <see cref="__Lists.DamageType"/>.
    /// </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }




    /// <summary>
    /// Урон оружия в процентах.
    /// </summary>
    [Required]
    public required int Percent { get; set; }
}
