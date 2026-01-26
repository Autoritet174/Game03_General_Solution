using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип существа. </summary>
[Table(nameof(DbContextGame.CreatureTypes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class CreatureType
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    ///// <summary>// Герои этого типа существ. Вычисляемое свойство.
    ///// </summary>
    //[NotMapped]
    //public IReadOnlyCollection<Hero> Heroes => X_Hero_CreatureType?.Select(static x => x.Hero).ToList() ?? [];
}
