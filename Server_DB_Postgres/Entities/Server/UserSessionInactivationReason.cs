using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Server;

/// <summary> Причины завершения сессии </summary>
[Table(nameof(DbContextGame.UserSessionInactivationReasons), Schema = nameof(Server))]
//[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class UserSessionInactivationReason
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public InactivationReason Code { get; set; }
}

public enum InactivationReason : int { EXPIRED = 1, ROTATION = 2, USER_LOGOUT = 3, MANUAL_REVOCATION = 4, OTHER_DEVICE = 5 }

public static class UserSessionInactivationReasonExtension
{
    public static async Task<int> GetInactivationReasonIdByCode(this DbContextGame dbContext, InactivationReason code)
    {
        return dbContext.UserSessionInactivationReasons.FirstAsync(a => a.Code == code).Id;
    }
}
