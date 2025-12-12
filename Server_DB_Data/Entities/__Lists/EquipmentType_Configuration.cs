using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Entities.__Lists;

internal class EquipmentType_Configuration : IEntityTypeConfiguration<EquipmentType>
{
    public void Configure(EntityTypeBuilder<EquipmentType> builder)
    {
        _ = builder.Property(a => a.Mass).HasDefaultValue(0);
        _ = builder.Property(a => a.CanCraftSmithing).HasDefaultValue(false);
        _ = builder.Property(a => a.CanCraftJewelcrafting).HasDefaultValue(false);
    }
}
