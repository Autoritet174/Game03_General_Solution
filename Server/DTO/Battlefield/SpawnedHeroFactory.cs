using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Server.Battlefield;
using Server.Battlefield.Abilities;
using Server.BattleField;
using Server.Utilities;

namespace Server.DTO.Battlefield;

public static class SpawnedHeroFactory
{
    private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole())
        .CreateLogger(nameof(SpawnedHeroFactory));

    public static ServerSpawnedHero CreateFromBaseHero(BaseHero bh, int level)
    {
        if (level < 1)
        {
            throw new ArgumentException("level most be 1 or more");
        }

        ServerSpawnedHero result = new()
        {
            abilities = CreateAbilities(bh),
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

    public static ServerSpawnedHero CreateFromHero(Hero hero, IEnumerable<Equipment> equipments, BaseHero baseHero)
    {
        int heroLevel = hero.level;
        ServerSpawnedHero sHero = new()
        {
            abilities = CreateAbilities(baseHero),
            spawnedId = UUID.CreateV7(),
            level = heroLevel,
            baseHeroId = hero.baseHeroId,
            health = hero.health,
            healthMax = 0,
            strength = hero.strength,
            agility = hero.agility,
            intelligence = hero.intelligence,
            critChance = hero.critChance,
            critMultiplier = hero.critMultiplier,
            enduranceMagical = hero.enduranceMagical,
            endurancePhysical = hero.endurancePhysical,
            haste = hero.haste,
            initiative = hero.initiative,
            versality = hero.versality,
            coefPowerByLevel = heroLevel > 1 ? MathF.Pow(BattlefieldManager.LEVEL_MULTIPLIER, heroLevel - 1) : 1,
            damage = hero.damage
        };

        sHero.health *= sHero.coefPowerByLevel;

        // корректировка характеристик героя по одетым предметам
        foreach (Equipment e in equipments)
        {
            if (e.stats == null || e.stats.Count < 1)
            {
                continue;
            }
            foreach (KeyValuePair<EStatType, List<float>> stat in e.stats)
            {
                if (stat.Value == null || stat.Value.Count < 1) {
                    continue;
                }
                switch (stat.Key)
                {
                    case EStatType.health:
                        sHero.health += stat.Value.Sum();
                        break;
                    case EStatType.damage:
                        sHero.damage += stat.Value.Sum();
                        break;
                    case EStatType.strength:
                        sHero.strength += stat.Value.Sum();
                        break;
                    case EStatType.agility:
                        sHero.agility += stat.Value.Sum();
                        break;
                    case EStatType.intelligence:
                        sHero.intelligence += stat.Value.Sum();
                        break;
                    case EStatType.critChance:
                        sHero.critChance += stat.Value.Sum();
                        break;
                    case EStatType.critMultiplier:
                        sHero.critMultiplier += stat.Value.Sum();
                        break;
                    case EStatType.haste:
                        sHero.haste += stat.Value.Sum();
                        break;
                    case EStatType.versality:
                        sHero.versality += stat.Value.Sum();
                        break;
                    case EStatType.initiative:
                        sHero.initiative += stat.Value.Sum();
                        break;
                    case EStatType.none:
                        break;
                    default:
                        logger.LogWarning("Не реализована обработка характеристики {StatType} экипировки {EquipmentId} при создании героя {HeroId}", stat.Key, e.id, hero.id);
                        break;
                }
            }
        }


        sHero.healthMax = sHero.health;
        return sHero;
    }

    /// <summary>Создаёт личное состояние способностей из общего списка базового героя.</summary>
    private static List<BattleAbility> CreateAbilities(BaseHero baseHero)
    {
        List<BattleAbility> abilities = [];
        foreach (Ability definition in baseHero.abilities)
        {
            BattleAbility? ability = definition.code switch
            {
                EBattlefieldLogAbility.attack => new Attack(definition),
                EBattlefieldLogAbility.healing => new Healing(definition),
                _ => null
            };

            if (ability is null)
            {
                logger.LogError("Ability not found: {AbilityCode}", definition.code);
                continue;
            }

            abilities.Add(ability);
        }

        return abilities;
    }
}
