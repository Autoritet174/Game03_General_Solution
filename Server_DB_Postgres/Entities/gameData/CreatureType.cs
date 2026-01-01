using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип существа. </summary>
[Table("CreatureTypes", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class CreatureType
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Навигационное свойство к Heroes. </summary>
    public ICollection<X_Hero_CreatureType> X_Hero_CreatureType { get; set; } = [];

    ///// <summary>// Герои этого типа существ. Вычисляемое свойство.
    ///// </summary>
    //[NotMapped]
    //public IReadOnlyCollection<Hero> Heroes => X_Hero_CreatureType?.Select(static x => x.Hero).ToList() ?? [];
}
