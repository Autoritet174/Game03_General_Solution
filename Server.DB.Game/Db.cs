using Microsoft.EntityFrameworkCore;
using Server.DB.Game;
namespace Server.DB.Users;
public class Db : DbContext_Game03Game
{
    private static readonly DbContextOptionsBuilder<DbContext_Game03Game> options = new();
#pragma warning disable CS8618
    private static DbContextOptions<DbContext_Game03Game> options_Options;
#pragma warning restore CS8618
    public static void Init()
    {
        _ = options.UseNpgsql(UtilitiesFunctions.GetConnectionString());
        options_Options = options.Options;
    }
    public Db(DbContextOptions<DbContext_Game03Game> options) : base(options)
    {

    }

    public Db() : base(options_Options)
    {

    }
}
