using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Server.BattleField;
using Server.Extensions;

namespace Server.Battlefield.Abilities;

/// <summary>Выбирает одного живого противника и наносит обычный удар с возможностью критического урона.</summary>
public sealed class Attack(Ability definition) : BattleAbility(definition)
{
    private const float COEF_EFFECTIVENESS = 1f;

    /// <summary>Оценивает урон доступной атаки; во время кулдауна эффективность равна нулю.</summary>
    public override void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context)
    {
        effectiveness = 0f;
        if (cooldownRemaining > 0 || caster.health <= 0f || definition.cost > caster.actionPoints)
        {
            return;
        }

        float damageExpected = caster.damage * GetExpectedCritMultiplier(caster);
        if (!float.IsFinite(damageExpected))
        {
            return;
        }

        float maxHp = context.heroes.Where(h => h.health > 0 && h.team != caster.team)
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

        SpawnedHero? target = context.heroes.Where(hero => hero.health > 0 && hero.team != caster.team).GetRandomElement();
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
        target.health -= damage;
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
