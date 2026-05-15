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
            SpawnedId = UUID.CreateV7(),
            BaseHeroId = bh.Id,
            Level = level,
            Health = bh.Health.GetRandomValue(),
            HealthMax = 0,
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
            CoefPowerByLevel = level > 1 ? MathF.Pow(BattlefieldManager.LEVEL_MULTIPLIER, level - 1) : 1,
            Damage = bh.Damage.GetRandomValue()
        };

        result.Health *= result.CoefPowerByLevel;
        result.HealthMax = result.Health;
        return result;
    }

    public static SpawnedHero CreateFromHero(Hero h)
    {
        SpawnedHero result = new()
        {
            SpawnedId = UUID.CreateV7(),
            Level = h.Level,
            BaseHeroId = h.BaseHeroId,
            Health = h.Health,
            HealthMax = 0,
            Strength = h.Strength,
            Agility = h.Agility,
            Intelligence = h.Intelligence,
            CritChance = h.CritChance,
            CritMultiplier = h.CritMultiplier,
            EnduranceMagical = h.EnduranceMagical,
            EndurancePhysical = h.EndurancePhysical,
            Haste = h.Haste,
            Initiative = h.Initiative,
            Versality = h.Versality,
            CoefPowerByLevel = h.Level > 1 ? MathF.Pow(BattlefieldManager.LEVEL_MULTIPLIER, h.Level - 1) : 1,
            Damage = h.Damage
        };

        result.Health *= result.CoefPowerByLevel;
        result.HealthMax = result.Health;
        return result;
    }
}
