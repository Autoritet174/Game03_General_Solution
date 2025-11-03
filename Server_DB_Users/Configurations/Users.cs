using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Users.Entities;

namespace Server_DB_Users.Configurations;
internal class Users : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.ToTable("users", "_main");


        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasKey(e => e.Id);


        //-------------------------------------
        _ = builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();


        //-------------------------------------
        _ = builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(255);

        _ = builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("email");


        //-------------------------------------
        _ = builder.Property(e => e.EmailVerifiedAt)
            .HasColumnName("email_verified_at");


        //-------------------------------------
        _ = builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255);


        //-------------------------------------
        _ = builder.Property(e => e.TimeZone)
            .HasColumnName("time_zone")
            .HasMaxLength(64);

        //-------------------------------------
        builder.HasMany(u => u.Bans)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .IsRequired()                      // FK NOT NULL
               .OnDelete(DeleteBehavior.NoAction); // или Restrict/NoAction
    }
}
