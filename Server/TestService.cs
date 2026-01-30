using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using General;

namespace Server;

public class TestService
{
    public async Task MainAsync(DbContextGame db, CancellationToken cancellationToken)
    {
        //019b7d31-93fd-703f-a582-c82e6bd40036
        var userId = Guid.Parse("019b7d31-93fd-703f-a582-c82e6bd40036");
        User user = db.Users.First(u => u.Id == userId);
        Random rand = new();

        //var list = db.BaseEquipments.ToList();
        //for (int i = 0; i < 100; i++)
        //{
        //    BaseEquipment baseEquipment = list[rand.Next(list.Count)];

        //    _ = await db.Equipments.AddAsync(new Equipment()
        //    {
        //        UserId = userId,
        //        User = user,
        //        BaseEquipmentId = baseEquipment.Id,
        //        BaseEquipment = baseEquipment
        //    }, cancellationToken);
        //}

        //int v = db.Heroes.ExecuteDelete();
        //_ = await db.SaveChangesAsync(cancellationToken);

        //var list = db.BaseHeroes.ToList();
        //for (int i = 0; i < 222; i++)
        //{
        //    var BaseHeroes = list[rand.Next(list.Count)];

        //    _ = await db.Heroes.AddAsync(new Hero()
        //    {
        //        UserId = userId,
        //        BaseHeroId = BaseHeroes.Id,
        //        Rarity = rand.Next(1, 7),
        //    }, cancellationToken);
        //}

        //_ = await db.SaveChangesAsync(cancellationToken);
    }

}
