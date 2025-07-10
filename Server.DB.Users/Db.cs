using Microsoft.EntityFrameworkCore;
namespace Server.DB.Users;
public class Db : DbContext_Game03Users
{
    private static readonly DbContextOptionsBuilder<DbContext_Game03Users> options = new();
#pragma warning disable CS8618
    private static DbContextOptions<DbContext_Game03Users> options_Options;
#pragma warning restore CS8618
    public static void Init()
    {
        _ = options.UseNpgsql(UtilitiesFunctions.GetConnectionString());
        options_Options = options.Options;
    }
    public Db(DbContextOptions<DbContext_Game03Users> options) : base(options)
    {

    }

    public Db() : base(options_Options)
    {

    }
}
