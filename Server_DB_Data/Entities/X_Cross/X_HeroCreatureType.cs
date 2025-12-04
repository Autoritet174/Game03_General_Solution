using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities._Heroes;
using Server_DB_Data.Entities.Directory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.X_Cross;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table("x_hero_creature_type", Schema = "x_cross")]
[Index(nameof(HeroId))]
[Index(nameof(CreatureTypeId))]
[PrimaryKey(nameof(HeroId), nameof(CreatureTypeId))]
public class X_HeroCreatureType
{
    /// <summary>
    /// Идентификатор Hero.
    /// </summary>
    [Key, Column(Order = 0)]
    public int HeroId { get; set; }

    /// <summary>
    /// Идентификатор CreatureType.
    /// </summary>
    [Key, Column(Order = 1)]
    public int CreatureTypeId { get; set; }

    /// <summary>
    /// Сущность Hero.
    /// </summary>
    [Required, ForeignKey(nameof(HeroId))]
    public required Hero Hero { get; set; }

    /// <summary>
    /// Сущность CreatureType.
    /// </summary>
    [Required, ForeignKey(nameof(CreatureTypeId))]
    public required CreatureType CreatureType { get; set; }
}
