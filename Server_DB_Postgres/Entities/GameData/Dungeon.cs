using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.Dungeons), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Dungeon
{
    public int Id { get; init; }

    [MaxLength(256)]
    public required string Name { get; set; }

    [Default(1)]
    public int Level { get; init; }

    public int Difficulty { get; init; }

}
