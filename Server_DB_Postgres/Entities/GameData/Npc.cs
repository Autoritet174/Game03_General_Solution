using General;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.Npcs), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Npc
{
    public int Id { get; init; }

    [MaxLength(256)]
    public required string Name { get; set; }

    public ERarity Rarity { get; set; }

    public ERank Rank { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    public EMainStat MainStat { get; set; }
}
