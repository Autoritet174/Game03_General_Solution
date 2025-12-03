using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Configurations;

internal class X_HeroCreatureTypeConfiguration : IEntityTypeConfiguration<Entities.HeroCreatureType>
{
    public void Configure(EntityTypeBuilder<Entities.HeroCreatureType> builder)
    {
        _ = builder.ToTable("x_hero_creature_type", "xcross");

        // 2. Составной первичный ключ
        _ = builder.HasKey(hct => new { hct.HeroId, hct.CreatureTypeId });

        //// 3. Настройка связи с Hero
        //_ = builder.HasOne(hct => hct.Hero)
        //      .WithMany(h => h.CreatureTypes)
        //      .HasForeignKey(hct => hct.HeroId)
        //      .OnDelete(DeleteBehavior.Cascade);

        // 5. Настройка колонок
        _ = builder.Property(hct => hct.HeroId)
              .HasColumnName("hero_id")
              .IsRequired();

        _ = builder.Property(hct => hct.CreatureTypeId)
              .HasColumnName("creature_type_id")
              .IsRequired();

        // 6. Настройка индексов
        _ = builder.HasIndex(hct => hct.HeroId);

        _ = builder.HasIndex(hct => hct.CreatureTypeId);
    }

}
