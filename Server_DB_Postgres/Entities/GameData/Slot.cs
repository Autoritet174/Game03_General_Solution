using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

[Table(nameof(DbContextGame.Slots), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Slot
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    public int SlotTypeId { get; set; }
    [ForeignKey(nameof(SlotTypeId))]
    public SlotType SlotType { get; set; } = null!;

    [HasDefaultValue(true)]
    public bool MainSlot { get; set; }
}
