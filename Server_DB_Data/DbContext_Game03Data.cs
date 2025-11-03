using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Configurations;
using Server_DB_Data.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Server_Common;
using System.Drawing;

namespace Server_DB_Data;

public class DbContext_Game03Data : DbContext
{
    public static string GetConnectionString()
    {
        return "Host=localhost;Port=5432;Database=Game03_Data;Username=postgres;Password=";
    }

    public static string GetStateConnection()
    {
        try
        {
            using DbContext_Game03Data db = new();
            _ = db.Heroes.FirstOrDefault();
            return Server_Common.Console.ColorizeText("SUCCESS", Color.Black, Color.LightGreen);
        }
        catch (Exception ex)
        {
            return Server_Common.Console.ColorizeText($"FAILURE [{ex.Message}]", Color.Black, Color.OrangeRed);
        }
    }

    public DbContext_Game03Data() : base(CreateOptions()) { }

    public DbContext_Game03Data(DbContextOptions<DbContext_Game03Data> options) : base(options) { }

    private static DbContextOptions<DbContext_Game03Data> CreateOptions()
    {
        DbContextOptionsBuilder<DbContext_Game03Data> optionsBuilder = new();
        return optionsBuilder.UseNpgsql(GetConnectionString()).Options;
    }

    public DbSet<Hero> Heroes { get; set; }
    public DbSet<CreatureType> CreatureTypes { get; set; }
    public DbSet<HeroCreatureType> HeroCreatureType { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new HeroesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CreatureTypesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new X_HeroCreatureTypeConfiguration());

        modelBuilder.ModelToSnakeCase();
    }

}
