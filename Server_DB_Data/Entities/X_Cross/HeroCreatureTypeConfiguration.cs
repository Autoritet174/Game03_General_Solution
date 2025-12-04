using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server_DB_Data.Entities.X_Cross;

namespace Server_DB_Data.Entities.XCross;

internal class SwordConfiguration : IEntityTypeConfiguration<X_HeroCreatureType>
{
    public void Configure(EntityTypeBuilder<X_HeroCreatureType> builder)
    {
        // Настройка таблицы связи многие-ко-многим
        //_ = builder.HasOne(x => x.Hero).WithMany(h => h.CreatureTypes).HasForeignKey(x => x.HeroId);

        //_ = builder.HasOne(x => x.CreatureType).WithMany(ct => ct.Heroes).HasForeignKey(x => x.CreatureTypeId);
    }

}
