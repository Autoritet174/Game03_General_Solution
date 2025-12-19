using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.Collections.Concurrent;
using System.Reflection;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres;

/// <summary> Контекст базы данных для работы с игровыми данными. </summary>
/// <remarks>
/// Конструктор, необходимый для Dependency Injection (DI) в ASP.NET Core.
/// Строка подключения и опции передаются через DI из Program.cs.
/// </remarks>
/// <param name="options">Настройки контекста, сформированные в Program.cs.</param>
public class DbContext_Game(DbContextOptions<DbContext_Game> options) : DbContext(options)
{
    private static readonly Guid GuidAdmin = Guid.Parse("113ae534-2310-40e3-a895-f3747ea976ca");

    private static DbContextOptions<DbContext_Game> dbContextOptions = null!;

    /// <summary>
    /// Инициализация.
    /// </summary>
    /// <param name="connectionString"></param>
    public static void Init(string connectionString)
    {
        DbContextOptionsBuilder<DbContext_Game> optionsBuilder = new();
        dbContextOptions = optionsBuilder.UseNpgsql(connectionString).Options;
    }

    /// <summary> Статический метод для проверки подключения к базе данных.
    /// Строка подключения передается из Program.cs. </summary>
    /// <exception cref="Exception">Генерирует исключение в случае ошибки подключения.</exception>
    public static async Task ThrowIfFailureConnection()
    {
        try
        {
            // Создаем экземпляр DbContext, используя полученные опции
            using DbContext_Game db = Create();

            // Выполняем простое чтение для проверки соединения
            _ = await db.Users.FirstOrDefaultAsync();

            int action = 0;
            switch (action)
            {
                case 1: AddRandomHeroes(); break;
                case 2: AddRandomEquipments(); break;
                case 3: await ChangeEquipment(); break;
                default: break;
            }

        }
        catch
        {
            System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(DbContext_Game)}\r\n\r\n");
            throw;
        }
    }
    private static void AddRandomHeroes()
    {
        using DbContext_Game db = Create();
        Random rnd = new();
        List<Hero> heroes = [];
        for (int i = 0; i < 50; i++)
        {
            Hero hero = new()
            {
                BaseHeroId = rnd.Next(1, 6),
                UserId = GuidAdmin,
            };
            heroes.Add(hero);
        }
        db.Heroes.AddRange(heroes);
        _ = db.SaveChanges();
    }
    private static void AddRandomEquipments()
    {
        using DbContext_Game db = Create();
        Random rnd = new();
        List<Equipment> equipments = [];
        for (int i = 0; i < 50; i++)
        {
            Equipment equipment = new()
            {
                BaseEquipmentId = rnd.Next(1, 3),
                UserId = GuidAdmin,
            };
            equipments.Add(equipment);
        }
        db.Equipments.AddRange(equipments);
        _ = db.SaveChanges();
    }
    private static async Task ChangeEquipment()
    {
        var task1 = Task.Run(() =>
        {
            using DbContext_Game db = Create();
            Equipment e = db.Equipments.First(a => a.Id == Guid.Parse("05c7c86b-e9c9-4aed-93d6-aa7ce882001b"));
            e.CreatedAt = DateTimeOffset.Now;
            Thread.Sleep(900);
            _ = db.SaveChanges();
        });
        var task2 = Task.Run(() =>
        {
            using DbContext_Game db = Create();
            Equipment e = db.Equipments.First(a => a.Id == Guid.Parse("05c7c86b-e9c9-4aed-93d6-aa7ce882001b"));
            e.CreatedAt = DateTimeOffset.Now;
            Thread.Sleep(800);
            _ = db.SaveChanges();
        });

    }


    /// <summary> Создаёт новый экземпляр <see cref="DbContext_Game"/> с указанной строкой подключения. </summary>
    /// <param name="connectionString">Строка подключения к базе данных PostgreSQL.</param>
    /// <returns>Новый экземпляр <see cref="DbContext_Game"/>, сконфигурированный для работы с базой данных. </returns>
    public static DbContext_Game Create(string connectionString)
    {
        DbContextOptionsBuilder<DbContext_Game> optionsBuilder = new();
        DbContextOptions<DbContext_Game> options = optionsBuilder.UseNpgsql(connectionString).Options;
        return new(options);
    }

    /// <summary> Создаёт новый экземпляр <see cref="DbContext_Game"/> со строкой подключения по умолчанию. </summary>
    /// <returns> Новый экземпляр <see cref="DbContext_Game"/>, сконфигурированный для работы с базой данных. </returns>
    public static DbContext_Game Create()
    {
        return new(dbContextOptions);
    }

    #region collection

    /// <summary> Экипировка игрока. </summary>
    public DbSet<Equipment> Equipments { get; set; }

    /// <summary> Герои игрока. </summary>
    public DbSet<Hero> Heroes { get; set; }

    #endregion  collection

    #region gameData

    /// <summary> Экипировка, болванки. </summary>
    public DbSet<BaseEquipment> BaseEquipments { get; set; }

    /// <summary> Данные героев. </summary>
    public DbSet<BaseHero> BaseHeroes { get; set; }

    /// <summary> Типы существ. </summary>
    public DbSet<CreatureType> CreatureTypes { get; set; }

    /// <summary> Типы урона. </summary>
    public DbSet<DamageType> DamageTypes { get; set; }

    /// <summary> Типы экипировки. </summary>
    public DbSet<EquipmentType> EquipmentTypes { get; set; }

    /// <summary> Дополнительный процентный урон от материала. </summary>
    public DbSet<MaterialDamagePercent> MaterialDamagePercents { get; set; }

    /// <summary> Типы слотов экипировки. </summary>
    public DbSet<SlotType> SlotTypes { get; set; }

    /// <summary> Материалы для кузнечного дела. </summary>
    public DbSet<SmithingMaterial> SmithingMaterials { get; set; }

    /// <summary> Таблица связи многие ко мноким между Heroes и CreatureTypes. </summary>
    public DbSet<X_Hero_CreatureType> x_Heroes_CreatureTypes { get; set; }

    /// <summary> Таблица связи многие ко мноким между WeaponTypes и DamageTypes. </summary>
    public DbSet<X_EquipmentType_DamageType> x_EquipmentTypes_DamageTypes { get; set; }

    #endregion gameData

    #region logs

    /// <summary> Лог авторизации пользователей. </summary>
    public DbSet<UserAuthorization> UserAuthorizations { get; set; }

    #endregion logs

    #region server

    /// <summary> Причины бана пользователей. </summary>
    public DbSet<UserBanReason> UserBanReasons { get; set; }

    #endregion server

    #region users

    /// <summary> Пользователи. </summary>
    public DbSet<User> Users { get; set; }

    /// <summary> Баны пользователей. </summary>
    public DbSet<UserBan> UserBans { get; set; }

    /// <summary> Устройства пользователей. </summary>
    public DbSet<UserDevice> UserDevices { get; set; }

    #endregion users

    /// <summary> Конфигурация модели данных. </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //    _ = modelBuilder.ApplyConfiguration(new EquipmentType_Configuration());
        //    _ = modelBuilder.ApplyConfiguration(new HeroConfiguration());
        //    _ = modelBuilder.ApplyConfiguration(new X_Hero_CreatureType_Configuration());
        //    _ = modelBuilder.ApplyConfiguration(new X_EquipmentType_DamageType_Configuration());

        modelBuilder.CorrectNames();
        //modelBuilder.FirstLetterToLowerInScheme();
        ApplyDefaultValues(modelBuilder);
        //Data_DamageType.Add(modelBuilder);
        //Data_CreatureType.Add(modelBuilder);
        //Data_Hero.Add(modelBuilder);


        // Добавить тег [ConcurrencyToken] к свойствам "Version"
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            IMutableProperty? versionProperty = entityType.FindProperty("Version");
            if (versionProperty != null && versionProperty.ClrType == typeof(long))
            {
                // Делаем Version ConcurrencyToken для всех сущностей
                versionProperty.IsConcurrencyToken = true;

                // Опционально: устанавливаем значение по умолчанию
                versionProperty.SetDefaultValue(1L);
            }
        }
    }

    private static void ApplyDefaultValues(ModelBuilder modelBuilder)
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

    /// <summary> <inheritdoc/> </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        OnSave();
        return base.SaveChanges();
    }

    /// <summary> <inheritdoc/> </summary>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        OnSave();
        return await base.SaveChangesAsync(ct);
    }

    private void OnSave()
    {
        OnSaveCorrectVersion();
        OnSaveSetTimespamp();
    }

    private static readonly ConcurrentDictionary<IEntityType, IProperty?> _versionPropertyCache = new();
    private static readonly ConcurrentDictionary<Type, bool> _interfaceCreatedAtCache = new();
    private static readonly ConcurrentDictionary<Type, bool> _interfaceUpdatedAtCache = new();

    private void OnSaveCorrectVersion()
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(static e => e.State == EntityState.Modified);
        foreach (EntityEntry entry in entries)
        {
            IProperty? versionProp = _versionPropertyCache.GetOrAdd(entry.Metadata, static type =>
            {
                IProperty? prop = type.FindProperty("Version");
                return prop?.IsConcurrencyToken == true ? prop : null;
            });
            if (versionProp != null)
            {
                object? ob = entry.CurrentValues[versionProp];
                if (ob != null)
                {
                    long current = (long)ob;
                    entry.CurrentValues[versionProp] = current + 1;
                }
            }
        }
    }

    private void OnSaveSetTimespamp()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (EntityEntry? entry in entries)
        {
            Type entityType = entry.Entity.GetType();
            bool hasCreatedAt = _interfaceCreatedAtCache.GetOrAdd(entityType, type => typeof(ICreatedAt).IsAssignableFrom(type));
            bool hasUpdatedAt = _interfaceUpdatedAtCache.GetOrAdd(entityType, type => typeof(IUpdatedAt).IsAssignableFrom(type));

            if (hasCreatedAt)
            {
                IProperty? prop = entry.Metadata.FindProperty(nameof(ICreatedAt.CreatedAt));
                if (prop != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.CurrentValues[prop] = utcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.CurrentValues[prop] = entry.OriginalValues[prop];
                    }
                }
            }

            if (hasUpdatedAt && entry.State is EntityState.Added or EntityState.Modified)
            {
                IProperty? prop = entry.Metadata.FindProperty(nameof(IUpdatedAt.UpdatedAt));
                if (prop != null)
                {
                    entry.CurrentValues[prop] = utcNow;
                }
            }
        }
    }
}
