using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Postgres.Entities.Logs;
using Server_DB_Postgres.Entities.Users;
using System.Reflection.Emit;
namespace Server_DB_Postgres;

public class DbContextGameConfig
{
    public static void ConfigureAll(ModelBuilder modelBuilder)
    {
        Configure(modelBuilder.Entity<UserSession>());
        Configure(modelBuilder.Entity<AuthenticationLog>());
        Configure(modelBuilder.Entity<UserBan>());
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
        _ = builder.HasQueryFilter(s => !s.IsUsed && !s.IsRevoked);// Глобальный фильтр для выборок - только живые токены

        // Уникальный индекс на живые токены (неиспользованные и неаннулированные)
        _ = builder.HasIndex(s => s.RefreshTokenHash).IsUnique().HasFilter($"""
            {nameof(UserSession.IsUsed).ToSnakeCase()} = false AND {nameof(UserSession.IsRevoked).ToSnakeCase()} = false
            """);
    }
}
