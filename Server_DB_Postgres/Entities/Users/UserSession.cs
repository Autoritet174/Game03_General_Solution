using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Server;

namespace Server_DB_Postgres.Entities.Users;

public class UserSession : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid id { get; init; }
    public Guid userId { get; set; }

    /// <summary> Токен сессии. Имеет индекс уникальности для живых токенов. </summary>
    public required byte[] refreshTokenHash { get; set; }

    /// <summary> Токен использован. </summary>
    public bool isUsed { get; set; }

    /// <summary> Токен анулирован. </summary>
    public bool isRevoked { get; set; }

    /// <summary> Когда сессия была фактически завершена (для истории). </summary>
    public DateTimeOffset? inactivatedAt { get; set; }

    /// <summary> Причина деактивации (например: "Rotation", "Logout", "SystemLock") </summary>
    public int? userSessionInactivationReasonId { get; set; }
    public UserSessionInactivationReason? userSessionInactivationReason { get; set; }

    public DateTimeOffset expiresAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset updatedAt { get; set; }

    public Guid userDeviceId { get; set; }
    public UserDevice? userDevice { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long version { get; set; }
}
