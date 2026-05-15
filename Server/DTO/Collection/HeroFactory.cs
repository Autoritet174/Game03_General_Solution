using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;

namespace Server.DTO.Collection;

public static class HeroFactory
{
    public static Hero CreateFromBaseHero(BaseHero bh, Guid userId) => new()
    {
        UserId = userId,
        BaseHeroId = bh.Id,
        Health = bh.Health.GetRandomValue(),
        Strength = bh.Strength.GetRandomValue(),
        Agility = bh.Agility.GetRandomValue(),
        Intelligence = bh.Intelligence.GetRandomValue(),
        CritChance = bh.CritChance.GetRandomValue(),
        CritMultiplier = bh.CritMultiplier.GetRandomValue(),
        EnduranceMagical = bh.EnduranceMagical.GetRandomValue(),
        EndurancePhysical = bh.EndurancePhysical.GetRandomValue(),
        Haste = bh.Haste.GetRandomValue(),
        Initiative = bh.Initiative.GetRandomValue(),
        Versality = bh.Versality.GetRandomValue(),
        Damage = bh.Damage.GetRandomValue()
    };
}
