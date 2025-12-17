using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.gameData;
using Server_DB_Postgres.Entities.users;
using System.Reflection;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres;

/// <summary>
/// Контекст базы данных для работы с игровыми данными.
/// </summary>
/// <remarks>
/// Конструктор, необходимый для Dependency Injection (DI) в ASP.NET Core.
/// Строка подключения и опции передаются через DI из Program.cs.
/// </remarks>
/// <param name="options">Настройки контекста, сформированные в Program.cs.</param>
public class DbContext_Game(DbContextOptions<DbContext_Game> options) : DbContext(options)
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
            DbContextOptionsBuilder<DbContext_Game> optionsBuilder = new();
            DbContextOptions<DbContext_Game> options = optionsBuilder.UseNpgsql(connectionString).Options;

            // Создаем экземпляр DbContext, используя полученные опции
            using DbContext_Game db = new(options);

            // Выполняем простое чтение для проверки соединения
            _ = await db.Heroes.FirstOrDefaultAsync();
        }
        catch
        {
            System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(DbContext_Game)}, connectionString={connectionString}\r\n\r\n");
            throw;
        }
    }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="DbContext_Game"/> с указанной строкой подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения к базе данных PostgreSQL.</param>
    /// <returns>
    /// Новый экземпляр <see cref="DbContext_Game"/>, сконфигурированный для работы с указанной базой данных.
    /// </returns>
    public static DbContext_Game Create(string connectionString)
    {
        DbContextOptionsBuilder<DbContext_Game> optionsBuilder = new();
        DbContextOptions<DbContext_Game> options = optionsBuilder.UseNpgsql(connectionString).Options;
        return new(options);
    }

    /// <summary>
    /// Типы существ.
    /// </summary>
    public DbSet<CreatureType> CreatureTypes { get; set; }

    /// <summary>
    /// Типы урона.
    /// </summary>
    public DbSet<DamageType> DamageTypes { get; set; }

    /// <summary>
    /// Материалы для кузнечного дела.
    /// </summary>
    public DbSet<SmithingMaterials> SmithingMaterials { get; set; }

    /// <summary>
    /// Типы оружия.
    /// </summary>
    public DbSet<EquipmentType> WeaponTypes { get; set; }

    /// <summary>
    /// Типы слотов экипировки.
    /// </summary>
    public DbSet<SlotType> SlotTypes { get; set; }

    /// <summary>
    /// Экипировка. оружие.
    /// </summary>
    public DbSet<Weapon> Weapons { get; set; }

    /// <summary>
    /// Данные героев.
    /// </summary>
    public DbSet<Hero> Heroes { get; set; }
    /// <summary>
    /// Данные героев.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Таблица связи многие ко мноким между Heroes и CreatureTypes.
    /// </summary>
    public DbSet<x_Hero_CreatureType> X_Heros_CreatureTypes { get; set; }

    /// <summary>
    /// Таблица связи многие ко мноким между WeaponTypes и DamageTypes.
    /// </summary>
    public DbSet<x_EquipmentType_DamageType> X_WeaponTypes_DamageTypes { get; set; }

    /// <summary>
    /// Конфигурация модели данных.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    //    _ = modelBuilder.ApplyConfiguration(new EquipmentType_Configuration());
    //    _ = modelBuilder.ApplyConfiguration(new HeroConfiguration());
    //    _ = modelBuilder.ApplyConfiguration(new X_Hero_CreatureType_Configuration());
    //    _ = modelBuilder.ApplyConfiguration(new X_EquipmentType_DamageType_Configuration());

        modelBuilder.CorrectNames(skipIfNameEnteredManual: true);
        modelBuilder.FirstLetterToLowerInScheme();
        ApplyDefaultValues(modelBuilder);
        //Data_DamageType.Add(modelBuilder);
        //Data_CreatureType.Add(modelBuilder);
        //Data_Hero.Add(modelBuilder);
    }
    private void ApplyDefaultValues(ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            EntityTypeBuilder entity = modelBuilder.Entity(entityType.ClrType);

            foreach (PropertyInfo property in entityType.ClrType.GetProperties())
            {
                HasDefaultValueAttribute? attr = property.GetCustomAttribute<HasDefaultValueAttribute>();
                if (attr != null)
                {
                    _ = entity.Property(property.Name).HasDefaultValue(attr.Value);
                }
            }
        }
    }
}
