using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Server.BattleField;
using Server.Extensions;

namespace Server.Battlefield.Abilities;

/// <summary>Выбирает одного живого противника и наносит обычный удар с возможностью критического урона.</summary>
public sealed class Attack : IBattleAbility
{
    private const float COEF_EFFECTIVENESS = 1f;
    public EBattlefieldLogAbility id => EBattlefieldLogAbility.attack;
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

        float damage = caster.damage * (1 + (caster.critChance / 100f * (caster.critMultiplier / 100f)));
        if (!float.IsFinite(damage))
        {
            return;
        }

        float maxHp = context.heroes.Where(h => h.health > 0 && h.team != caster.team).Max(h => h.health);
        if (damage > maxHp)
        {
            damage = maxHp;
        }
        effectiveness = damage * COEF_EFFECTIVENESS;
    }

    public bool UseAbility(SpawnedHero caster, BattleAbilityContext context)
    {
        BaseHero baseHero = context.cacheService.TableBaseHeroes[caster.baseHeroId];
        Ability ability = baseHero.abilities.First(a => a != null && a.code == id);
        SpawnedHero? target = context.heroes.Where(hero => hero.health > 0 && hero.team != caster.team).GetRandomElement();
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
        damage = MathF.Min(damage, target.health);
        if (!float.IsFinite(damage))
        {
            return false;
        }

        context.ChangeActionPoints(caster, -ability.cost);
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

        return true;
    }
}
