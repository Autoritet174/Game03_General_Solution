using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Collection;

/// <summary> Герой в коллекции пользователя. </summary>
[Table("Equipments", Schema = nameof(Collection))]
public class Equipment : IVersion, ICreatedAt, IUpdatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Уникальный идентификатор владельца. </summary>
    public Guid UserId { get; set; }
    /// <summary> Сущность <see cref="Users.User"/>. </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary> Имя группы. </summary>
    [MaxLength(255)]
    public string? GroupName { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> Идентификатор базовой версии экипировки. </summary>
    public int BaseEquipmentId { get; set; }
    /// <summary> Сущность <see cref="GameData.BaseEquipment"/>. </summary>
    [ForeignKey(nameof(BaseEquipmentId))]
    public BaseEquipment? BaseEquipment { get; set; }
}
