using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities;

namespace Server_DB_Data.Configurations;

internal class TypeDamageConfiguration : IEntityTypeConfiguration<TypeDamage>
{
    public void Configure(EntityTypeBuilder<TypeDamage> builder)
    {
        _ = builder.ToTable("type_damage", "_main");

        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            //.HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd()
            ;

        _ = builder.HasKey(e => e.Id);

        //-------------------------------------
        _ = builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        _ = builder.HasIndex(e => e.Name)
            .IsUnique();

        //-------------------------------------
        _ = builder.Property(e => e.NameRu)
            .HasColumnName("name_ru")
            .HasMaxLength(255)
            .IsRequired();
    }
}
