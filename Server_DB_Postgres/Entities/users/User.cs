using General.DTO.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Server_DB_Postgres.Entities.Users;

public class User : IdentityUser<Guid>, IVersion, ICreatedAt, IUpdatedAt
{
    public string? timeZone { get; set; }

    public ICollection<UserBan> userBans { get; set; } = [];

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset updatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long version { get; set; }

}
