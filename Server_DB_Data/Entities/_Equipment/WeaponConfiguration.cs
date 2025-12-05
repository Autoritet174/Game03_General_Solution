using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities.X_Cross;

namespace Server_DB_Data.Entities._Equipment;

internal class WeaponConfiguration : IEntityTypeConfiguration<_Equipment.Weapon>
{
    public void Configure(EntityTypeBuilder<_Equipment.Weapon> builder)
    {
        _ = builder.Property(a => a.IsUnique).HasDefaultValue(false);
    }

}
