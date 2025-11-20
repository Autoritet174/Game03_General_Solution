using Microsoft.EntityFrameworkCore;
using Server_Common;
using Server_DB_Users.Entities;

namespace Server_DB_Users;

/// <summary>
/// Контекст базы данных для работы с данными пользователей (PostgreSQL).
/// </summary>
/// <remarks>
/// Конструктор, необходимый для Dependency Injection (DI) в ASP.NET Core.
/// Строка подключения и опции передаются через DI из Program.cs.
/// </remarks>
/// <param name="options">Настройки контекста, сформированные в Program.cs.</param>
public class DbContext_Game03Users(DbContextOptions<DbContext_Game03Users> options) : DbContext(options)
{
    /// <summary>
    /// Набор данных для работы с сущностью пользователя.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Набор данных для работы с сущностью блокировок пользователей.
    /// </summary>
    public DbSet<User_Ban> Users_Bans { get; set; }

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
            DbContextOptionsBuilder<DbContext_Game03Users> optionsBuilder = new();
            DbContextOptions<DbContext_Game03Users> options = optionsBuilder.UseNpgsql(connectionString).Options;

            // Создаем экземпляр DbContext, используя полученные опции
            using DbContext_Game03Users db = new(options);

            // Выполняем простое чтение для проверки соединения
            _ = await db.Users.FirstOrDefaultAsync();
        }
        catch
        {
            System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(DbContext_Game03Users)}, connectionString={connectionString}\r\n\r\n");
            throw;
        }
    }

    /// <summary>
    /// Конфигурация модели данных.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Configurations.Users());
        _ = modelBuilder.ApplyConfiguration(new Configurations.Users_Bans());

        modelBuilder.ModelToSnakeCase();
    }
}
