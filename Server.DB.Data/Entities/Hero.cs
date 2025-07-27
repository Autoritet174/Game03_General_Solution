using System.Data.SqlTypes;

namespace Server.DB.Data.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class Hero
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public required string Name { get; set; }
}
