using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_Heroes_CreatureTypes), Schema = nameof(GameData))]
public class X_Hero_CreatureType
{
    public int Id { get; init; }

    public int BaseHeroId { get; set; }
    [ForeignKey(nameof(BaseHeroId))]
    public BaseHero BaseHero { get; set; } = null!;

    public int CreatureTypeId { get; set; }
    [ForeignKey(nameof(CreatureTypeId))]
    public CreatureType CreatureType { get; set; } = null!;
}
