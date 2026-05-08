using General.DTO.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Server_DB_Postgres.Entities.Users;

public class User : IdentityUser<Guid>, IVersion, ICreatedAt, IUpdatedAt
{
    public string? TimeZone { get; set; }

    public ICollection<UserBan> UserBans { get; set; } = [];

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

}
