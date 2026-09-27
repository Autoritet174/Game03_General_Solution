using General.DTO.Battlefield;
using Server.Cache;

namespace Server.BattleField;

/// <summary>Предоставляет всех участников боя и операции изменения состояния с записью в общий журнал.</summary>
public sealed class BattleAbilityContext(IReadOnlyList<SpawnedHero> heroes, List<BattlefieldLogRecordBase> battleLog, CacheService cacheService)
{
    public IReadOnlyList<SpawnedHero> heroes { get; } = heroes;
    public CacheService cacheService { get; } = cacheService;

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
}
