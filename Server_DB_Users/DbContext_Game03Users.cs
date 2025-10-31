// DbContext_Game03Users.cs
using Microsoft.EntityFrameworkCore;
using Server_DB_Users.Entities;
using System.Drawing;

namespace Server_DB_Users;

public class DbContext_Game03Users : DbContext
{
    public static string GetConnectionString()
    {
        return "Host=localhost;Port=5432;Database=Game03_Users;Username=postgres;Password=";
    }

    public static string GetStateConnection()
    {
        try
        {
            using DbContext_Game03Users db = new();
            _ = db.Users.FirstOrDefault();
            return Server_Common.Console.ColorizeText("SUCCESS", Color.Black, Color.LightGreen);
        }
        catch (Exception ex)
        {
            return Server_Common.Console.ColorizeText($"FAILURE [{ex.Message}]", Color.Black, Color.OrangeRed);
        }
    }

    public DbContext_Game03Users() : base(CreateOptions()) { }

    public DbContext_Game03Users(DbContextOptions<DbContext_Game03Users> options) : base(options) { }

    private static DbContextOptions<DbContext_Game03Users> CreateOptions()
    {
        DbContextOptionsBuilder<DbContext_Game03Users> optionsBuilder = new();
        return optionsBuilder.UseNpgsql(GetConnectionString()).Options;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Configurations.Users());
        _ = modelBuilder.Entity<User>().HasQueryFilter(a => a.DeletedAt == null);

        //DbContext_Game03Data.ModelToSnakeCase(modelBuilder);
    }
}
