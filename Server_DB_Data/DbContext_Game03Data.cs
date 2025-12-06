using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Server_Common;
using Server_DB_Data.Entities.__Lists;
using Server_DB_Data.Entities._Equipment;
using Server_DB_Data.Entities.X_Cross;
using Server_DB_Data.ModelBuilderData;

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
    /// Создаёт новый экземпляр <see cref="DbContext_Game03Data"/> с указанной строкой подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения к базе данных PostgreSQL.</param>
    /// <returns>
    /// Новый экземпляр <see cref="DbContext_Game03Data"/>, сконфигурированный для работы с указанной базой данных.
    /// </returns>
    public static DbContext_Game03Data Create(string connectionString)
    {
        DbContextOptionsBuilder<DbContext_Game03Data> optionsBuilder = new();
        DbContextOptions<DbContext_Game03Data> options = optionsBuilder.UseNpgsql(connectionString).Options;
        return new(options);
    }

    /// <summary>
    /// Типы существ.
    /// </summary>
    public DbSet<Entities.__Lists.CreatureType> CreatureTypes { get; set; }

    /// <summary>
    /// Типы урона.
    /// </summary>
    public DbSet<Entities.__Lists.DamageType> DamageTypes { get; set; }

    /// <summary>
    /// Типы оружия.
    /// </summary>
    public DbSet<Entities.__Lists.WeaponType> WeaponTypes { get; set; }

    /// <summary>
    /// Экипировка. оружие.
    /// </summary>
    public DbSet<Entities._Equipment.Weapon> Weapons { get; set; }

    /// <summary>
    /// Данные героев.
    /// </summary>
    public DbSet<Entities._Heroes.Hero> Heroes { get; set; }

    /// <summary>
    /// Таблица связи многие ко мноким между Heroes и CreatureTypes.
    /// </summary>
    public DbSet<Entities.X_Cross.X_Hero_CreatureType> X_Heros_CreatureTypes { get; set; }

    /// <summary>
    /// Таблица связи многие ко мноким между WeaponTypes и DamageTypes.
    /// </summary>
    public DbSet<Entities.X_Cross.X_WeaponType_DamageType> X_WeaponTypes_DamageTypes { get; set; }

    /// <summary>
    /// Конфигурация модели данных.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new WeaponType_Configuration());
        _ = modelBuilder.ApplyConfiguration(new WeaponConfiguration());
        _ = modelBuilder.ApplyConfiguration(new X_Hero_CreatureType_Configuration());
        _ = modelBuilder.ApplyConfiguration(new X_WeaponType_DamageType_Configuration());

        modelBuilder.ModelToSnakeCase(skipIfNameEnteredManual: true);
        modelBuilder.FirstLetterToLowerInScheme();

        Data_DamageType.Add(modelBuilder);
        Data_CreatureType.Add(modelBuilder);
        Data_Hero.Add(modelBuilder);
    }
}
