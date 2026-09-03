using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Server.Battlefield;
using Server.Utilities;

namespace Server.DTO.Battlefield;

public static class SpawnedHeroFactory
{
    public static SpawnedHero CreateFromBaseHero(BaseHero bh, int level)
    {
        if (level < 1)
        {
            throw new ArgumentException("level most be 1 or more");
        }

        SpawnedHero result = new()
        {
            spawnedId = UUID.CreateV7(),
            baseHeroId = bh.id,
            level = level,
            health = bh.health.GetRandomValue(),
            healthMax = 0,
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
            coefPowerByLevel = level > 1 ? MathF.Pow(BattlefieldManager.LEVEL_MULTIPLIER, level - 1) : 1,
            damage = bh.damage.GetRandomValue()
        };

        result.health *= result.coefPowerByLevel;
        result.healthMax = result.health;
        return result;
    }

    public static SpawnedHero CreateFromHero(Hero h)
    {
        SpawnedHero result = new()
        {
            spawnedId = UUID.CreateV7(),
            level = h.level,
            baseHeroId = h.baseHeroId,
            health = h.health,
            healthMax = 0,
            strength = h.strength,
            agility = h.agility,
            intelligence = h.intelligence,
            critChance = h.critChance,
            critMultiplier = h.critMultiplier,
            enduranceMagical = h.enduranceMagical,
            endurancePhysical = h.endurancePhysical,
            haste = h.haste,
            initiative = h.initiative,
            versality = h.versality,
            coefPowerByLevel = h.level > 1 ? MathF.Pow(BattlefieldManager.LEVEL_MULTIPLIER, h.level - 1) : 1,
            damage = h.damage
        };

        result.health *= result.coefPowerByLevel;
        result.healthMax = result.health;
        return result;
    }
}
