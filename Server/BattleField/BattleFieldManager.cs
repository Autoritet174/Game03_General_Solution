using FluentResults;
using General;
using General.DTO.Battlefield;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.Hubs;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.GameData;

namespace Server.BattleField;

public class BattleFieldManager(Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    ILogger<Client> logger,
    CacheService cacheService)
{
    private bool inCombat = false;

    SpawnedBattlefield? spawnedBattlefield = null;
    DateTime dateTimeStartCombat = DateTime.MinValue;

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


            List<X_Battlefield_BaseNpc> npcs = [.. cacheService.TableX_Battlefields_Npcs.Values.Where(x => x.BattlefieldId == eBattleFiled)];
            // тут подразумевается что лист npcs уже содержит всех нпс которых нужно заспавнить


            DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);


            // спаун героев
            List<SpawnedHero> spawnedHeroes = await db.Heroes.AsNoTracking().Where(a => a.UserId == userId && spawnedHeroesId.Contains(a.Id)).Select(a => new SpawnedHero(a.Id, Guid.CreateVersion7())).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (spawnedHeroes.Count < 1)
            {
                //return Result.Fail("zero heroes spawned");
                return null;
            }
            if (spawnedHeroes.Count > 8)
            {
                // return Result.Fail("too many heroes spawned, max 8");
                return null;
            }


            // спаун врагов
            List<SpawnedNpc> spawnedNpcs = [];
            spawnedNpcs.AddRange(npcs.Select(i => new SpawnedNpc(i.BaseNpcId, Guid.CreateVersion7())));


            spawnedBattlefield = new SpawnedBattlefield(eBattleFiled, spawnedHeroes, spawnedNpcs);
            dateTimeStartCombat = DateTime.UtcNow;
            inCombat = true;
        }

        return spawnedBattlefield;
    }
}
