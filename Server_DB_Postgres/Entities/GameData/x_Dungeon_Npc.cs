using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_Dungeons_Npcs), Schema = nameof(GameData))]
[PrimaryKey(nameof(DungeonId), nameof(NpcId))]
public class x_Dungeon_Npc
{
    [Key, Column(Order = 0)]
    public int DungeonId { get; set; }
    [ForeignKey(nameof(DungeonId))]
    public Dungeon Dungeon { get; set; } = null!;

    [Key, Column(Order = 1)]
    public int NpcId { get; set; }
    [ForeignKey(nameof(NpcId))]
    public Npc Npc { get; set; } = null!;
}
