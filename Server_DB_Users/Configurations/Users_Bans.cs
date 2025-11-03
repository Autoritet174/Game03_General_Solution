using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Users.Entities;

namespace Server_DB_Users.Configurations;
internal class Users_Bans : IEntityTypeConfiguration<User_Ban>
{
    public void Configure(EntityTypeBuilder<User_Ban> builder)
    {
        _ = builder.ToTable("users_bans", "_main");


        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasKey(e => e.Id);

        _ = builder.Property(e => e.UserId)
           .HasColumnName("user_id")
           .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.ExpiresAt)
            .HasColumnName("expires_at");

        //-------------------------------------
        _ = builder.Property(e => e.UserBansReasonsId)
          .HasColumnName("user_bans_reasons_id");
    }
}
