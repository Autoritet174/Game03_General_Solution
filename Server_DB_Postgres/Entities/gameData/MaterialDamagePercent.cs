using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Коэфициенты урона для материалов.
/// </summary>
[Table("MaterialDamagePercents", Schema = nameof(gameData))]
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
    public int SmithingMaterialsId { get; set; }

    /// <summary>
    /// Навигационное свойство к <see cref="__Lists.SmithingMaterials"/>.
    /// </summary>
    [ForeignKey(nameof(SmithingMaterialsId))]
    public required SmithingMaterials SmithingMaterials { get; set; }


    /// <summary>
    /// Возвращает или задает идентификатор <see cref="GameData__Lists.DamageType"/>, связанного с данным объектом.
    /// </summary>
    public int DamageTypeId { get; set; }

    /// <summary>
    /// Навигационное свойство к <see cref="GameData__Lists.DamageType"/>.
    /// </summary>
    [ForeignKey(nameof(DamageTypeId))]
    public required DamageType DamageType { get; set; }


    /// <summary>
    /// Урон оружия в процентах.
    /// </summary>
    public int Percent { get; set; }
}
