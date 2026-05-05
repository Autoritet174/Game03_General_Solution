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

    private readonly List<SpawnedHero> spawnedHeroes = [];
    private readonly List<SpawnedNpc> spawnedNpcs = [];

    public async Task<Result> CombatStartAsync(EBattleFiled eBattleFiled, Guid[] spawnedHeroesId, CancellationToken cancellationToken)
    {
        if (inCombat)
        {
            return Result.Fail("inCombat");
        }
        int id = (int)eBattleFiled;
        Battlefield battlefield = cacheService.TableBattlefields[id];

        // Проверки количества героев для спауна
        // массив spawnedHeroesId присылает пользователь, а пользователю нельзя доверять
        if (spawnedHeroesId.Length < 1)
        {
            return Result.Fail("zero heroes");
        }

        if (spawnedHeroesId.Length > 8)
        {
            return Result.Fail("too many heroes, max 8");
        }


        List<X_Battlefield_BaseNpc> npcs = [.. cacheService.TableX_Battlefields_Npcs.Values.Where(x => x.BattlefieldId == id)];

        spawnedNpcs.Clear();
        spawnedNpcs.AddRange(npcs.Select(i => new SpawnedNpc(i.BaseNpcId, Guid.CreateVersion7())));

        DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        spawnedHeroes.Clear();
        List<SpawnedHero> spawnedHeroesTemp = await db.Heroes.AsNoTracking().Where(a => a.UserId == userId && spawnedHeroesId.Contains(a.Id)).Select(a => new SpawnedHero(a.Id)).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (spawnedHeroesTemp.Count< 1)
        {
            return Result.Fail("zero heroes spawned");
        }
        if (spawnedHeroesTemp.Count > 8)
        {
            return Result.Fail("too many heroes spawned, max 8");
        }
        spawnedHeroes.AddRange(spawnedHeroesTemp);

        inCombat = true;
        return Result.Ok();
    }
}
