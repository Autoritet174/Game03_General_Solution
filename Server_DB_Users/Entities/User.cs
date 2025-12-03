using System.Data.SqlTypes;

namespace Server_DB_Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    public required Guid Id { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? EmailVerifiedAt { get; set; }
    public string? PasswordHash { get; set; }
    public string? TimeZone { get; set; }
    public bool IsAdmin { get; set; } = false;


    /// <summary>
    /// Коллекция банов (1 ко многим)
    /// </summary>
    public ICollection<User_Ban> Bans { get; set; } = [];
}
