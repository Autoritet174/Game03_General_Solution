using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Users;

[Table("UserSessions", Schema = nameof(Users))]
public class UserSession : ICreatedAt, IUpdatedAt
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    /// <summary>
    /// Токен сессии. Имеет индекс уникальности для живых токенов.
    /// </summary>
    public required byte[] TokenHash { get; set; }

    /// <summary>
    /// Токен использован.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Токен анулирован.
    /// </summary>
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

    public Guid UserDeviceId { get; set; }
    [ForeignKey(nameof(UserDeviceId))]
    public UserDevice? UserDevice { get; set; }
}
