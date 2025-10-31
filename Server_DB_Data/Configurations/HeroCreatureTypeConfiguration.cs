using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Configurations;
internal class HeroCreatureTypeConfiguration : IEntityTypeConfiguration<Entities.HeroCreatureType>
{
    public void Configure(EntityTypeBuilder<Entities.HeroCreatureType> builder)
    {
        _ = builder.ToTable("hero_x_creature_type", "relations");

        // 2. Составной первичный ключ
        _ = builder.HasKey(hct => new { hct.HeroId, hct.CreatureTypeId })
              .HasName("pk_hero_x_creature_type");

        // 3. Настройка связи с Hero
        _ = builder.HasOne(hct => hct.Hero)
              .WithMany(h => h.CreatureTypes)
              .HasForeignKey(hct => hct.HeroId)
              .HasConstraintName("fk_hero_x_creature_type_hero")
              .OnDelete(DeleteBehavior.Cascade);

        // 4. Настройка связи с CreatureType
        _ = builder.HasOne(hct => hct.CreatureType)
              .WithMany(ct => ct.Heroes)
              .HasForeignKey(hct => hct.CreatureTypeId)
              .HasConstraintName("fk_hero_x_creature_type_creature_type")
              .OnDelete(DeleteBehavior.Cascade);

        // 5. Настройка колонок
        _ = builder.Property(hct => hct.HeroId)
              .HasColumnName("hero_id")
              .IsRequired();

        _ = builder.Property(hct => hct.CreatureTypeId)
              .HasColumnName("creature_type_id")
              .IsRequired();

        _ = builder.Property(hct => hct.CreatedAt)
              .HasColumnName("created_at")
              .HasDefaultValueSql("NOW()")
              .ValueGeneratedOnAdd();

        // 6. Настройка индексов
        _ = builder.HasIndex(hct => hct.HeroId)
              .HasDatabaseName("ix_hero_x_creature_type_hero_id");

        _ = builder.HasIndex(hct => hct.CreatureTypeId)
              .HasDatabaseName("ix_hero_x_creature_type_creature_type_id");
    }

}
