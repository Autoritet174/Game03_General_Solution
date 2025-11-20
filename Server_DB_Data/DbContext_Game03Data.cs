using Microsoft.EntityFrameworkCore;
using Server_Common;
using Server_DB_Data.Configurations;
using Server_DB_Data.Entities;

namespace Server_DB_Data;

/// <summary>
/// Контекст базы данных для работы с игровыми данными.
/// </summary>
/// <remarks>
/// Конструктор, необходимый для Dependency Injection (DI) в ASP.NET Core.
/// Строка подключения и опции передаются через DI из Program.cs.
/// </remarks>
/// <param name="options">Настройки контекста, сформированные в Program.cs.</param>
public class DbContext_Game03Data(DbContextOptions<DbContext_Game03Data> options) : DbContext(options)
{
    /// <summary>
    /// Статический метод для проверки подключения к базе данных.
    /// Строка подключения передается из Program.cs.
    /// </summary>
    /// <param name="connectionString">Строка подключения, которую необходимо проверить.</param>
    /// <exception cref="Exception">Генерирует исключение в случае ошибки подключения.</exception>
    public static async Task ThrowIfFailureConnection(string connectionString)
    {
        try
        {
            // Для статической проверки необходимо создать опции вручную, 
            // используя переданную строку подключения.
            DbContextOptionsBuilder<DbContext_Game03Data> optionsBuilder = new();
            DbContextOptions<DbContext_Game03Data> options = optionsBuilder.UseNpgsql(connectionString).Options;

            // Создаем экземпляр DbContext, используя полученные опции
            using DbContext_Game03Data db = new(options);

            // Выполняем простое чтение для проверки соединения
            _ = await db.Heroes.FirstOrDefaultAsync();
        }
        catch
        {
            System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(DbContext_Game03Data)}, connectionString={connectionString}\r\n\r\n");
            throw;
        }
    }

    /// <summary>
    /// Данные героев.
    /// </summary>
    public DbSet<Hero> Heroes { get; set; }

    /// <summary>
    /// Типы существ.
    /// </summary>
    public DbSet<CreatureType> CreatureTypes { get; set; }

    /// <summary>
    /// Таблица связи многие ко мноким между Heroes и CreatureTypes.
    /// </summary>
    public DbSet<HeroCreatureType> HeroCreatureType { get; set; }

    /// <summary>
    /// Конфигурация модели данных.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new HeroesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CreatureTypesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new X_HeroCreatureTypeConfiguration());

        modelBuilder.ModelToSnakeCase();
    }
}
