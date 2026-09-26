using General.DTO.Battlefield;

namespace Server.BattleField;

/// <summary>Предоставляет всех участников боя и операции изменения состояния с записью в общий журнал.</summary>
public sealed class BattleAbilityContext(IReadOnlyList<SpawnedHero> heroes, List<BattlefieldLogRecordBase> battleLog)
{
    public IReadOnlyList<SpawnedHero> heroes { get; } = heroes;

    /// <summary>Добавляет событие в журнал и возвращает его индекс для связи с последующими эффектами.</summary>
    public int AddLog(BattlefieldLogRecordBase log)
    {
        log.index = battleLog.Count + 1;
        battleLog.Add(log);
        return log.index;
    }

    /// <summary>Изменяет очки действия героя и записывает приращение в журнал.</summary>
    public void ChangeActionPoints(SpawnedHero hero, int actionPointsChange)
    {
        hero.actionPoints += actionPointsChange;
        _ = AddLog(new BattlefieldLogRecord_ChangeActionPoints
        {
            spawnedHeroId = hero.spawnedId,
            countAP = actionPointsChange
        });
    }

    /// <summary>Записывает применение способности и возвращает индекс причины её эффектов.</summary>
    public int RecordAbilityUse(SpawnedHero hero, EBattlefieldLogAbility ability, Guid[] spawnedHeroTargets)
    {
        return AddLog(new BattlefieldLogRecord_UseAbility
        {
            spawnedHero1Id = hero.spawnedId,
            ability = ability,
            spawnedHeroTargets = spawnedHeroTargets
        });
    }

    /// <summary>Уменьшает здоровье цели и связывает событие урона с вызвавшей его записью.</summary>
    public void ApplyDamage(SpawnedHero caster, SpawnedHero target, float damage, int indexReason, bool isCrit = false, bool isPeriodic = false)
    {
        target.health -= damage;
        _ = AddLog(new BattlefieldLogRecord_Damage
        {
            hero1Id = caster.spawnedId,
            hero2Id = target.spawnedId,
            indexReason = indexReason,
            damage = damage,
            isCrit = isCrit,
            isPerodic = isPeriodic
        });
    }

    /// <summary>Восстанавливает здоровье в пределах максимума и записывает фактическое лечение.</summary>
    public void ApplyHealing(SpawnedHero caster, SpawnedHero target, float healing, int indexReason)
    {
        float actualHealing = MathF.Min(MathF.Max(0, healing), MathF.Max(0, target.healthMax - target.health));
        target.health += actualHealing;
        _ = AddLog(new BattlefieldLogRecord_Healing
        {
            hero1Id = caster.spawnedId,
            hero2Id = target.spawnedId,
            indexReason = indexReason,
            healing = actualHealing
        });
    }
}
