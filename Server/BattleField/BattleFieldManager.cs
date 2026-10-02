using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.BattleField;
using Server.Cache;
using Server.DTO.Battlefield;
using Server_DB_Postgres;
using BattlefieldDefinition = General.DTO.Entities.GameData.Battlefield;

namespace Server.Battlefield;

/// <summary>Создаёт команды участников, рассчитывает бой и формирует последовательный журнал событий.</summary>
public class BattlefieldManager(Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    ILogger<Hubs.Client> logger,
    CacheService cacheService)
{
    public const float LEVEL_MULTIPLIER = 1.017f;
    public const int ACTION_POINTS_ON_START = 10;
    public const int INITIATIVES_FOR_ACTION_POINT = 100;

    private const int MAX_COMBAT_TURNS = 1000;

    private bool inCombat;
    private SpawnedBattlefield? spawnedBattlefield;
    private List<BattlefieldLogRecordBase> battleLog = [];

    #region Управление боем

    /// <summary>Создаёт новый бой с выбранными героями либо возвращает уже запущенный бой.</summary>
    /// <returns>Начальное состояние боя или null при недопустимом количестве героев.</returns>
    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattlefield eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (inCombat)
        {
            return spawnedBattlefield;
        }

        if (spawnedHeroesId.Length < 1)
        {
            return null;
        }

        BattlefieldDefinition battlefield = cacheService.tableBattlefields[eBattleFiled];
        if (spawnedHeroesId.Length > battlefield.maxHeroCount)
        {
            return null;
        }

        List<ServerSpawnedHero> spawnedHeroesEnemy = CreateEnemyHeroes(eBattleFiled, battlefield.maxEnemyCount);
        List<ServerSpawnedHero> spawnedHeroesPlayer = await CreatePlayerHeroesAsync(spawnedHeroesId, cancellationToken).ConfigureAwait(false);

        if (spawnedHeroesPlayer.Count < 1 || spawnedHeroesPlayer.Count > battlefield.maxHeroCount)
        {
            return null;
        }

        spawnedBattlefield = new(eBattleFiled, [.. spawnedHeroesPlayer], [.. spawnedHeroesEnemy])
        {
            battlefieldLog = []
        };
        battleLog = spawnedBattlefield.battlefieldLog;
        inCombat = true;
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Combat started: {BattlefieldId}, players: {PlayerCount}, enemies: {EnemyCount}",
            eBattleFiled, spawnedHeroesPlayer.Count, spawnedHeroesEnemy.Count);
        }

        return spawnedBattlefield;
    }

    /// <summary>Завершает бой, сохраняя накопленный журнал для последующей выдачи клиенту.</summary>
    public bool CombatBreak()
    {
        inCombat = false;
        spawnedBattlefield = null;
        return true;
    }

    ///// <summary>Проверяет наличие активного боя; ручное применение способности пока не реализовано.</summary>
    //public Task<bool> UseAbilityAsync(EBattlefieldLogAbility eAbility, Guid heroSpawnedId, Guid? target)
    //{
    //    return Task.FromResult(inCombat && spawnedBattlefield != null);
    //}

    /// <summary>Рассчитывает активный бой и передаёт накопленный журнал, очищая буфер менеджера.</summary>
    public List<BattlefieldLogRecordBase> GetBattleLog()
    {
        if (!inCombat)
        {
            return [];
        }

        CombatProcess();

        List<BattlefieldLogRecordBase> log = battleLog;
        battleLog = [];
        File.WriteAllText(@"C:\UnityProjects\Game03_Git\Client_Game03\TestData\battle_log.json", JSON.Serialize(log));
        return log;
    }

    #endregion Управление боем

    #region Подготовка участников

    /// <summary>Создаёт противников, выбирая сначала гарантированные записи с оставшимся количеством.</summary>
    private List<ServerSpawnedHero> CreateEnemyHeroes(EBattlefield battlefieldId, int maxEnemyCount)
    {
        List<X_Battlefield_BaseHero> enemyPool =
        [
            .. cacheService.tableX_Battlefields_BaseHeroes.Values
                .Where(enemy => enemy.battlefieldId == battlefieldId)
                .Select(enemy => enemy.Copy())
        ];

        List<ServerSpawnedHero> spawnedHeroes = [];
        for (int i = 0; i < maxEnemyCount; i++)
        {
            if (enemyPool.Count < 1)
            {
                break;
            }

            List<X_Battlefield_BaseHero> candidates = [.. enemyPool.Where(enemy => enemy.count > 0 && enemy.guarantSpawn)];
            if (candidates.Count < 1)
            {
                candidates = [.. enemyPool.Where(enemy => enemy.count > 0 && enemy.probabilitySpawn > 0)];
                if (candidates.Count < 1)
                {
                    break;
                }
            }

            X_Battlefield_BaseHero selectedEnemy = candidates[Random.Shared.Next(candidates.Count)];
            ServerSpawnedHero hero = SpawnedHeroFactory.CreateFromBaseHero(selectedEnemy.baseHero, 1);
            hero.team = 2;
            spawnedHeroes.Add(hero);
            selectedEnemy.count--;

            InitActionPoints(hero);
        }

        return spawnedHeroes;
    }

    /// <summary>Загружает принадлежащих игроку героев с экипировкой и создаёт их боевые представления.</summary>
    private async Task<List<ServerSpawnedHero>> CreatePlayerHeroesAsync(Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        var heroesWithEquipment = db.heroes
            .AsNoTracking()
            .Where(hero => hero.userId == userId && spawnedHeroesId.Contains(hero.id))
            .Select(hero => new
            {
                hero,
                items = db.equipments.Where(equipment => equipment.heroId == hero.id).ToList()
            })
            .ToList();

        List<ServerSpawnedHero> spawnedHeroes = [];
        foreach (var row in heroesWithEquipment)
        {
            ServerSpawnedHero hero = SpawnedHeroFactory.CreateFromHero(row.hero, row.items, cacheService.tableBaseHeroes[row.hero.baseHeroId]);
            spawnedHeroes.Add(hero);
            hero.team = 1;
            InitActionPoints(hero);
        }

        return spawnedHeroes;
    }

    /// <summary>Начисляет стартовые очки действия с вероятностным округлением дробного вклада инициативы.</summary>
    private static void InitActionPoints(SpawnedHero hero)
    {
        float remainingInitiative = hero.initiative;
        int actionPoints = (int)(remainingInitiative / INITIATIVES_FOR_ACTION_POINT);
        remainingInitiative -= actionPoints * INITIATIVES_FOR_ACTION_POINT;

        if (remainingInitiative > 0 && Random.Shared.NextSingle() * INITIATIVES_FOR_ACTION_POINT < remainingInitiative)
        {
            actionPoints++;
        }

        hero.actionPoints = actionPoints + ACTION_POINTS_ON_START;
    }

    #endregion Подготовка участников

    #region Расчёт боя

    /// <summary>Разыгрывает ходы до обнаружения победителя или достижения предельного числа ходов.</summary>
    private void CombatProcess()
    {
        if (spawnedBattlefield == null)
        {
            return;
        }

        List<ServerSpawnedHero> heroesByInitiative =
        [
            .. spawnedBattlefield.spawnedHeroPlayerList
                .Concat(spawnedBattlefield.spawnedHeroEnemyList)
                .Cast<ServerSpawnedHero>()
                .OrderByDescending(hero => hero.initiative)
        ];

        BattleAbilityContext context = new(heroesByInitiative.AsReadOnly(), battleLog, cacheService);

        for (int turn = 1; turn <= MAX_COMBAT_TURNS; turn++)
        {
            // Кулдаун 2 после применения на ходу 1 истекает в начале хода 3.
            foreach (ServerSpawnedHero hero in context.heroes)
            {
                foreach (BattleAbility ability in hero.abilities)
                {
                    ability.ReduceCooldown();
                }
            }

            _ = context.AddLog(new BattlefieldLogRecord_TurnStart
            {
                turn = turn
            });

            if (ProcessTurn(context))
            {
                break;
            }

            RestoreActionPoints(context);
        }

        _ = CombatBreak();
    }

    /// <summary>Применяет наиболее эффективные доступные способности каждого героя, пока это возможно.</summary>
    /// <returns>True, если у очередного живого героя больше нет живых противников.</returns>
    private static bool ProcessTurn(BattleAbilityContext context)
    {
        foreach (ServerSpawnedHero hero in context.heroes)
        {
            if (hero.health <= 0)
            {
                continue;
            }

            // условие завершения боя
            if (!context.heroes.Any(other => other.health > 0 && other.team != hero.team))
            {
                return true;
            }

            bool abilityUsed = true;
            while (abilityUsed)
            {
                abilityUsed = false;

                foreach (BattleAbility ability in hero.abilities)
                {
                    ability.CalcEffectiveness(hero, context);
                }

                BattleAbility? abilityWithMaxEffectiveness = hero.abilities.MaxBy(a => a.effectiveness);
                if (abilityWithMaxEffectiveness != null && abilityWithMaxEffectiveness.effectiveness > 0f && abilityWithMaxEffectiveness.UseAbility(hero, context))
                {
                    abilityUsed = true;
                }
            }
        }

        return false;
    }

    /// <summary>В конце незавершённого хода начисляет каждому живому герою от 5 до 15 очков действия.</summary>
    private static void RestoreActionPoints(BattleAbilityContext context)
    {
        foreach (SpawnedHero hero in context.heroes)
        {
            if (hero.health > 0)
            {
                int ap = Random.Shared.Next(5, 16);
                context.ChangeActionPoints(hero, ap);
            }
        }
    }

    #endregion Расчёт боя
}
