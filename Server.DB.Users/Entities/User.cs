using System.Data.SqlTypes;

namespace Server.DB.Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? EmailVerifiedAt { get; set; }
    public string? PasswordHash { get; set; }
    public string? TimeZone { get; set; }
}
