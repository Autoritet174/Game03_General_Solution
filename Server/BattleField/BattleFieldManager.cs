using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.DTO.Battlefield;
using Server.Extensions;
using Server.Hubs;
using Server_DB_Postgres;

namespace Server.Battlefield;

public class BattlefieldManager(Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    ILogger<Client> logger,
    CacheService cacheService)
{
    private bool inCombat = false;

    private SpawnedBattlefield? spawnedBattlefield = null;
    private List<BattlefieldLogRecord> battleLog = [];
    private DateTime dateTimeStartCombat = DateTime.MinValue;
    private int battleLogIndex = 1;

    public const float LEVEL_MULTIPLIER = 1.017f;
    public const int ACTION_POINTS_ON_START = 10;
    public const int INITIATIVES_FOR_ACTION_POINT = 100;
    private int battlefieldTurn = 1;

    public async Task<SpawnedBattlefield?> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (!inCombat)
        {

            // Проверки количества героев для спауна
            if (spawnedHeroesId.Length < 1)
            {
                // Result.Fail("zero heroes");
                return null;
            }

            General.DTO.Entities.GameData.Battlefield battlefield = cacheService.TableBattlefields[eBattleFiled];
            if (spawnedHeroesId.Length > battlefield.MaxHeroCount)
            {
                //return Result.Fail($"too many heroes, max {battlefield.MaxHeroCount}");
                return null;
            }


            // Все герои которые могут сгенерироваться на этом поле боя как ВРАГИ.
            List<X_Battlefield_BaseHero> enemyList = [.. cacheService.TableX_Battlefields_BaseHeroes.Values.Where(x => x.BattlefieldId == eBattleFiled).Select(a => a.Copy())];

            List<SpawnedHero> spawnedHeroesEnemy = [];
            for (int c = 0; c < battlefield.MaxEnemyCount; c++)
            {
                if (enemyList.Count < 1)
                {
                    break;
                }

                List<X_Battlefield_BaseHero> enemies = [.. enemyList.Where(a => a.Count > 0 && a.GuarantSpawn)];
                if (enemies.Count < 1)
                {
                    enemies = [.. enemyList.Where(a => a.Count > 0 && a.ProbabilitySpawn > 0)];
                    if (enemies.Count < 1)
                    {
                        break;
                    }
                }

                X_Battlefield_BaseHero randomEnemy = enemies[Random.Shared.Next(enemies.Count)];
                SpawnedHero sh = SpawnedHeroFactory.CreateFromBaseHero(randomEnemy.BaseHero, 1);
                sh.Team = 2;
                spawnedHeroesEnemy.Add(sh);
                randomEnemy.Count--;

                InitActionPoints(sh);
            }


            DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);


            // спаун героев
            List<SpawnedHero> spawnedHeroesPlayer = [];
            foreach (Hero hero in db.Heroes.Include(a => a.BaseHero).AsNoTracking().Where(a => a.UserId == userId //&& spawnedHeroesId.Contains(a.Id)
            ).OrderBy(a => a.Id).Take(12))
            {
                SpawnedHero sh = SpawnedHeroFactory.CreateFromHero(hero);
                spawnedHeroesPlayer.Add(sh);
                sh.Team = 1;

                InitActionPoints(sh);
            }

            if (spawnedHeroesPlayer.Count < 1)
            {
                // ("zero heroes spawned");
                return null;
            }
            if (spawnedHeroesPlayer.Count > battlefield.MaxHeroCount)
            {
                // ($"too many heroes spawned, max {battlefield.MaxHeroCount}");
                return null;
            }

            spawnedBattlefield = new SpawnedBattlefield(eBattleFiled, spawnedHeroesPlayer, spawnedHeroesEnemy)
            {
                BattlefieldLog = []
            };
            battleLog = spawnedBattlefield.BattlefieldLog;
            dateTimeStartCombat = DateTime.UtcNow;
            battleLogIndex = 1;
            inCombat = true;
        }
        //CombatProcess();
        return spawnedBattlefield;
    }

    public bool CombatBreak()
    {
        inCombat = false;
        spawnedBattlefield = null;
        // battleLog = null; не добавлять эту строку
        return true;
    }


    public async Task<bool> UseAbilityAsync(EBattlefieldLogAbility eAbility, Guid heroSpawnedId, Guid? target)
    {
        if (!inCombat || spawnedBattlefield == null)
        {
            return false;
        }

        if (eAbility != EBattlefieldLogAbility.Attack)
        {
            // тут надо сделать проверку, что если абилка не "атака" то существует ли она у героя
        }

        //var unitTarget





        return true;
    }

    public List<BattlefieldLogRecord>? GetBattleLog()
    {
        if (!inCombat)
        {
            return null;
        }
        CombatProcess();
        List<BattlefieldLogRecord>? log = battleLog;
        battleLog = [];
        return log;
    }

    // === Вспомогательные методы ===

    /// <summary> Инициализировать очки действия по инициативе. </summary>
    private static void InitActionPoints(SpawnedHero sh)
    {
        float initiative = sh.Initiative;//  220.2
        int ap = (int)(initiative / INITIATIVES_FOR_ACTION_POINT);//  220.2/100=2
        initiative -= ap * INITIATIVES_FOR_ACTION_POINT;//  =220.2 - 2*100 = 20.2
        if (initiative > 0 && Random.Shared.NextSingle() * INITIATIVES_FOR_ACTION_POINT < initiative)
        {
            ap++;
        }

        sh.ActionPoints = ap + ACTION_POINTS_ON_START;
    }

    private const int COST_AP_ABILITY_ATTACK = 10;

    private void CombatProcess()
    {
        int teamWinner = 0;
        if (spawnedBattlefield == null)
        {
            return;
        }


        List<SpawnedHero> allHeroesSortedByInitiative = [.. spawnedBattlefield.SpawnedHeroPlayerList.Concat(spawnedBattlefield.SpawnedHeroEnemyList).OrderByDescending(a => a.Initiative)];

        for (battlefieldTurn = 1; battlefieldTurn <= 1000; battlefieldTurn++)
        {
            battleLog.Add(new BattlefieldLogRecord
            {
                Index = battleLogIndex++,
                Action = EBattlefieldLogAction.StartTurn,
                IntValue1 = battlefieldTurn
            });


            // все герои ходят

            for (int i = 0; i < allHeroesSortedByInitiative.Count; i++)
            {
                SpawnedHero hero = allHeroesSortedByInitiative[i];
                if (hero.Health > 0)
                {
                    // Выбираем коллекцию героев противников того героя который сейчас атакует
                    List<SpawnedHero> targets = hero.Team == 1 ? spawnedBattlefield.SpawnedHeroEnemyList : spawnedBattlefield.SpawnedHeroPlayerList;

                    // Выбираем случайного противника
                    SpawnedHero? heroForAttack = targets.Where(a => a.Health > 0).GetRandomElement();

                    if (heroForAttack == null)
                    {
                        teamWinner = hero.Team;
                        // Живого героя для атаки не найдено, значит что в одной из команд все герои мертвы
                        break;
                    }

                    if (hero.ActionPoints >= COST_AP_ABILITY_ATTACK)
                    {
                        UseAbilityAttack(hero, heroForAttack); // атака
                    }
                    else
                    {

                    }

                }

                // Изменяем статус IsAlive всех героев
                //for (int i1 = 0; i1 < allHeroesSortedByInitiative.Count; i1++)
                //{
                //    SpawnedHero h1 = allHeroesSortedByInitiative[i1];
                //    if (h1.Health <= 0)
                //    {
                //        h1.IsAlive = false;
                //    }
                //}
            }

            if (teamWinner > 0)
            {
                break;
            }

            // добавить всем героям по 5-15 АП
            for (int i = 0; i < allHeroesSortedByInitiative.Count; i++)
            {
                SpawnedHero h = allHeroesSortedByInitiative[i];
                if (h.Health > 0)
                {
                    h.ActionPoints += Random.Shared.Next(5, 16);
                }
            }
        }
        _ = CombatBreak();
        //return teamWinner;
    }

    private void UseAbilityAttack(SpawnedHero h1, SpawnedHero h2)
    {
        float damage = h1.Damage;
        bool isCrit = false;
        if (Random.Shared.NextSingle() * 100 < h1.CritChance)
        {
            damage *= (h1.CritMultiplier / 100f) + 1;
            isCrit = true;
        }


        h1.ActionPoints -= COST_AP_ABILITY_ATTACK;
        battleLog.Add(new BattlefieldLogRecord
        {
            Index = battleLogIndex++,
            Action = EBattlefieldLogAction.ChangeActionPoints,
            H1 = h1.SpawnedId,
            IntValue1 = -COST_AP_ABILITY_ATTACK
        });


        h2.Health -= damage;
        battleLog.Add(new BattlefieldLogRecord
        {
            Index = battleLogIndex++,
            Action = EBattlefieldLogAction.Attack,
            H1 = h1.SpawnedId,
            H2 = h2.SpawnedId,
            Ability = EBattlefieldLogAbility.Attack,
            FloatValue1 = damage,
            BoolValue1 = isCrit,
        });

    }
}
