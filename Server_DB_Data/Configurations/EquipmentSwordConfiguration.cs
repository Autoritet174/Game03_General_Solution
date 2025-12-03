using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities;
using System.Reflection.Emit;

namespace Server_DB_Data.Configurations;

internal class EquipmentSwordConfiguration : IEntityTypeConfiguration<EquipmentSword>
{
    public void Configure(EntityTypeBuilder<EquipmentSword> builder)
    {
        _ = builder.ToTable("sword", "equipment");


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
        _ = builder.Property(e => e.Rarity)
            .HasColumnName("rarity")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.IsUnique)
            .HasColumnName("is_unique")
            .IsRequired();

        //-------------------------------------
        _ = builder.Property(e => e.Attack)
            .HasColumnName("attack")
            .HasMaxLength(50)
            .HasColumnType(Config.VARCHAR)
            .IsRequired();
    }
}
