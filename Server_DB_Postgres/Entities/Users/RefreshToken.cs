using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Users;

[Table("RefreshTokens", Schema = nameof(Users))]
public class RefreshToken : EasyRefreshToken.EFCore.RefreshToken<User, Guid>
{
    
}
