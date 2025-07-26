using Microsoft.EntityFrameworkCore;
using Server.DB.Game.Entities;

namespace Server.DB.Game;

/// <summary>
/// Контекст базы данных, соответствующий первой PostgreSQL базе.
/// </summary>
/// <remarks>
/// Создаёт экземпляр контекста с указанными параметрами.
/// </remarks>
public class DbContext_Game03Game(DbContextOptions<DbContext_Game03Game> options) : DbContext(options)
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
