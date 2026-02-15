using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Users;
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
        _ = builder.HasIndex(s => s.RefreshTokenHash).IsUnique().HasFilter($"""
            {nameof(UserSession.IsUsed).ToSnakeCase()} = false AND {nameof(UserSession.IsRevoked).ToSnakeCase()} = false
            """);
    }

    private static void Configure(EntityTypeBuilder<Equipment> builder)
    {
        // 1. Уникальный индекс для надетых предметов.
        // Гарантирует, что у героя в конкретном слоте только один предмет.
        _ = builder.HasIndex(e => new { e.HeroId, e.SlotId })
            .IsUnique()
            .HasFilter($"""
            {nameof(Equipment.HeroId).ToSnakeCase()} IS NOT NULL AND {nameof(Equipment.SlotId).ToSnakeCase()} IS NOT NULL
            """);

        // 2. Неуникальный индекс для предметов в инвентаре.
        // Ускоряет получение списка всех предметов игрока, которые НЕ надеты.
        _ = builder.HasIndex(e => new { e.UserId })
            .HasFilter($"""
            {nameof(Equipment.HeroId).ToSnakeCase()} IS NULL
            """);

    }
}
