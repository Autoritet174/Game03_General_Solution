using Microsoft.EntityFrameworkCore;
using Server.Common;
using Server.DB.Data.Configurations;
using Server.DB.Data.Entities;
using System.Drawing;

namespace Server.DB.Data;

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
            return Common.Console.ColorizeText("SUCCESS", Color.Black, Color.LightGreen);
        }
        catch (Exception ex)
        {
            return Common.Console.ColorizeText($"FAILURE [{ex.Message}]", Color.Black, Color.OrangeRed);
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
        _ = modelBuilder.ApplyConfiguration(new HeroCreatureTypeConfiguration());

        ModelToSnakeCase(modelBuilder);
    }

    public static void ModelToSnakeCase(ModelBuilder modelBuilder) {
        // ПРИВЕДЕНИЕ ВСЕХ ИМЕН К СТАНДАРТУ snake_case
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            // Таблицы в snake_case
            entity.SetTableName(entity.GetTableName()!.ToSnakeCase());

            // Колонки в snake_case
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            // Первичный ключ: pk_{table}
            Microsoft.EntityFrameworkCore.Metadata.IMutableKey? pk = entity.FindPrimaryKey();
            pk?.SetName($"pk_{entity.GetTableName()}");

            // Индексы: idx_{table}_{columns}
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableIndex index in entity.GetIndexes())
            {
                index.SetDatabaseName($"idx_{entity.GetTableName()}_{string.Join("_", index.Properties.Select(p => p.GetColumnName()))}");
            }

            // Внешние ключи: fk_{table}_{column}
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey fk in entity.GetForeignKeys())
            {
                string principalTable = fk.PrincipalEntityType.GetTableName()!.ToSnakeCase();
                string columnName = fk.Properties[0].GetColumnName().ToSnakeCase();
                fk.SetConstraintName($"fk_{entity.GetTableName()}_{columnName}_{principalTable}");
            }
        }
    }
}
