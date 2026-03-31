using General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Users;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Server_DB_Postgres;

public class DbContextGameConfig
{
    public static void ConfigureAll(ModelBuilder modelBuilder)
    {
        Configure(modelBuilder.Entity<UserSession>());
        Configure(modelBuilder.Entity<AuthenticationLog>());
        Configure(modelBuilder.Entity<UserBan>());
        Configure(modelBuilder.Entity<Equipment>());
    }

    private static void Configure(EntityTypeBuilder<UserBan> builder)
    {
        // Переопределение для каскадного удаления только для нужных связей
        // Настройка для UserBan: Cascade при удалении ApplicationUser
        _ = builder.HasOne(b => b.User) // Навигационное свойство в UserBan
            .WithMany(u => u.UserBans)      // Коллекция в ApplicationUser
            .HasForeignKey(b => b.UserId) // FK в UserBan
            .OnDelete(DeleteBehavior.Cascade); // Включить каскад: удалять UserBan при удалении ApplicationUser
    }

    private static void Configure(EntityTypeBuilder<AuthenticationLog> builder)
    {
        // Настройка для UserAuthorization: Cascade при удалении ApplicationUser
        _ = builder.HasOne(a => a.User) // Навигационное свойство в UserAuthorization
            .WithMany()                     // Нет коллекции в ApplicationUser (one-to-many без обратной коллекции, если не добавлена)
            .HasForeignKey(a => a.UserId) // FK в UserAuthorization (nullable, но Cascade все равно сработает)
            .OnDelete(DeleteBehavior.Cascade); // Включить каскад: удалять UserAuthorization при удалении ApplicationUser
    }

    private static void Configure(EntityTypeBuilder<UserSession> builder)
    {
        //_ = builder.HasQueryFilter(s => !s.IsUsed && !s.IsRevoked);// Глобальный фильтр для выборок - только живые токены

        // Уникальный индекс на живые токены (неиспользованные и неаннулированные)
        _ = builder.HasIndex(s => s.RefreshTokenHash).IsUnique().HasFilter($"{nameof(UserSession.IsUsed).ToSnakeCase()} = false AND {nameof(UserSession.IsRevoked).ToSnakeCase()} = false");
    }


    #region Equipment
    private static void Configure(EntityTypeBuilder<Equipment> builder)
    {
        //вручную делаем индексы так как EF не сделает автоматически изза следующих индексов
        _ = builder.HasIndex(e => e.HeroId);
        _ = builder.HasIndex(e => e.UserId);

        // Уникальный индекс для надетых предметов. Гарантирует, что у героя в конкретном слоте только один предмет.
        _ = builder.HasIndex(e => new { e.HeroId, e.SlotId }).IsUnique()
            .HasFilter($"{nameof(Equipment.HeroId).ToSnakeCase()} IS NOT NULL AND {nameof(Equipment.SlotId).ToSnakeCase()} IS NOT NULL");


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
        PropertyBuilder<Dictionary<EStatType, List<float>>?> property = builder.Property(e => e.Stats)
            .HasColumnType("jsonb")
            .HasConversion(
                v => v == null ? null : v.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value),
                v => v == null ? null : v.ToDictionary(kvp => (EStatType)kvp.Key, kvp => kvp.Value)
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
}
