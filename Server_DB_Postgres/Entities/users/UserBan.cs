using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Server;

namespace Server_DB_Postgres.Entities.Users;

/// <summary> Представляет запись о блокировке (бане) пользователя. </summary>
public class UserBan : IVersion, ICreatedAt, IUpdatedAt
{
    public Guid id { get; init; }

    /// <summary> Идентификатор пользователя, к которому применена блокировка. </summary>
    public required Guid userId { get; set; }
    public User? user { get; set; }


    /// <summary> <inheritdoc/> </summary>
    public long version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset updatedAt { get; set; }

    /// <summary> Дата и время окончания блокировки. Null, если блокировка бессрочная. </summary>
    public DateTimeOffset? expiresAt { get; set; }


    /// <summary> Идентификатор причины блокировки. </summary>
    public int userBanReasonId { get; set; }
    public UserBanReason? userBanReason { get; set; }

}
