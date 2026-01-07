using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

[Table("Slots", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class Slot
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    public int SlotTypeId { get; set; }
    [ForeignKey(nameof(SlotTypeId))]
    public SlotType? SlotType { get; set; }
}
