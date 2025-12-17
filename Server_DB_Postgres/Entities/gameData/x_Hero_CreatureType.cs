using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
[Table(nameof(x_Hero_CreatureType), Schema = nameof(gameData))]
[PrimaryKey(nameof(HeroId), nameof(CreatureTypeId))]
public class x_Hero_CreatureType
{
    /// <summary>
    /// Идентификатор <see cref="_Heroes.Hero"/>.
    /// </summary>
    [Key, Column(Order = 0)]
    public int HeroId { get; set; }

    /// <summary>
    /// Идентификатор <see cref="GameData__Lists.CreatureType"/>.
    /// </summary>
    [Key, Column(Order = 1)]
    public int CreatureTypeId { get; set; }

    /// <summary>
    /// Сущность <see cref="_Heroes.Hero"/>.
    /// </summary>
    [ForeignKey(nameof(HeroId))]
    public required Hero Hero { get; set; }

    /// <summary>
    /// Сущность <see cref="CreatureType"/>.
    /// </summary>
    [ForeignKey(nameof(CreatureTypeId))]
    public required CreatureType CreatureTypes { get; set; }
}
