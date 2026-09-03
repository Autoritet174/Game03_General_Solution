
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Server;
using Server_DB_Postgres.Entities.Users;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Server_DB_Postgres;

public class DbContextGameConfig
{
    private static readonly string gameData = "GameData";
    private static readonly string collection = "Collection";
    private static readonly string users = "Users";
    private static readonly string server = "Server";
    private static readonly string logs = "Logs";
    public static void ConfigureAll(ModelBuilder modelBuilder)
    {
        // GameData
        Configure(modelBuilder.Entity<BaseHero>());
        Configure(modelBuilder.Entity<BaseEquipment>());
        Configure(modelBuilder.Entity<SlotType>());
        Configure(modelBuilder.Entity<Slot>());
        Configure(modelBuilder.Entity<EquipmentType>());
        Configure(modelBuilder.Entity<MaterialDamagePercent>());
        Configure(modelBuilder.Entity<SmithingMaterial>());
        Configure(modelBuilder.Entity<Battlefield>());
        Configure(modelBuilder.Entity<CreatureType>());
        Configure(modelBuilder.Entity<DamageType>());
        Configure(modelBuilder.Entity<X_Battlefield_BaseHero>());
        Configure(modelBuilder.Entity<X_EquipmentType_DamageType>());
        Configure(modelBuilder.Entity<X_Hero_CreatureType>());

        // Collection
        Configure(modelBuilder.Entity<Hero>());
        Configure(modelBuilder.Entity<Equipment>());
        Configure(modelBuilder.Entity<DropRate>());

        // Users
        Configure(modelBuilder.Entity<User>());
        Configure(modelBuilder.Entity<UserAccesskey>());
        Configure(modelBuilder.Entity<UserBan>());
        Configure(modelBuilder.Entity<UserDevice>());
        Configure(modelBuilder.Entity<UserSession>());

        // Server
        Configure(modelBuilder.Entity<UserBanReason>());
        Configure(modelBuilder.Entity<UserSessionInactivationReason>());

        // Logs
        Configure(modelBuilder.Entity<AuthenticationLog>());
        Configure(modelBuilder.Entity<RegistrationLog>());
    }
    public static void ConfigureAll2(ModelBuilder modelBuilder)
    {
        // Users
        Configure2(modelBuilder.Entity<UserBan>());

        // Logs
        Configure2(modelBuilder.Entity<AuthenticationLog>());
        Configure2(modelBuilder.Entity<RegistrationLog>());
    }

    #region GameData
    private static void Configure(EntityTypeBuilder<BaseHero> builder)
    {
        //_ = builder.Property(static e => e.Rarity).HasDefaultValue(int.Common).HasSentinel(int.Common);
        _ = builder.Property(static e => e.mainStat).HasDefaultValue(EMainStat.universal).HasSentinel(EMainStat.universal);
        builder.ToTable(nameof(DbContextGame.BaseHeroes), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.isUnique).HasDefaultValue(false);
        builder.Property(e => e.isPlayable).HasDefaultValue(true);
        builder.Property(e => e.health).HasColumnType("jsonb");
        builder.Property(e => e.damage).HasColumnType("jsonb");
        builder.Property(e => e.strength).HasColumnType("jsonb");
        builder.Property(e => e.agility).HasColumnType("jsonb");
        builder.Property(e => e.intelligence).HasColumnType("jsonb");
        builder.Property(e => e.critChance).HasColumnType("jsonb");
        builder.Property(e => e.critMultiplier).HasColumnType("jsonb");
        builder.Property(e => e.haste).HasColumnType("jsonb");
        builder.Property(e => e.versality).HasColumnType("jsonb");
        builder.Property(e => e.endurancePhysical).HasColumnType("jsonb");
        builder.Property(e => e.enduranceMagical).HasColumnType("jsonb");
        builder.Property(e => e.initiative).HasColumnType("jsonb");
    }
    private static void Configure(EntityTypeBuilder<BaseEquipment> builder)
    {
        //_ = builder.Property(static e => e.Rarity).HasDefaultValue(int.Common).HasSentinel(int.Common);
        builder.ToTable(nameof(DbContextGame.BaseEquipments), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.isUnique).HasDefaultValue(false);
        builder.HasOne(e => e.equipmentType).WithMany().HasForeignKey(e => e.equipmentTypeId);
        builder.HasOne(e => e.smithingMaterial).WithMany().HasForeignKey(e => e.smithingMaterialId).IsRequired(false);
        builder.Property(e => e.possibleStats).HasColumnType("jsonb");
    }
    private static void Configure(EntityTypeBuilder<SlotType> builder)
    {
        builder.ToTable(nameof(DbContextGame.SlotTypes), gameData);
        builder.HasKey(x => x.id);
        builder.Property(x => x.id).ValueGeneratedNever();
        builder.Property(x => x.name).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.name).IsUnique();
        builder.Property(x => x.nameRu).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable(nameof(DbContextGame.Slots), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.mainSlot).HasDefaultValue(true);
        builder.HasOne(e => e.slotType).WithMany().HasForeignKey(e => e.slotTypeId);
    }
    private static void Configure(EntityTypeBuilder<EquipmentType> builder)
    {
        builder.ToTable(nameof(DbContextGame.EquipmentTypes), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.nameRu).HasMaxLength(256);
        builder.Property(e => e.massPhysical).HasDefaultValue(0);
        builder.Property(e => e.massMagical).HasDefaultValue(0);
        builder.Property(e => e.canCraftSmithing).HasDefaultValue(false);
        builder.Property(e => e.canCraftJewelcrafting).HasDefaultValue(false);
        builder.Property(e => e.spendActionPoints).HasDefaultValue(0);
        builder.HasOne(e => e.slotType).WithMany().HasForeignKey(e => e.slotTypeId);
        builder.Property(e => e.damage).HasColumnType("jsonb");
        builder.Property(e => e.possibleStats).HasColumnType("jsonb");
    }
    private static void Configure(EntityTypeBuilder<MaterialDamagePercent> builder)
    {
        builder.ToTable(nameof(DbContextGame.MaterialDamagePercents), gameData);
        builder.HasOne(e => e.smithingMaterials).WithMany().HasForeignKey(e => e.smithingMaterialsId);
        builder.HasOne(e => e.damageType).WithMany().HasForeignKey(e => e.damageTypeId);
    }
    private static void Configure(EntityTypeBuilder<SmithingMaterial> builder)
    {
        builder.ToTable(nameof(DbContextGame.SmithingMaterials), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.nameRu).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<Battlefield> builder)
    {
        builder.ToTable(nameof(DbContextGame.Battlefields), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.enumName).HasMaxLength(256);
        builder.Property(e => e.maxEnemyCount).HasDefaultValue(12);
        builder.Property(e => e.maxHeroCount).HasDefaultValue(12);
    }
    private static void Configure(EntityTypeBuilder<CreatureType> builder)
    {
        builder.ToTable(nameof(DbContextGame.CreatureTypes), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<DamageType> builder)
    {
        builder.ToTable(nameof(DbContextGame.DamageTypes), gameData);
        builder.HasIndex(e => e.name).IsUnique();
        builder.Property(e => e.name).HasMaxLength(256);
        builder.Property(e => e.nameRu).HasMaxLength(256);
        builder.Property(e => e.devHintRu).HasColumnType("text");
        builder.Property(e => e.category).HasDefaultValue(0);
    }
    private static void Configure(EntityTypeBuilder<X_Battlefield_BaseHero> builder)
    {
        builder.ToTable(nameof(DbContextGame.x_Battlefields_BaseHeroes), gameData);
        builder.HasOne(e => e.battlefield).WithMany().HasForeignKey(e => e.battlefieldId);
        builder.HasOne(e => e.baseHero).WithMany().HasForeignKey(e => e.baseHeroId);
        builder.Property(e => e.count).HasDefaultValue(1);
    }
    private static void Configure(EntityTypeBuilder<X_EquipmentType_DamageType> builder)
    {
        builder.ToTable(nameof(DbContextGame.x_EquipmentTypes_DamageTypes), gameData);
        builder.HasOne(e => e.equipmentType).WithMany().HasForeignKey(e => e.equipmentTypeId);
        builder.HasOne(e => e.damageType).WithMany().HasForeignKey(e => e.damageTypeId);
        builder.Property(e => e.damageCoef).HasDefaultValue(0);
    }
    private static void Configure(EntityTypeBuilder<X_Hero_CreatureType> builder)
    {
        builder.ToTable(nameof(DbContextGame.x_Heroes_CreatureTypes), gameData);
        builder.HasOne(e => e.baseHero).WithMany().HasForeignKey(e => e.baseHeroId);
        builder.HasOne(e => e.creatureType).WithMany().HasForeignKey(e => e.creatureTypeId);
    }
    #endregion
    #region Collection
    private static void Configure(EntityTypeBuilder<Hero> builder)
    {
        builder.ToTable(nameof(DbContextGame.Heroes), collection);

        //builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);

        // создание внешнего ключа в базе postgres без навигационного свойства в EF
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.userId);

        builder.HasIndex(e => e.userId);
        builder.HasOne(e => e.baseHero).WithMany().HasForeignKey(e => e.baseHeroId);
        builder.Property(e => e.groupName).HasMaxLength(256);
        builder.Property(e => e.level).HasDefaultValue(1);
        builder.Property(e => e.experience).HasDefaultValue(0);
    }
    private static void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable(nameof(DbContextGame.Equipments), collection);
        builder.HasIndex(e => new { e.heroId, e.slotId });

        //builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);

        // создание внешнего ключа в базе postgres без навигационного свойства в EF
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.userId);

        builder.Property(e => e.groupName).HasMaxLength(256);
        builder.HasOne(e => e.baseEquipment).WithMany().HasForeignKey(e => e.baseEquipmentId);
        builder.HasOne<Hero>().WithMany().HasForeignKey(e => e.heroId).IsRequired(false);
        builder.HasOne<Slot>().WithMany().HasForeignKey(e => e.slotId).IsRequired(false);
        builder.Property(e => e.level).HasDefaultValue(1);
        builder.Property(e => e.stats).HasColumnType("jsonb");


        //вручную делаем индексы так как EF не сделает автоматически изза следующих индексов
        _ = builder.HasIndex(static e => e.heroId);
        _ = builder.HasIndex(static e => e.userId);

        // Уникальный индекс для надетых предметов. Гарантирует, что у героя в конкретном слоте только один предмет.
        _ = builder.HasIndex(e => new { e.heroId, e.slotId }).IsUnique()
            .HasFilter($"{nameof(Equipment.heroId).ToSnakeCase()} IS NOT NULL AND {nameof(Equipment.slotId).ToSnakeCase()} IS NOT NULL");


        // Конветрет для статов, чтобы хранить в базе не название enum полей а их числовые значения
        // Создаем ValueComparer для правильного отслеживания изменений
        var valueComparer = new ValueComparer<Dictionary<EStatType, List<float>>>(
            // Сравнение словарей по содержимому
            static (d1, d2) => CompareDictionaries(d1, d2),
            // Хэш-код на основе содержимого
            static d => GetDictionaryHashCode(d),
            // Создание глубокой копии для снимка
            static d => CopyDictionary(d)
        );

        // Настраиваем свойство
        PropertyBuilder<Dictionary<EStatType, List<float>>?> property = builder.Property(static e => e.stats)
            .HasColumnType("jsonb")
            .HasConversion(
                static v => v == null ? null : v.ToDictionary(static kvp => (int)kvp.Key, static kvp => kvp.Value),
                static v => v == null ? null : v.ToDictionary(static kvp => (EStatType)kvp.Key, static kvp => kvp.Value)
            );

        // Устанавливаем ValueComparer через Metadata
        property.Metadata.SetValueComparer(valueComparer);


        /*
            * Тут была попытка сделать частичный индекс на предметы пользователя только неодетые, 
            * но их 95% и при таком сценарии частичный индекс теряет смысл, проще оставить обычный полный индекс по UserId
         
        // Неуникальные индексы для предметов в инвентаре.
        // только неодетые предметы
        //builder.HasIndex(e => e.UserId).HasFilter($"{nameof(Equipment.HeroId).ToSnakeCase()} IS NULL");

        // только одетые предметы
        //builder.HasIndex(e => new { e.UserId, e.HeroId }).HasFilter($"{nameof(Equipment.HeroId).ToSnakeCase()} IS NOT NULL");
        */
    }
    #region Equipment
    /// <summary>
    /// Сравнивает два словаря характеристик.
    /// Оптимизировано за счет исключения деконструкции KeyValuePair и добавления проверок на пустые коллекции.
    /// </summary>
    /// <param name="d1">Первый словарь характеристик.</param>
    /// <param name="d2">Второй словарь характеристик.</param>
    /// <returns>True, если словари идентичны по составу и значениям.</returns>
    private static bool CompareDictionaries(
        Dictionary<EStatType, List<float>>? d1,
        Dictionary<EStatType, List<float>>? d2)
    {
        if (ReferenceEquals(d1, d2))
        {
            return true;
        }

        if (d1 is null || d2 is null)
        {
            return false;
        }

        int count = d1.Count;
        if (count != d2.Count)
        {
            return false;
        }

        if (count == 0)
        {
            return true;
        }

        foreach (KeyValuePair<EStatType, List<float>> entry in d1)
        {
            ref List<float> list2 = ref CollectionsMarshal.GetValueRefOrNullRef(d2, entry.Key);

            if (Unsafe.IsNullRef(ref list2) || !CompareLists(entry.Value, list2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Сравнивает два списка чисел через Span с использованием SIMD-инструкций.
    /// Добавлен быстрый путь для пустых списков и кэширование размера.
    /// </summary>
    private static bool CompareLists(List<float>? list1, List<float>? list2)
    {
        if (ReferenceEquals(list1, list2))
        {
            return true;
        }

        if (list1 is null || list2 is null)
        {
            return false;
        }

        int count = list1.Count;
        if (count != list2.Count)
        {
            return false;
        }

        if (count == 0)
        {
            return true;
        }

        // SequenceEqual для float использует SIMD (Vectorized) на уровне среды выполнения
        return CollectionsMarshal.AsSpan(list1).SequenceEqual(CollectionsMarshal.AsSpan(list2));
    }

    /// <summary>
    /// Вычисляет хэш-код словаря. 
    /// Использование Span позволяет избежать лишних обращений к свойствам списка в цикле.
    /// </summary>
    private static int GetDictionaryHashCode(Dictionary<EStatType, List<float>>? dict)
    {
        if (dict is null || dict.Count == 0)
        {
            return 0;
        }

        int cumulativeHash = 0;

        foreach (KeyValuePair<EStatType, List<float>> entry in dict)
        {
            // Вместо создания объекта HashCode для каждого ключа, 
            // используем XOR напрямую для примитивов
            int entryHash = (int)entry.Key;

            List<float>? list = entry.Value;
            if (list is { Count: > 0 })
            {
                ReadOnlySpan<float> span = CollectionsMarshal.AsSpan(list);
                foreach (float val in span)
                {
                    entryHash = HashCode.Combine(entryHash, val);
                }
            }

            // XOR обеспечивает детерминированность хэша независимо от порядка ключей в словаре
            cumulativeHash ^= entryHash;
        }

        return HashCode.Combine(cumulativeHash);
    }

    /// <summary>
    /// Создает глубокую копию словаря.
    /// Использование конструктора List(IEnumerable) эффективнее для List[T], так как он оптимизирован под ICollection.
    /// </summary>
    private static Dictionary<EStatType, List<float>> CopyDictionary(Dictionary<EStatType, List<float>>? dict)
    {
        if (dict is null || dict.Count == 0)
        {
            return [];
        }

        var copy = new Dictionary<EStatType, List<float>>(dict.Count);

        foreach (KeyValuePair<EStatType, List<float>> entry in dict)
        {
            copy[entry.Key] = entry.Value is null ? [] : [.. entry.Value];
        }

        return copy;
    }
    #endregion Equipment

    private static void Configure(EntityTypeBuilder<DropRate> builder)
    {
        builder.ToTable(nameof(DbContextGame.DropRates), collection);
        builder.HasOne(e => e.user).WithMany().HasForeignKey(e => e.userId);
    }
    #endregion
    #region Users
    private static void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(DbContextGame.Users));
        builder.Property(e => e.TimeZone).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<UserAccesskey> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserAccesskeys), users);
        builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        builder.Property(e => e.DeviceName).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<UserBan> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserBans), users);
        builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        builder.HasOne(e => e.UserBanReason).WithMany().HasForeignKey(e => e.UserBanReasonId);
    }
    private static void Configure2(EntityTypeBuilder<UserBan> builder)
    {
        // Переопределение для каскадного удаления только для нужных связей
        // Настройка для UserBan: Cascade при удалении ApplicationUser
        _ = builder.HasOne(static b => b.User) // Навигационное свойство в UserBan
            .WithMany(static u => u.UserBans)      // Коллекция в ApplicationUser
            .HasForeignKey(static b => b.UserId) // FK в UserBan
            .OnDelete(DeleteBehavior.Cascade); // Включить каскад: удалять UserBan при удалении ApplicationUser
    }
    private static void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserDevices), users);
        builder.Property(e => e.SystemEnvironmentUserName).HasMaxLength(256);
        builder.Property(e => e.DeviceUniqueIdentifier).HasMaxLength(256);
        builder.Property(e => e.DeviceModel).HasMaxLength(256);
        builder.Property(e => e.DeviceType).HasMaxLength(256);
        builder.Property(e => e.OperatingSystem).HasMaxLength(256);
        builder.Property(e => e.ProcessorType).HasMaxLength(256);
        builder.Property(e => e.GraphicsDeviceName).HasMaxLength(256);
        builder.Property(e => e.SystemInfoNpotSupport).HasMaxLength(256);
    }
    private static void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserSessions), users);
        builder.HasOne(e => e.UserSessionInactivationReason).WithMany().HasForeignKey(e => e.UserSessionInactivationReasonId).IsRequired(false);
        builder.HasOne(e => e.UserDevice).WithMany().HasForeignKey(e => e.UserDeviceId);

        //_ = builder.HasQueryFilter(s => !s.IsUsed && !s.IsRevoked);// Глобальный фильтр для выборок - только живые токены

        // Уникальный индекс на живые токены (неиспользованные и неаннулированные)
        _ = builder.HasIndex(static s => s.RefreshTokenHash).IsUnique().HasFilter($"{nameof(UserSession.IsUsed).ToSnakeCase()} = false AND {nameof(UserSession.IsRevoked).ToSnakeCase()} = false");
    }
    #endregion
    #region Server
    private static void Configure(EntityTypeBuilder<UserBanReason> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserBanReasons), server);
        builder.HasIndex(e => e.Name).IsUnique();
    }
    private static void Configure(EntityTypeBuilder<UserSessionInactivationReason> builder)
    {
        builder.ToTable(nameof(DbContextGame.UserSessionInactivationReasons), server);
        builder.HasIndex(e => e.Name).IsUnique();
    }
    #endregion
    #region Logs
    private static void Configure(EntityTypeBuilder<AuthenticationLog> builder)
    {
        builder.ToTable(nameof(DbContextGame.AuthenticationLogs), logs);
        builder.Property(e => e.email).HasMaxLength(256);
        builder.HasOne(e => e.user).WithMany().HasForeignKey(e => e.userId).IsRequired(false);
        builder.HasOne(e => e.userDevice).WithMany().HasForeignKey(e => e.userDeviceId).IsRequired(false);
        builder.HasOne(e => e.userSession).WithMany().HasForeignKey(e => e.userSessionId).IsRequired(false);
    }
    private static void Configure2(EntityTypeBuilder<AuthenticationLog> builder)
    {
        // Настройка для UserAuthorization: Cascade при удалении ApplicationUser
        _ = builder.HasOne(static a => a.user)
            .WithMany()
            .HasForeignKey(static a => a.userId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    private static void Configure(EntityTypeBuilder<RegistrationLog> builder)
    {
        builder.ToTable(nameof(DbContextGame.RegistrationLogs), logs);
        builder.Property(e => e.email).HasMaxLength(256);
        builder.HasOne(e => e.user).WithMany().HasForeignKey(e => e.userId).IsRequired(false);
        builder.HasOne(e => e.userDevice).WithMany().HasForeignKey(e => e.userDeviceId).IsRequired(false);
    }
    private static void Configure2(EntityTypeBuilder<RegistrationLog> builder)
    {
        // Настройка для UserAuthorization: Cascade при удалении ApplicationUser
        _ = builder.HasOne(static a => a.user)
            .WithMany()
            .HasForeignKey(static a => a.userId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    #endregion


}
