using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DB.Users.Entities;

namespace Server.DB.Users.Configurations;
internal class Users : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.ToTable("users");


        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        _ = builder.HasKey(e => e.Id)
            .HasName("pk_users_id");


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
        _ = builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");


        //-------------------------------------
        _ = builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        _ = builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("email");


        //-------------------------------------
        _ = builder.Property(e => e.EmailVerifiedAt)
            .HasColumnName("email_verified_at");


        //-------------------------------------
        _ = builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();


        //-------------------------------------
        _ = builder.Property(e => e.TimeZone)
            .HasColumnName("time_zone")
            .HasMaxLength(64);

    }
}
