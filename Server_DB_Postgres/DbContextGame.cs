using General.DTO;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using General.DTO.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using System.Collections.Concurrent;

namespace Server_DB_Postgres;

/// <summary> Контекст базы данных для работы с игровыми данными. </summary>
public class DbContextGame(DbContextOptions<DbContextGame> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    #region collection

    public DbSet<Equipment> equipments { get; set; }
    public DbSet<Hero> heroes { get; set; }
    public DbSet<DropRate> dropRates { get; set; }

    #endregion  collection

    #region gameData

    /// <summary> Способности героев и нпс. </summary>
    public DbSet<Ability> abilities { get; set; }

    /// <summary> Экипировка, болванки. </summary>
    public DbSet<BaseEquipment> baseEquipments { get; set; }

    /// <summary> Данные героев. </summary>
    public DbSet<BaseHero> baseHeroes { get; set; }

    /// <summary> Не игровые персонажи. </summary>
    public DbSet<BaseHero> baseHeroNpcs { get; set; }

    public DbSet<Battlefield> battlefields { get; set; }

    /// <summary> Типы существ. </summary>
    public DbSet<CreatureType> creatureTypes { get; set; }

    /// <summary> Типы урона. </summary>
    public DbSet<DamageType> damageTypes { get; set; }

    /// <summary> Типы экипировки. </summary>
    public DbSet<EquipmentType> equipmentTypes { get; set; }

    /// <summary> Дополнительный процентный урон от материала. </summary>
    public DbSet<MaterialDamagePercent> materialDamagePercents { get; set; }

    /// <summary> Типы слотов экипировки. </summary>
    public DbSet<Slot> slots { get; set; }
    public DbSet<SlotType> slotTypes { get; set; }

    /// <summary> Материалы для кузнечного дела. </summary>
    public DbSet<SmithingMaterial> smithingMaterials { get; set; }

    /// <summary> Таблица связи многие ко мноким между Heroes и CreatureTypes. </summary>
    public DbSet<X_Hero_CreatureType> x_Heroes_CreatureTypes { get; set; }

    /// <summary> Таблица связи многие ко мноким между WeaponTypes и DamageTypes. </summary>
    public DbSet<X_EquipmentType_DamageType> x_EquipmentTypes_DamageTypes { get; set; }
    public DbSet<X_Battlefield_BaseHero> x_Battlefields_BaseHeroes { get; set; }
    public DbSet<X_BaseHero_Ability> x_BaseHeroes_Abilities { get; set; }

    #endregion gameData

    #region logs

    /// <summary> Лог авторизации пользователей. </summary>
    public DbSet<AuthenticationLog> authenticationLogs { get; set; }
    public DbSet<RegistrationLog> registrationLogs { get; set; }

    #endregion logs

    #region server

    /// <summary> Причины бана пользователей. </summary>
    public DbSet<UserBanReason> userBanReasons { get; set; }
    public DbSet<UserSessionInactivationReason> userSessionInactivationReasons { get; set; }

    #endregion server

    #region users
    public DbSet<UserBan> userBans { get; set; }
    public DbSet<UserDevice> userDevices { get; set; }
    public DbSet<UserSession> userSessions { get; set; }
    public DbSet<UserAccesskey> userAccesskeys { get; set; }
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
        _ = modelBuilder.Ignore<Dice>();

        _ = modelBuilder.Ignore<IdentityPasskeyData>();

        modelBuilder.AddConcurrencyTokenToVersion();
        modelBuilder.ApplyDefaultValues();

        DbContextGameConfig.ConfigureAll(modelBuilder);

        // Глобальное отключение каскадного удаления для всех сущностей
        IEnumerable<IMutableForeignKey> foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(static e => e.GetForeignKeys());
        foreach (IMutableForeignKey? foreignKey in foreignKeys)
        {
            // Устанавливаем Restrict, чтобы предотвратить каскадное удаление на уровне БД.
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        DbContextGameConfig.ConfigureAll2(modelBuilder);

        // Переопределение имен таблиц Identity
        string shema = nameof(Entities.Users).ToSnakeCase();
        _ = modelBuilder.Entity<User>().ToTable("identity_users", shema);
        _ = modelBuilder.Entity<IdentityRole<Guid>>().ToTable("identity_roles", shema);
        _ = modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("identity_user_roles", shema);
        _ = modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("identity_user_claims", shema);
        _ = modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("identity_role_claims", shema);
        _ = modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("identity_user_logins", shema);
        _ = modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("identity_user_tokens", shema);

        modelBuilder.CorrectNames();


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
        return await base.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    private void OnSave()
    {
        OnSaveCorrectVersion();
        OnSaveSetTimestamp();
    }


    private static readonly ConcurrentDictionary<IEntityType, IProperty?> versionPropertyCache = new();
    private static readonly ConcurrentDictionary<Type, bool> interfaceCreatedAtCache = new();
    private static readonly ConcurrentDictionary<Type, bool> interfaceUpdatedAtCache = new();



    private void OnSaveCorrectVersion()
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(static e => e.State == EntityState.Modified);
        foreach (EntityEntry entry in entries)
        {
            IProperty? versionProp = versionPropertyCache.GetOrAdd(entry.Metadata, static type =>
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
            bool hasCreatedAt = interfaceCreatedAtCache.GetOrAdd(entityType, type => typeof(ICreatedAt).IsAssignableFrom(type));
            bool hasUpdatedAt = interfaceUpdatedAtCache.GetOrAdd(entityType, type => typeof(IUpdatedAt).IsAssignableFrom(type));

            if (hasCreatedAt)
            {
                IProperty? prop = entry.Metadata.FindProperty(nameof(ICreatedAt.createdAt));
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
                IProperty? prop = entry.Metadata.FindProperty(nameof(IUpdatedAt.updatedAt));
                if (prop != null)
                {
                    entry.CurrentValues[prop] = utcNow;
                }
            }
        }
    }
}
