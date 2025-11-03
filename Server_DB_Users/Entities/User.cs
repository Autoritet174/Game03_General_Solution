using System.Data.SqlTypes;

namespace Server_DB_Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? EmailVerifiedAt { get; set; }
    public string? PasswordHash { get; set; }
    public string? TimeZone { get; set; }

    // Коллекция банов (1 ко многим)
    public ICollection<User_Ban> Bans { get; set; } = [];
}
