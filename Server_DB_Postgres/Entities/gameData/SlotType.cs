using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип слота экипировки. </summary>
[Table(nameof(DbContextGame.SlotTypes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class SlotType
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Наименование на русском. </summary>
    [MaxLength(256)]
    public string? NameRu { get; set; }

}
