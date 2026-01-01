using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.Collections.Concurrent;

namespace Server_DB_Postgres;

/// <summary> Контекст базы данных для работы с игровыми данными. </summary>
public class DbContext_Game: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbContext_Game(DbContextOptions<DbContext_Game> options) : base(options)
    {
    }
    //public static DbContextOptions<DbContext_Game> DbContextOptions { get; private set; } = null!;
    //public static NpgsqlDataSource DataSource { get; private set; } = null!;
    //public static void Init(string connectionString)
    //{
    //    JsonSerializerSettings jsonSettings = new()
    //    {
    //        NullValueHandling = NullValueHandling.Ignore // Это исключит null поля из JSON
    //    };
    //    NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);
    //    _ = dataSourceBuilder.UseJsonNet(jsonSettings);
    //    DataSource = dataSourceBuilder.Build();
    //    DbContextOptionsBuilder<DbContext_Game> optionsBuilder = new();
    //    DbContextOptions = optionsBuilder.UseNpgsql(DataSource).Options;
    //}

    ///// <summary> Статический метод для проверки подключения к базе данных. </summary>
    //public static async Task SimpleQueryFromDb()
    //{
    //    try
    //    {
    //        // Создаем экземпляр DbContext, используя полученные опции
    //        using DbContext_Game db = Create();

    //        // Выполняем простое чтение для проверки соединения
    //        _ = await db.Users.FirstOrDefaultAsync();

    //        int action = 3;
    //        switch (action)
    //        {
    //            case 1: AddRandomHeroes(); break;
    //            case 2: AddRandomEquipments(); break;
    //            case 3: await Test(); break;
    //            default: break;
    //        }

    //    }
    //    catch
    //    {
    //        System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(DbContext_Game)}\r\n\r\n");
    //        throw;
    //    }
    //}
    //private static void AddRandomHeroes()
    //{
    //    using DbContext_Game db = Create();
    //    Random rnd = new();
    //    List<Hero> heroes = [];
    //    for (int i = 0; i < 50; i++)
    //    {
    //        Hero hero = new()
    //        {
    //            BaseHeroId = rnd.Next(1, 6),
    //            UserId = GuidAdmin,
    //        };
    //        heroes.Add(hero);
    //    }
    //    db.Heroes.AddRange(heroes);
    //    _ = db.SaveChanges();
    //}
    //private static void AddRandomEquipments()
    //{
    //    using DbContext_Game db = Create();
    //    Random rnd = new();
    //    List<Equipment> equipments = [];
    //    for (int i = 0; i < 50; i++)
    //    {
    //        Equipment equipment = new()
    //        {
    //            BaseEquipmentId = rnd.Next(1, 3),
    //            UserId = GuidAdmin,
    //        };
    //        equipments.Add(equipment);
    //    }
    //    db.Equipments.AddRange(equipments);
    //    _ = db.SaveChanges();
    //}
    //private static async Task Test()
    //{
    //    using DbContext_Game db = Create();
    //var h1 = db.BaseHeroes.First(a => a.Id == 1);

    //foreach (var h in db.BaseHeroes)
    //{
    //    h.Stats ??= new Stats();
    //    h.Stats.Health = h.Health;
    //    h.Stats.Damage = h.Damage;
    //}

    //h1.Health = new Dice(16, 24);
    //h1.Damage = new Dice(4, 21);

    //var h2 = db.BaseHeroes.First(a => a.Id == 2);
    //h2.Health = new Dice(10, 28);
    //h2.Damage = new Dice(5, 21);

    //var h3 = db.BaseHeroes.First(a => a.Id == 3);
    //h3.Health = new Dice(11, 39);
    //h3.Damage = new Dice(3, 25);

    //var h4 = db.BaseHeroes.First(a => a.Id == 4);
    //h4.Health = new Dice(15, 21);
    //h4.Damage = new Dice(4, 23);

    //var h5 = db.BaseHeroes.First(a => a.Id == 5);
    //h5.Health = new Dice(16, 58);
    //h5.Damage = new Dice(4, 21);

    //    _ = db.SaveChanges();
    //}

    ///// <summary> Создаёт новый экземпляр <see cref="DbContext_Game"/> со строкой подключения по умолчанию. </summary>
    ///// <returns> Новый экземпляр <see cref="DbContext_Game"/>, сконфигурированный для работы с базой данных. </returns>
    //public static DbContext_Game Create()
    //{
    //    return new(DbContextOptions);
    //}

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

    ///// <summary> Пользователи. </summary>
    //public DbSet<User> Users { get; set; }

    /// <summary> Баны пользователей. </summary>
    public DbSet<UserBan> UserBans { get; set; }

    /// <summary> Устройства пользователей. </summary>
    public DbSet<UserDevice> UserDevices { get; set; }

    #endregion users

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Host=localhost;Port=5432;Database=Game;Username=postgres;Password=";
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            // Обязательно для Newtonsoft
            _ = dataSourceBuilder.UseJsonNet();

            NpgsqlDataSource dataSource = dataSourceBuilder.Build();
            _ = optionsBuilder.UseNpgsql(dataSource);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Автоматически делаем все свойства типа Dice (или Dice?) колонками jsonb
        //configurationBuilder.Properties<Dice>().HaveColumnType("jsonb");
    }

    /// <summary> Конфигурация модели данных. </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        // В базовом DbContext метод пуст, но вызов является хорошей практикой 
        // на случай смены базового класса (например, на IdentityDbContext).
        base.OnModelCreating(modelBuilder);


        modelBuilder.Ignore<Microsoft.AspNetCore.Identity.IdentityPasskeyData>();
        // Переопределение имен таблиц Identity
        modelBuilder.Entity<ApplicationUser>().ToTable("asp_net_users", "auth");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("asp_net_roles", "auth");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("asp_net_user_roles", "auth");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("asp_net_user_claims", "auth");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("asp_net_role_claims", "auth");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("asp_net_user_logins", "auth");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("asp_net_user_tokens", "auth");


        // Ваши существующие расширения
        modelBuilder.AddConcurrencyTokenToVersion();
        modelBuilder.CorrectNames();
        modelBuilder.ApplyDefaultValues();

        // Глобальное отключение каскадного удаления для всех сущностей
        var foreignKeys = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(static e => e.GetForeignKeys());

        foreach (var foreignKey in foreignKeys)
        {
            // Устанавливаем Restrict, чтобы предотвратить каскадное удаление на уровне БД.
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        //modelBuilder.Entity<Equipment>().HasQueryFilter(e => e.DeletedAt == null);
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
        OnSaveSetTimestamp();
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

    private void OnSaveSetTimestamp()
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
