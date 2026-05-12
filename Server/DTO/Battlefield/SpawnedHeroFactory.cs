using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Server.Utilities;

namespace Server.DTO.Battlefield;

public static class SpawnedHeroFactory
{
    public static SpawnedHero CreateFromBaseHero(BaseHero bh, int level) => new()
    {
        SpawnedId = UUID.CreateV7(),
        BaseHeroId = bh.Id,
        Level = level,
        Health = bh.Health.NewRandomDice(),
        Strength = bh.Strength.NewRandomDice(),
        Agility = bh.Agility.NewRandomDice(),
        Intelligence = bh.Intelligence.NewRandomDice(),
        CritChance = bh.CritChance.NewRandomDice(),
        CritMultiplier = bh.CritMultiplier.NewRandomDice(),
        EnduranceMagical = bh.EnduranceMagical.NewRandomDice(),
        EndurancePhysical = bh.EndurancePhysical.NewRandomDice(),
        Haste = bh.Haste.NewRandomDice(),
        Initiative = bh.Initiative.NewRandomDice(),
        Versality = bh.Versality.NewRandomDice()
    };

    public static SpawnedHero CreateFromHero(Hero h) => new()
    {
        SpawnedId = UUID.CreateV7(),
        Level = h.Level,
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
