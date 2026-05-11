using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;

namespace Server.DTO.Collection;

public static class HeroFactory
{
    public static Hero CreateFromBaseHero(BaseHero bh, Guid userId) => new()
    {
        UserId = userId,
        BaseHeroId = bh.Id,
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
}
