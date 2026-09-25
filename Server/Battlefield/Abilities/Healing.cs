using General;
using General.DTO.Battlefield;
using Server.Extensions;

namespace Server.Battlefield.Abilities;

/// <summary>Самостоятельно выбирает одного раненого союзника и восстанавливает его здоровье.</summary>
public sealed class Healing : IBattleAbility
{
    public const int COST_AP = 10;
    public const float HEALING_MULTIPLIER = 1f;

    public EBattlefieldLogAbility id => EBattlefieldLogAbility.healing;

    /// <summary>Применяет исцеление, если герой жив, имеет очки действия и подходящую цель.</summary>
    public bool TryUse(SpawnedHero caster, BattleAbilityContext context)
    {
        float requestedHealing = caster.damage * HEALING_MULTIPLIER;
        if (caster.health is not > 0 || caster.actionPoints < COST_AP
            || !float.IsFinite(requestedHealing) || requestedHealing <= 0)
        {
            return false;
        }

        SpawnedHero? target = context.heroes
            .Where(hero => hero.health > 0 && hero.team == caster.team && hero.health < hero.healthMax)
            .GetRandomElement();
        if (target == null)
        {
            return false;
        }

        context.ChangeActionPoints(caster, -COST_AP);
        int indexReason = context.RecordAbilityUse(caster, id, [target.spawnedId]);
        context.ApplyHealing(caster, target, requestedHealing, indexReason);
        return true;
    }
}
