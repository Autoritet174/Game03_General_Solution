using General;
using General.DTO.Battlefield;
using Server.Extensions;

namespace Server.Battlefield.Abilities;

/// <summary>Выбирает одного живого противника и наносит обычный удар с возможностью критического урона.</summary>
public sealed class Attack : IBattleAbility
{
    public const int COST_AP = 10;

    public EBattlefieldLogAbility id => EBattlefieldLogAbility.attack;

    /// <summary>Атакует случайного противника, если герой жив и имеет достаточно очков действия.</summary>
    public bool TryUse(SpawnedHero caster, BattleAbilityContext context)
    {
        if (caster.health is not > 0 || caster.actionPoints < COST_AP)
        {
            return false;
        }

        SpawnedHero? target = context.heroes
            .Where(hero => hero.health > 0 && hero.team != caster.team)
            .GetRandomElement();
        if (target == null)
        {
            return false;
        }

        float damage = caster.damage;
        bool isCrit = false;
        if (Random.Shared.NextSingle() * 100 < caster.critChance)
        {
            damage *= (caster.critMultiplier / 100f) + 1;
            isCrit = true;
        }

        context.ChangeActionPoints(caster, -COST_AP);
        int indexReason = context.RecordAbilityUse(caster, id, [target.spawnedId]);
        context.ApplyDamage(caster, target, damage, indexReason, isCrit);
        return true;
    }
}
