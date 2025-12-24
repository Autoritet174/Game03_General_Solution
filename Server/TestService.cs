using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.GameData;

namespace Server;

public interface ITestService
{
    Task Main(DbContext_Game db, CancellationToken cancellationToken = default);
}
public class TestService : ITestService
{
    public async Task Main(DbContext_Game db, CancellationToken cancellationToken = default)
    {
        //foreach (EquipmentType? item in await db.EquipmentTypes.ToListAsync(cancellationToken))
        //{
        //    item.Damage = new General.DTO.Dice(item.AttackOld);
        //}
        //_ = await db.SaveChangesAsync();
    }

}
