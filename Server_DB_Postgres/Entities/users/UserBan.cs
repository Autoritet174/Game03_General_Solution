using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Server;

namespace Server_DB_Postgres.Entities.Users;

/// <summary> Представляет запись о блокировке (бане) пользователя. </summary>
public class UserBan : IVersion, ICreatedAt, IUpdatedAt
{
    public Guid Id { get; init; }

    /// <summary> Идентификатор пользователя, к которому применена блокировка. </summary>
    public required Guid UserId { get; set; }
    public User? User { get; set; }


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
    public UserBanReason? UserBanReason { get; set; }

}
