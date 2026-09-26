using General.DTO.Battlefield;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.Battlefield.Abilities;
using Server.DTO.Battlefield;
using Server.Extensions;
using Server_DB_Postgres;
using BattlefieldDefinition = General.DTO.Entities.GameData.Battlefield;
using Server.BattleField;

namespace Server.Battlefield;

/// <summary>Создаёт команды участников, рассчитывает бой и формирует последовательный журнал событий.</summary>
public class BattlefieldManager(Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    //ILogger<Hubs.Client> logger,
    CacheService cacheService)
{
    public const float LEVEL_MULTIPLIER = 1.017f;
    public const int ACTION_POINTS_ON_START = 10;
    public const int INITIATIVES_FOR_ACTION_POINT = 100;

    private const int MAX_COMBAT_TURNS = 1000;

    // Порядок задаёт приоритет: герой выполняет первую доступную способность.
    private readonly IBattleAbility[] abilities = [new Healing(), new Attack()];

    private bool inCombat;
    private SpawnedBattlefield? spawnedBattlefield;
    private List<BattlefieldLogRecordBase> battleLog = [];

    #region Управление боем

    /// <summary>Создаёт новый бой с выбранными героями либо возвращает уже запущенный бой.</summary>
    /// <returns>Начальное состояние боя или null при недопустимом количестве героев.</returns>
    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (inCombat)
        {
            return spawnedBattlefield;
        }

        if (spawnedHeroesId.Length < 1)
        {
            return null;
        }

        BattlefieldDefinition battlefield = cacheService.TableBattlefields[eBattleFiled];
        if (spawnedHeroesId.Length > battlefield.maxHeroCount)
        {
            return null;
        }

        List<SpawnedHero> spawnedHeroesEnemy = CreateEnemyHeroes(eBattleFiled, battlefield.maxEnemyCount);
        List<SpawnedHero> spawnedHeroesPlayer = await CreatePlayerHeroesAsync(spawnedHeroesId, cancellationToken).ConfigureAwait(false);

        if (spawnedHeroesPlayer.Count < 1 || spawnedHeroesPlayer.Count > battlefield.maxHeroCount)
        {
            return null;
        }

        spawnedBattlefield = new(eBattleFiled, spawnedHeroesPlayer, spawnedHeroesEnemy)
        {
            battlefieldLog = []
        };
        battleLog = spawnedBattlefield.battlefieldLog;
        inCombat = true;

        return spawnedBattlefield;
    }

    /// <summary>Завершает бой, сохраняя накопленный журнал для последующей выдачи клиенту.</summary>
    public bool CombatBreak()
    {
        inCombat = false;
        spawnedBattlefield = null;
        return true;
    }

    /// <summary>Проверяет наличие активного боя; ручное применение способности пока не реализовано.</summary>
    public Task<bool> UseAbilityAsync(EBattlefieldLogAbility eAbility, Guid heroSpawnedId, Guid? target)
    {
        return Task.FromResult(inCombat && spawnedBattlefield != null);
    }

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
        return log;
    }

    #endregion Управление боем

    #region Подготовка участников

    /// <summary>Создаёт противников, выбирая сначала гарантированные записи с оставшимся количеством.</summary>
    private List<SpawnedHero> CreateEnemyHeroes(EBattleFiled battlefieldId, int maxEnemyCount)
    {
        List<X_Battlefield_BaseHero> enemyPool =
        [
            .. cacheService.TableX_Battlefields_BaseHeroes.Values
                .Where(enemy => enemy.battlefieldId == battlefieldId)
                .Select(enemy => enemy.Copy())
        ];

        List<SpawnedHero> spawnedHeroes = [];
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
            SpawnedHero hero = SpawnedHeroFactory.CreateFromBaseHero(selectedEnemy.baseHero, 1);
            hero.team = 2;
            spawnedHeroes.Add(hero);
            selectedEnemy.count--;

            InitActionPoints(hero);
        }

        return spawnedHeroes;
    }

    /// <summary>Загружает принадлежащих игроку героев с экипировкой и создаёт их боевые представления.</summary>
    private async Task<List<SpawnedHero>> CreatePlayerHeroesAsync(Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        var heroesWithEquipment = db.Heroes
            .AsNoTracking()
            .Where(hero => hero.userId == userId && spawnedHeroesId.Contains(hero.id))
            .Select(hero => new
            {
                hero,
                items = db.Equipments.Where(equipment => equipment.heroId == hero.id).ToList()
            })
            .ToList();

        List<SpawnedHero> spawnedHeroes = [];
        foreach (var row in heroesWithEquipment)
        {
            SpawnedHero hero = SpawnedHeroFactory.CreateFromHero(row.hero, row.items);
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

        List<SpawnedHero> heroesByInitiative =
        [
            .. spawnedBattlefield.spawnedHeroPlayerList
                .Concat(spawnedBattlefield.spawnedHeroEnemyList)
                .OrderByDescending(hero => hero.initiative)
        ];

        BattleAbilityContext context = new(heroesByInitiative.AsReadOnly(), battleLog);

        for (int turn = 1; turn <= MAX_COMBAT_TURNS; turn++)
        {
            context.AddLog(new BattlefieldLogRecord_TurnStart
            {
                turn = turn
            });

            if (ProcessTurn(context))
            {
                break;
            }

            RestoreActionPoints(heroesByInitiative);
        }

        _ = CombatBreak();
    }

    /// <summary>Перебирает способности героев в порядке приоритета до первого успешного применения.</summary>
    /// <returns>True, если у очередного живого героя больше нет живых противников.</returns>
    private bool ProcessTurn(BattleAbilityContext context)
    {
        foreach (SpawnedHero hero in context.heroes)
        {
            if (hero.health is not > 0)
            {
                continue;
            }

            // Это условие завершения боя; допустимые цели определяет сама способность.
            if (!context.heroes.Any(other => other.health > 0 && other.team != hero.team))
            {
                return true;
            }

            foreach (IBattleAbility ability in abilities)
            {
                if (ability.TryUse(hero, context))
                {
                    break;
                }
            }
        }

        return false;
    }

    /// <summary>В конце незавершённого хода начисляет каждому живому герою от 5 до 15 очков действия.</summary>
    private static void RestoreActionPoints(List<SpawnedHero> heroes)
    {
        foreach (SpawnedHero hero in heroes)
        {
            if (hero.health > 0)
            {
                hero.actionPoints += Random.Shared.Next(5, 16);
            }
        }
    }

    #endregion Расчёт боя
}
