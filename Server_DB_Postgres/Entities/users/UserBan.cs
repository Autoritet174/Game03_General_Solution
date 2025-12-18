using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Users;

/// <summary> Представляет запись о блокировке (бане) пользователя. </summary>
[Table("UserBans", Schema = nameof(Users))]
public class UserBan : IVersion, ICreatedAt, IUpdatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();


    /// <summary> Идентификатор пользователя, к которому применена блокировка. </summary>
    public required Guid UserId { get; set; }
    /// <summary> Навигационное свойство к <see cref="Users.User"/>. </summary>
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }


    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> Дата и время окончания блокировки. Null, если блокировка бессрочная. </summary>
    public DateTimeOffset? ExpiresAt { get; set; }


    /// <summary> Идентификатор причины блокировки. </summary>
    public int UserBanReasonId { get; set; }
    /// <summary> Навигационное свойство к <see cref="Server.UserBanReason"/>. </summary>
    [ForeignKey(nameof(UserBanReasonId))]
    public required UserBanReason UserBanReason { get; set; }

}
