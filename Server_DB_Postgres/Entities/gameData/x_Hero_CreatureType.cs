using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_Heroes_CreatureTypes), Schema = nameof(GameData))]
[PrimaryKey(nameof(BaseHeroId), nameof(CreatureTypeId))]
public class X_Hero_CreatureType
{
    /// <summary> Идентификатор <see cref="GameData.BaseHero"/>. </summary>
    [Key, Column(Order = 0)]
    public int BaseHeroId { get; set; }
    /// <summary> Сущность <see cref="GameData.BaseHero"/>. </summary>
    [ForeignKey(nameof(BaseHeroId))]
    public BaseHero BaseHero { get; set; } = null!;

    /// <summary> Идентификатор <see cref="GameData.CreatureType"/>. </summary>
    [Key, Column(Order = 1)]
    public int CreatureTypeId { get; set; }
    /// <summary> Сущность <see cref="GameData.CreatureType"/>. </summary>
    [ForeignKey(nameof(CreatureTypeId))]
    public CreatureType CreatureType { get; set; } = null!;
}
