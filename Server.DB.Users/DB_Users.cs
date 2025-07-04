using Microsoft.EntityFrameworkCore;
using Server.DB.Users.Entities;

namespace Server.DB.Users;

/// <summary>
/// Контекст базы данных, соответствующий первой PostgreSQL базе.
/// </summary>
/// <remarks>
/// Создаёт экземпляр контекста с указанными параметрами.
/// </remarks>
public class DB_Users(DbContextOptions<DB_Users> options) : DbContext(options)
{

    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Конфигурирует модель базы данных.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Configurations.Users());


        // Глобальный фильтр по удалённым записям
        _ = modelBuilder.Entity<User>().HasQueryFilter(a => a.DeletedAt == null);

    }
}
