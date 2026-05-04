using General;
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

    public bool CombatStart(EBattleFiled eBattleFiled, CancellationToken cancellationToken) {
        if (inCombat)
        {
            return false;
        }
        int id = (int)eBattleFiled;
        Battlefield dungeon = cacheService.TableBattlefields[id];
        List<X_Battlefield_BaseNpc> npcs = [.. cacheService.TableX_Battlefields_Npcs.Values.Where(x => x.BattlefieldId == id)];


        inCombat = true;
        return true;
    }
}
