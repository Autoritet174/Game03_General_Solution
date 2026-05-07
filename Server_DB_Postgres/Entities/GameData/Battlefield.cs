using General;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.Battlefields), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Battlefield
{
    public EBattleFiled Id { get; init; }

    [MaxLength(256)]
    public required string Name { get; init; }

    //[Default(100)]
    //public int LevelMax { get; init; }

}
