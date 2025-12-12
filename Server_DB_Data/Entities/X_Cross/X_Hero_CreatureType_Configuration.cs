using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server_DB_Data.Entities.X_Cross;

internal class X_Hero_CreatureType_Configuration : IEntityTypeConfiguration<X_Hero_CreatureType>
{
    public void Configure(EntityTypeBuilder<X_Hero_CreatureType> builder)
    {
        // Связь с Hero (Many-to-One)
        builder.HasOne(x => x.Hero).WithMany(a => a.X_Hero_CreatureType).HasForeignKey(xx => xx.HeroId).OnDelete(DeleteBehavior.Cascade);

        // Связь с CreatureType (Many-to-One)
        builder.HasOne(x => x.CreatureTypes).WithMany(a => a.X_Hero_CreatureType).HasForeignKey(xx => xx.CreatureTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}
