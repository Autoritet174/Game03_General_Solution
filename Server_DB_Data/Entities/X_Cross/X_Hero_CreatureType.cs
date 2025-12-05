using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.__Lists;
using Server_DB_Data.Entities._Heroes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.X_Cross;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table(nameof(X_Hero_CreatureType), Schema = nameof(X_Cross))]
[Index(nameof(HeroId))]
[Index(nameof(CreatureTypeId))]
[PrimaryKey(nameof(HeroId), nameof(CreatureTypeId))]
public class X_Hero_CreatureType
{
    /// <summary>
    /// Идентификатор <see cref="_Heroes.Hero"/>.
    /// </summary>
    [Key, Column(Order = 0)]
    public int HeroId { get; set; }

    /// <summary>
    /// Идентификатор <see cref="__Lists.CreatureType"/>.
    /// </summary>
    [Key, Column(Order = 1)]
    public int CreatureTypeId { get; set; }

    /// <summary>
    /// Сущность <see cref="_Heroes.Hero"/>.
    /// </summary>
    [Required, ForeignKey(nameof(HeroId))]
    public required Hero Hero { get; set; }

    /// <summary>
    /// Сущность <see cref="__Lists.CreatureType"/>.
    /// </summary>
    [Required, ForeignKey(nameof(CreatureTypeId))]
    public required CreatureType CreatureType { get; set; }
}
