using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DB.Data.Entities;

namespace Server.DB.Data.Configurations;
internal class CreatureTypes : IEntityTypeConfiguration<CreatureType>
{
    public void Configure(EntityTypeBuilder<CreatureType> builder)
    {
        _ = builder.ToTable("creature_types");


        //Уникальный идентификатор и индекс первичного ключа
        _ = builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        _ = builder.HasKey(e => e.Id)
            .HasName("pk_creature_types_id");


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
        _ = builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        _ = builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("ix_creature_types_name");
    }

}
