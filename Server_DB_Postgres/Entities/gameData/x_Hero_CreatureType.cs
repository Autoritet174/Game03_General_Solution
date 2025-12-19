using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(X_Hero_CreatureType), Schema = nameof(GameData))]
[PrimaryKey(nameof(HeroId), nameof(CreatureTypeId))]
public class X_Hero_CreatureType
{
    /// <summary> Идентификатор <see cref="GameData.BaseHero"/>. </summary>
    [Key, Column(Order = 0)]
    public int HeroId { get; set; }
    /// <summary> Сущность <see cref="GameData.BaseHero"/>. </summary>
    [ForeignKey(nameof(HeroId))]
    public BaseHero? BaseHero { get; set; }

    /// <summary> Идентификатор <see cref="GameData.CreatureType"/>. </summary>
    [Key, Column(Order = 1)]
    public int CreatureTypeId { get; set; }
    /// <summary> Сущность <see cref="CreatureType"/>. </summary>
    [ForeignKey(nameof(CreatureTypeId))]
    public CreatureType? CreatureTypes { get; set; }
}
