using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Entities.X_Cross;

internal class X_EquipmentType_DamageType_Configuration : IEntityTypeConfiguration<X_EquipmentType_DamageType>
{
    public void Configure(EntityTypeBuilder<X_EquipmentType_DamageType> builder)
    {
        // Связь с DamageType (Many-to-One)
        _ = builder.HasOne(x => x.DamageType).WithMany(a => a.X_EquipmentType_DamageType).HasForeignKey(xx => xx.DamageTypeId).OnDelete(DeleteBehavior.Cascade);

        // Связь с WeaponType (Many-to-One)
        _ = builder.HasOne(x => x.EquipmentType).WithMany(a => a.X_EquipmentType_DamageType).HasForeignKey(xx => xx.EquipmentTypeId).OnDelete(DeleteBehavior.Cascade);

        _ = builder.Property(a => a.DamageCoef).HasDefaultValue(0);
    }

}
