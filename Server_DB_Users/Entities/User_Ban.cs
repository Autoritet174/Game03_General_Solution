using System.Data.SqlTypes;

namespace Server_DB_Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User_Ban
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public Guid? UserBansReasonsId { get; set; }

    // Навигация на User (многие к одному)
    public User? User { get; set; }
}
