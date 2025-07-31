using Microsoft.EntityFrameworkCore;
using Server.Common;
using Server.DB.Data.Configurations;
using Server.DB.Data.Entities;

namespace Server.DB.Data;

/// <summary>
/// Контекст базы данных, соответствующий первой PostgreSQL базе.
/// </summary>
/// <remarks>
/// Создаёт экземпляр контекста с указанными параметрами.
/// </remarks>
public class DbContext_Game03Data(DbContextOptions<DbContext_Game03Data> options) : DbContext(options)
{

    public DbSet<Hero> Heroes { get; set; }
    public DbSet<CreatureType> CreatureTypes { get; set; }

    /// <summary>
    /// Конфигурирует модель базы данных.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new HeroesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CreatureTypesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new HeroCreatureTypeConfiguration());


        // Глобальный фильтр по удалённым записям
        //_ = modelBuilder.Entity<Hero>().ToTable("heroes", "main");//.HasQueryFilter(a => a.DeletedAt == null);

        //_ = modelBuilder.Entity<CreatureType>().ToTable("creature_types", "main");//.HasQueryFilter(a => a.DeletedAt == null);

        // ПРИВЕДЕНИЕ ВСЕХ ИМЕН К СТАНДАРТУ snake_case
        // Для таблиц
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
