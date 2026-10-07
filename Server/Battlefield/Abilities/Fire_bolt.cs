using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Server.BattleField;

namespace Server.Battlefield.Abilities;

/// <summary></summary>
public sealed class Fire_bolt(Ability definition) : BattleAbility(definition)
{
    private const float COEF_EFFECTIVENESS = 1.0f;

    public override void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context)
    {
        effectiveness = 0f;
        if (cooldownRemaining > 0 || caster.health <= 0 || definition.cost > caster.actionPoints)
        {
            return;
        }

        float healing = caster.intelligence * (1 + (caster.critChance / 100f * (caster.critMultiplier / 100f)));
        float maxHp = context.heroes.Where(h => h.health > 0 && h.team != caster.team)
            .Select(h => h.health).DefaultIfEmpty(0f).Max();
        if (healing > maxHp)
        {
            healing = maxHp;
        }
        effectiveness = healing * COEF_EFFECTIVENESS;
    }

    public override bool UseAbility(SpawnedHero caster, BattleAbilityContext context)
    {
        if (cooldownRemaining > 0 || caster.health <= 0 || definition.cost > caster.actionPoints)
        {
            return false;
        }

        SpawnedHero? target = context.heroes.Where(hero => hero.health > 0 && hero.team == caster.team && hero.health < hero.healthMax).MaxBy(h => h.healthMax - h.health);
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

        context.ChangeActionPoints(caster, -definition.cost);
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

        StartCooldown();
        return true;
    }
}
