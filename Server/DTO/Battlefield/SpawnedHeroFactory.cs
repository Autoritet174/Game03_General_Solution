using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Server.Utilities;

namespace Server.DTO.Battlefield;

public static class SpawnedHeroFactory
{
    public static SpawnedHero CreateFromBaseHero(BaseHero bh) => new()
    {
        SpawnedId = UUID.CreateV7(),
        BaseHeroId = bh.Id,
        Health = bh.Health.GetRandom(),
        Strength = bh.Strength.GetRandom(),
        Agility = bh.Agility.GetRandom(),
        Intelligence = bh.Intelligence.GetRandom(),
        CritChance = bh.CritChance.GetRandom(),
        CritMultiplier = bh.CritMultiplier.GetRandom(),
        EnduranceMagical = bh.EnduranceMagical.GetRandom(),
        EndurancePhysical = bh.EndurancePhysical.GetRandom(),
        Haste = bh.Haste.GetRandom(),
        Initiative = bh.Initiative.GetRandom(),
        Versality = bh.Versality.GetRandom()
    };

    public static SpawnedHero CreateFromHero(Hero h) => new()
    {
        SpawnedId = UUID.CreateV7(),
        BaseHeroId = h.BaseHeroId,
        Health = h.Health,
        Strength = h.Strength,
        Agility = h.Agility,
        Intelligence = h.Intelligence,
        CritChance = h.CritChance,
        CritMultiplier = h.CritMultiplier,
        EnduranceMagical = h.EnduranceMagical,
        EndurancePhysical = h.EndurancePhysical,
        Haste = h.Haste,
        Initiative = h.Initiative,
        Versality = h.Versality
    };
}
