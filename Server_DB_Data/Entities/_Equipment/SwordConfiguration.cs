using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities.X_Cross;

namespace Server_DB_Data.Entities._Equipment;

internal class SwordConfiguration : IEntityTypeConfiguration<_Equipment.Sword>
{
    public void Configure(EntityTypeBuilder<_Equipment.Sword> builder)
    {
        builder.Property(a => a.IsUnique).HasDefaultValue(false);
    }

}
