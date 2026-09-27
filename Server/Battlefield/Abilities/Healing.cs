using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Server.BattleField;
using Server.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Battlefield.Abilities;

/// <summary>Самостоятельно выбирает одного раненого союзника и восстанавливает его здоровье.</summary>
public sealed class Healing : IBattleAbility
{
    private const float COEF_EFFECTIVENESS = 1f;

    public EBattlefieldLogAbility id => EBattlefieldLogAbility.healing;
    public float effectiveness { get; private set; } = 0f;

    public void ResetEffectiveness()
    {
        effectiveness = 0f;
    }

    /// <summary></summary>
    public void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context)
    {
        effectiveness = 0f;
        if (caster.health <= 0)
        {
            return;
        }

        BaseHero baseHero = context.cacheService.TableBaseHeroes[caster.baseHeroId];
        if (baseHero.abilities.Count == 0)
        {
            return;
        }
        Ability? ability = baseHero.abilities.FirstOrDefault(a => a != null && a.code == id);
        if (ability == null)
        {
            return;
        }

        if (ability.cost > caster.actionPoints)
        {
            return;
        }

        float healing = caster.intelligence * (1 + (caster.critChance / 100f * (caster.critMultiplier / 100f)));
        float maxHp = context.heroes.Where(h => h.health > 0 && h.team != caster.team).Max(h => h.health);
        if (healing > maxHp)
        {
            healing = maxHp;
        }
        effectiveness = healing * COEF_EFFECTIVENESS;
    }

    public bool UseAbility(SpawnedHero caster, BattleAbilityContext context)
    {
        BaseHero baseHero = context.cacheService.TableBaseHeroes[caster.baseHeroId];
        Ability ability = baseHero.abilities.First(a => a != null && a.code == id);
        SpawnedHero? target = context.heroes
            .Where(hero => hero.health > 0 && hero.team == caster.team && hero.health < hero.healthMax)
            .OrderByDescending(h => h.healthMax - h.health).FirstOrDefault();
        if (target == null)
        {
            return false;
        }

        float healing = caster.intelligence;
        bool isCrit = false;
        if (Random.Shared.NextSingle() * 100 < caster.critChance)
        {
            healing *= (caster.critMultiplier / 100f) + 1;
            isCrit = true;
        }

        float missingHealth = MathF.Max(0, target.healthMax - target.health);
        float actualHealing = MathF.Min(healing, missingHealth);
        if (!float.IsFinite(actualHealing))
        {
            return false;
        }

        context.ChangeActionPoints(caster, -ability.cost);
        int indexReason = context.RecordAbilityUse(caster, id, [target.spawnedId]);
        target.health += actualHealing;
        _ = context.AddLog(new BattlefieldLogRecord_Healing
        {
            hero1Id = caster.spawnedId,
            hero2Id = target.spawnedId,
            indexReason = indexReason,
            healing = actualHealing,
            isCrit = isCrit,
            isPerodic = false,
        });

        return true;
    }
}
