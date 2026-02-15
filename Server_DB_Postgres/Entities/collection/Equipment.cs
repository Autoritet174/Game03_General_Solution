using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.Collection;

[Table(nameof(DbContextGame.Equipments), Schema = nameof(Collection))]
[Index(nameof(HeroId), nameof(SlotId))]
public class Equipment : IVersion, ICreatedAt, IUpdatedAt
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [MaxLength(256)]
    public string? GroupName { get; set; }

    public long Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public int BaseEquipmentId { get; set; }
    [ForeignKey(nameof(BaseEquipmentId))]
    public BaseEquipment? BaseEquipment { get; set; }

    /// <summary>
    /// Герой на которого экипирован предмет.
    /// </summary>
    public Guid? HeroId { get; set; }
    [ForeignKey(nameof(HeroId))]
    public Hero? Hero { get; set; }

    public int? SlotId { get; set; }
    [ForeignKey(nameof(SlotId))]
    public Slot? Slot { get; set; }
}
