using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.DTO.Battlefield;
using Server.Hubs;
using Server_DB_Postgres;

namespace Server.BattleField;

public class BattleFieldManager(Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    ILogger<Client> logger,
    CacheService cacheService)
{
    private bool inCombat = false;

    private SpawnedBattlefield? spawnedBattlefield = null;
    private DateTime dateTimeStartCombat = DateTime.MinValue;

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

            Battlefield battlefield = cacheService.TableBattlefields[eBattleFiled];
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
                spawnedHeroesEnemy.Add(SpawnedHeroFactory.CreateFromBaseHero(randomEnemy.BaseHero, 1));
            }


            DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);


            // спаун героев
            List<SpawnedHero> spawnedHeroesPlayer = [];
            foreach (Hero hero in db.Heroes.AsNoTracking().Where(a => a.UserId == userId //&& spawnedHeroesId.Contains(a.Id)
            ).OrderBy(a => a.Id).Take(8))
            {
                spawnedHeroesPlayer.Add(SpawnedHeroFactory.CreateFromHero(hero));
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
            //spawnedHeroesEnemy;
            spawnedBattlefield = new SpawnedBattlefield(eBattleFiled, spawnedHeroesPlayer, spawnedHeroesEnemy);
            dateTimeStartCombat = DateTime.UtcNow;
            inCombat = true;
        }

        return spawnedBattlefield;
    }
}
