using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Entities.X_Cross;

internal class X_WeaponType_DamageType_Configuration : IEntityTypeConfiguration<X_WeaponType_DamageType>
{
    public void Configure(EntityTypeBuilder<X_WeaponType_DamageType> builder)
    {
        // Связь с DamageType (Many-to-One)
        _ = builder.HasOne(x => x.DamageType).WithMany(a => a.X_WeaponType_DamageType).HasForeignKey(xx => xx.DamageTypeId).OnDelete(DeleteBehavior.Cascade);

        // Связь с WeaponType (Many-to-One)
        _ = builder.HasOne(x => x.WeaponType).WithMany(a => a.X_WeaponType_DamageType).HasForeignKey(xx => xx.WeaponTypeId).OnDelete(DeleteBehavior.Cascade);

        _ = builder.Property(a => a.DamageCoef).HasDefaultValue(0);
    }

}
