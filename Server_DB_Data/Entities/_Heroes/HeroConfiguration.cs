using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities._Heroes;
using Server_DB_Data.Entities.X_Cross;

namespace Server_DB_Data.Entities._Heroes;

internal class HeroConfiguration : IEntityTypeConfiguration<Hero>
{
    public void Configure(EntityTypeBuilder<Hero> builder)
    {
        _ = builder.Property(a => a.IsUnique).HasDefaultValue(false);
        _ = builder.Property(a => a.MainStat).HasDefaultValue(0);
    }

}
