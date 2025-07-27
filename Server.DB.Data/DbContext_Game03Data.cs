using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public DbSet<Hero> Heroes { get; set; }

    /// <summary>
    /// Конфигурирует модель базы данных.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Configurations.Heroes());


        // Глобальный фильтр по удалённым записям
        _ = modelBuilder.Entity<Hero>().HasQueryFilter(a => a.DeletedAt == null);

    }
}
