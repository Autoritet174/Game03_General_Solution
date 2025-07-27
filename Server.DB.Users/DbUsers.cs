using Microsoft.EntityFrameworkCore;
using System.Drawing;
namespace Server.DB.Users;
public class DbUsers : DbContext_Game03Users
{
    public static string GetConnectionString()
    {
        return "Host=127.127.126.5;Port=5432;Database=Game03_Users;Username=postgres;Password=";
    }
    public static string GetStateConnection()
    {
        try
        {
            DbUsers db = new();
            _ = db.Users.FirstOrDefault();
            return Common.Console.ColorizeText("SUCCESS", Color.Black, Color.LightGreen);
        }
        catch (Exception ex)
        {
            return Common.Console.ColorizeText($"FAILURE [{ex.Message}]", Color.Black, Color.OrangeRed);
        }
    }

    private static readonly DbContextOptionsBuilder<DbContext_Game03Users> options = new();
#pragma warning disable CS8618
    private static DbContextOptions<DbContext_Game03Users> options_Options;
#pragma warning restore CS8618
    public static void Init()
    {
        _ = options.UseNpgsql(GetConnectionString());
        options_Options = options.Options;
    }
    //public DbUsers(DbContextOptions<DbContext_Game03Users> options) : base(options)
    //{
    //}

    public DbUsers() : base(options_Options)
    {
    }

}
