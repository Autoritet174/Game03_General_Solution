using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Entities.__Lists;

internal class WeaponType_Configuration : IEntityTypeConfiguration<WeaponType>
{
    public void Configure(EntityTypeBuilder<WeaponType> builder)
    {
        _ = builder.Property(a => a.Mass).HasDefaultValue(0);
    }
}
