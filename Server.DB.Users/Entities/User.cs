using System.Data.SqlTypes;

namespace Server.DB.Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public required string Email { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public required string PasswordHash { get; set; }
    public string? TimeZone { get; set; }
}
