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
        Dungeon? dungeon = cacheService.TableDungeons.FirstOrDefault(d => d.Id == id);
        if (dungeon == null)
        {
            return false;
        }



        inCombat = true;
        return true;
    }
}
