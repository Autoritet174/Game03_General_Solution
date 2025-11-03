using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities;

namespace Server_DB_Data.Configurations;
internal class HeroesConfiguration : IEntityTypeConfiguration<Hero>
{
    public void Configure(EntityTypeBuilder<Hero> builder)
    {
        _ = builder.ToTable("heroes", "_main");


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
        _ = builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        _ = builder.HasIndex(e => e.Name)
            .IsUnique();

        //-------------------------------------
        _ = builder.Property(e => e.BaseHealth)
            .HasColumnName("base_health")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.Rarity)
            .HasColumnName("rarity")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.BaseAttack)
            .HasColumnName("base_attack")
            .IsRequired();

    }
}
