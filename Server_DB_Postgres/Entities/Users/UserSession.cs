using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Users;

[Table("UserSessions", Schema = nameof(Users))]
public class UserSession : ICreatedAt, IUpdatedAt
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public required byte[] TokenHash { get; set; }

    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Когда сессия была фактически завершена (для истории)
    /// </summary>
    public DateTimeOffset? InactivatedAt { get; set; }

    /// <summary>
    /// Причина деактивации (например: "Rotation", "Logout", "SystemLock")
    /// </summary>
    public string? InactivationReason { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public int UserDeviceId { get; set; }
    [ForeignKey(nameof(UserDeviceId))]
    public UserDevice? UserDevice { get; set; }
}
