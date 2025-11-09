using System.Security.Claims;

namespace Server.Jwt_NS;

public static class ClaimsExtensions
{

    public static Guid? GetGuid(this ClaimsPrincipal user)
    {
        string? id = user.FindFirst("sub")?.Value
                     ?? user.FindFirst("id")?.Value
                     ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        return Guid.TryParse(id, out Guid guid) ? guid : null;
    }
}
