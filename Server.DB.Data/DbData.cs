using Microsoft.EntityFrameworkCore;
using System.Drawing;
namespace Server.DB.Data;
public class DbData : DbContext_Game03Data
{
    public static string GetConnectionString()
    {
        return "Host=127.127.126.5;Port=5432;Database=Game03_Data;Username=postgres;Password=";
    }
    public static string GetStateConnection()
    {
        try
        {
            DbData db = new();
            _ = db.Heroes.FirstOrDefault();
            return Common.Console.ColorizeText("SUCCESS", Color.Black, Color.LightGreen);
        }
        catch (Exception ex)
        {
            return Common.Console.ColorizeText($"FAILURE [{ex.Message}]", Color.Black, Color.OrangeRed);
        }
    }

    private static readonly DbContextOptionsBuilder<DbContext_Game03Data> options = new();
#pragma warning disable CS8618
    private static DbContextOptions<DbContext_Game03Data> options_Options;
#pragma warning restore CS8618
    public static void Init()
    {
        _ = options.UseNpgsql(GetConnectionString());
        options_Options = options.Options;
    }
    //public DbData(DbContextOptions<DbContext_Game03Data> options) : base(options)
    //{
    //}

    public DbData() : base(options_Options)
    {
    }

}
