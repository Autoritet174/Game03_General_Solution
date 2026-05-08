using General;
using General.DTO.Battlefield;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
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
            Battlefield battlefield = cacheService.TableBattlefields[eBattleFiled];

            // Проверки количества героев для спауна
            if (spawnedHeroesId.Length < 1)
            {
                // Result.Fail("zero heroes");
                return null;
            }

            if (spawnedHeroesId.Length > 8)
            {
                //return Result.Fail("too many heroes, max 8");
                return null;
            }


            List<X_Battlefield_BaseHero> npcs = [.. cacheService.TableX_Battlefields_BaseHeroes.Values.Where(x => x.BattlefieldId == eBattleFiled)];
            List<SpawnedHero> spawnedHeroesEnemy = [];
            foreach (var i in spawnedHeroesEnemy)
            {
                //DtoHero hero = new DtoHero();
                //spawnedHeroesEnemy.Add(new SpawnedHero());
            }
            // тут подразумевается что лист npcs уже содержит всех нпс которых нужно заспавнить


            DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);


            // спаун героев
            List<SpawnedHero> spawnedHeroesPlayer = [];
            foreach (Hero? hero in db.Heroes.AsNoTracking().Where(a => a.UserId == userId //&& spawnedHeroesId.Contains(a.Id)
            ).Take(8))
            {
                //spawnedHeroesPlayer.Add(new SpawnedHero(hero.Id, Guid.CreateVersion7()));
            }

            if (spawnedHeroesPlayer.Count < 1)
            {
                //return Result.Fail("zero heroes spawned");
                return null;
            }
            if (spawnedHeroesPlayer.Count > 8)
            {
                // return Result.Fail("too many heroes spawned, max 8");
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
