using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Server.BattleField;
using Server.Extensions;

namespace Server.Battlefield.Abilities;

/// <summary></summary>
public sealed class Fire_bolt(Ability definition) : BattleAbility(definition)
{
    private const float COEF_EFFECTIVENESS = 1.0f;
    private const float BASE_DAMAGE = 50.0f;

    public override void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context)
    {
        effectiveness = 0f;
        if (cooldownRemaining > 0 || caster.health <= 0f || definition.cost > caster.actionPoints)
        {
            return;
        }

        float damageExpected = BASE_DAMAGE * GetExpectedCritMultiplier(caster);
        if (!float.IsFinite(damageExpected))
        {
            return;
        }
        float maxHp = context.heroes.Where(h => h.health > 0f && h.team != caster.team)
            .Select(h => h.health).DefaultIfEmpty(0f).Max();
        if (damageExpected > maxHp)
        {
            damageExpected = maxHp;
        }
        effectiveness = damageExpected * COEF_EFFECTIVENESS;
    }

    public override bool UseAbility(SpawnedHero caster, BattleAbilityContext context)
    {
        if (cooldownRemaining > 0 || caster.health <= 0 || definition.cost > caster.actionPoints)
        {
            return false;
        }

        SpawnedHero? target = context.heroes.Where(hero => hero.health > 0f && hero.team == caster.team && hero.health < hero.healthMax).GetRandomElement();
        if (target == null)
        {
            return false;
        }

        (float damage, bool isCrit) = CalculateCritValue(caster.damage, caster);
        damage = MathF.Min(damage, target.health);
        if (!float.IsFinite(damage))
        {
            return false;
        }

        context.ChangeActionPoints(caster, -definition.cost);
        int indexReason = context.RecordAbilityUse(caster, id, [target.spawnedId]);
        target.health += damage;
        _ = context.AddLog(new BattlefieldLogRecord_Damage
        {
            hero1Id = caster.spawnedId,
            hero2Id = target.spawnedId,
            indexReason = indexReason,
            damage = damage,
            isCrit = isCrit,
            isPerodic = false,
        });

        StartCooldown();
        return true;
    }
}
