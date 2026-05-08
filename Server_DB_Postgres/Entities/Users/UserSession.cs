using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Server;

namespace Server_DB_Postgres.Entities.Users;

public class UserSession : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }

    /// <summary> Токен сессии. Имеет индекс уникальности для живых токенов. </summary>
    public required byte[] RefreshTokenHash { get; set; }

    /// <summary> Токен использован. </summary>
    public bool IsUsed { get; set; }

    /// <summary> Токен анулирован. </summary>
    public bool IsRevoked { get; set; }

    /// <summary> Когда сессия была фактически завершена (для истории). </summary>
    public DateTimeOffset? InactivatedAt { get; set; }

    /// <summary> Причина деактивации (например: "Rotation", "Logout", "SystemLock") </summary>
    public int? UserSessionInactivationReasonId { get; set; }
    public UserSessionInactivationReason? UserSessionInactivationReason { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    public Guid UserDeviceId { get; set; }
    public UserDevice? UserDevice { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }
}
