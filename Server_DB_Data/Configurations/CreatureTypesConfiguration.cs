using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities;

namespace Server_DB_Data.Configurations;
internal class CreatureTypesConfiguration : IEntityTypeConfiguration<CreatureType>
{
    public void Configure(EntityTypeBuilder<CreatureType> builder)
    {
        _ = builder.ToTable("creature_types", "_main");


        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        _ = builder.HasKey(e => e.Id);
            //.HasName("creature_types____pkey");


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

    }

}
