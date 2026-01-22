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
using static General.StringExt;

namespace Server_DB_Postgres;

/// <summary> Контекст базы данных для работы с игровыми данными. </summary>
public class DbContext_Game(DbContextOptions<DbContext_Game> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    #region collection

    /// <summary> Экипировка игрока. </summary>
    public required DbSet<Equipment> Equipments { get; set; }

    /// <summary> Герои игрока. </summary>
    public required DbSet<Hero> Heroes { get; set; }

    #endregion  collection

    #region gameData

    /// <summary> Экипировка, болванки. </summary>
    public required DbSet<BaseEquipment> BaseEquipments { get; set; }

    /// <summary> Данные героев. </summary>
    public required DbSet<BaseHero> BaseHeroes { get; set; }

    /// <summary> Типы существ. </summary>
    public required DbSet<CreatureType> CreatureTypes { get; set; }

    /// <summary> Типы урона. </summary>
    public required DbSet<DamageType> DamageTypes { get; set; }

    /// <summary> Типы экипировки. </summary>
    public required DbSet<EquipmentType> EquipmentTypes { get; set; }

    /// <summary> Дополнительный процентный урон от материала. </summary>
    public required DbSet<MaterialDamagePercent> MaterialDamagePercents { get; set; }

    /// <summary> Типы слотов экипировки. </summary>
    public required DbSet<Slot> Slots { get; set; }
    public required DbSet<SlotType> SlotTypes { get; set; }

    /// <summary> Материалы для кузнечного дела. </summary>
    public required DbSet<SmithingMaterial> SmithingMaterials { get; set; }

    /// <summary> Таблица связи многие ко мноким между Heroes и CreatureTypes. </summary>
    public required DbSet<X_Hero_CreatureType> x_Heroes_CreatureTypes { get; set; }

    /// <summary> Таблица связи многие ко мноким между WeaponTypes и DamageTypes. </summary>
    public required DbSet<X_EquipmentType_DamageType> x_EquipmentTypes_DamageTypes { get; set; }

    #endregion gameData

    #region logs

    /// <summary> Лог авторизации пользователей. </summary>
    public required DbSet<AuthRegLog> AuthRegLogs { get; set; }

    #endregion logs

    #region server

    /// <summary> Причины бана пользователей. </summary>
    public required DbSet<UserBanReason> UserBanReasons { get; set; }

    #endregion server

    #region users
    public required DbSet<UserBan> UserBans { get; set; }
    public required DbSet<UserDevice> UserDevices { get; set; }
    public required DbSet<UserSession> UserSessions { get; set; }
    public required DbSet<UserPasskey> UserPasskeys { get; set; }
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

    /// <summary> Конфигурация модели данных. </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Кастомные ограничения
        //_ = modelBuilder.Entity<BaseEquipment>(static entity =>
        //{
        //    string s1 = nameof(BaseEquipment.SlotType1Id).ToSnakeCase();
        //    string s2 = nameof(BaseEquipment.SlotType2Id).ToSnakeCase();
        //    _ = entity.ToTable(t => t.HasCheckConstraint("CK_BaseEquipment_SlotTypes_Different".ToSnakeCase(), $"""
        //        "{s2}" IS NULL OR "{s2}" <> "{s1}"
        //        """));
        //});



        _ = modelBuilder.Ignore<Microsoft.AspNetCore.Identity.IdentityPasskeyData>();
        // Переопределение имен таблиц Identity
        _ = modelBuilder.Entity<User>().ToTable("identity_users", "users");
        _ = modelBuilder.Entity<IdentityRole<Guid>>().ToTable("identity_roles", "users");
        _ = modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("identity_user_roles", "users");
        _ = modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("identity_user_claims", "users");
        _ = modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("identity_role_claims", "users");
        _ = modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("identity_user_logins", "users");
        _ = modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("identity_user_tokens", "users");

        modelBuilder.AddConcurrencyTokenToVersion();
        modelBuilder.CorrectNames();
        modelBuilder.ApplyDefaultValues();


        // Глобальное отключение каскадного удаления для всех сущностей
        IEnumerable<IMutableForeignKey> foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(static e => e.GetForeignKeys());
        foreach (IMutableForeignKey? foreignKey in foreignKeys)
        {
            // Устанавливаем Restrict, чтобы предотвратить каскадное удаление на уровне БД.
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Переопределение для каскадного удаления только для нужных связей
        // Настройка для UserBan: Cascade при удалении ApplicationUser
        _ = modelBuilder.Entity<UserBan>()
            .HasOne(b => b.User) // Навигационное свойство в UserBan
            .WithMany(u => u.UserBans)      // Коллекция в ApplicationUser
            .HasForeignKey(b => b.UserId) // FK в UserBan
            .OnDelete(DeleteBehavior.Cascade); // Включить каскад: удалять UserBan при удалении ApplicationUser

        // Настройка для UserAuthorization: Cascade при удалении ApplicationUser
        _ = modelBuilder.Entity<AuthRegLog>()
            .HasOne(a => a.User) // Навигационное свойство в UserAuthorization
            .WithMany()                     // Нет коллекции в ApplicationUser (one-to-many без обратной коллекции, если не добавлена)
            .HasForeignKey(a => a.UserId) // FK в UserAuthorization (nullable, но Cascade все равно сработает)
            .OnDelete(DeleteBehavior.Cascade); // Включить каскад: удалять UserAuthorization при удалении ApplicationUser

        _ = modelBuilder.Entity<UserSession>()
            .HasIndex(s => s.TokenHash)
            .IsUnique()
            .HasFilter($"""
            {nameof(UserSession.IsUsed).ToSnakeCase()} = false AND {nameof(UserSession.IsRevoked).ToSnakeCase()} = false
            """);
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
