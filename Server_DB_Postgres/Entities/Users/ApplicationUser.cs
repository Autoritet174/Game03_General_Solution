using Microsoft.AspNetCore.Identity;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Server_DB_Postgres.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>, IVersion, ICreatedAt, IUpdatedAt
{
    [MaxLength(256)]
    public string? TimeZone { get; set; }

    public ICollection<UserBan> UserBans { get; set; } = [];

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

}
