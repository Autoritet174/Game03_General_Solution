using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.SmithingMaterials), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class SmithingMaterial
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; init; }

    /// <summary> Наименование на русском. </summary>
    [MaxLength(256)]
    public string? NameRu { get; init; }
}
