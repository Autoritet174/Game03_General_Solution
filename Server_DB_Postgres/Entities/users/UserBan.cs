using Server_DB_Postgres.Entities.server;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.users;

/// <summary>
/// Представляет запись о блокировке (бане) пользователя.
/// </summary>
[Table("UserBans", Schema = nameof(users))]
public class UserBan
{
    /// <summary>
    /// Уникальный идентификатор записи о блокировке.
    /// </summary>
    public required Guid Id { get; set; } = Guid.NewGuid();


    /// <summary>
    /// Идентификатор пользователя, к которому применена блокировка.
    /// </summary>
    public required Guid UserId { get; set; }
    /// <summary>
    /// Навигационное свойство к <see cref="users.User"/>.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }


    /// <summary>
    /// Дата и время создания записи о блокировке.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Дата и время окончания блокировки. Null, если блокировка бессрочная.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; set; }


    /// <summary>
    /// Идентификатор причины блокировки.
    /// </summary>
    public int UserBanReasonId { get; set; }
    /// <summary>
    /// Навигационное свойство к <see cref="server.UserBanReason"/>.
    /// </summary>
    [ForeignKey(nameof(UserBanReasonId))]
    public required UserBanReason UserBanReason { get; set; }

}
