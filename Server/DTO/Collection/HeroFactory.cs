using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;

namespace Server.DTO.Collection;

public static class HeroFactory
{
    public static Hero CreateFromBaseHero(BaseHero bh, Guid userId) => new()
    {
        userId = userId,
        baseHeroId = bh.id,
        health = bh.health.GetRandomValue(),
        strength = bh.strength.GetRandomValue(),
        agility = bh.agility.GetRandomValue(),
        intelligence = bh.intelligence.GetRandomValue(),
        critChance = bh.critChance.GetRandomValue(),
        critMultiplier = bh.critMultiplier.GetRandomValue(),
        enduranceMagical = bh.enduranceMagical.GetRandomValue(),
        endurancePhysical = bh.endurancePhysical.GetRandomValue(),
        haste = bh.haste.GetRandomValue(),
        initiative = bh.initiative.GetRandomValue(),
        versality = bh.versality.GetRandomValue(),
        damage = bh.damage.GetRandomValue()
    };
}
