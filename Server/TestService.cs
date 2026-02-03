using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using System.Threading;

namespace Server;

public class TestService
{
    public async Task MainAsync(DbContextGame db, CancellationToken cancellationToken)
    {
        //019b7d31-93fd-703f-a582-c82e6bd40036
        var userId = Guid.Parse("019b7d31-93fd-703f-a582-c82e6bd40036");
        User user = db.Users.First(u => u.Id == userId);

        //BaseHero Warrior = db.BaseHeroes.First(a => a.Id == 1);
        //Warrior.Health = new General.DTO.Dice(16, 24);
        //Warrior.Damage = new General.DTO.Dice(4, 21);

        //BaseHero Huntress = db.BaseHeroes.First(a => a.Id == 2);
        //Huntress.Health = new General.DTO.Dice(10, 28);
        //Huntress.Damage = new General.DTO.Dice(5, 21);

        //BaseHero Hammerman = db.BaseHeroes.First(a => a.Id == 3);
        //Hammerman.Health = new General.DTO.Dice(11, 39);
        //Hammerman.Damage = new General.DTO.Dice(3, 25);

        //BaseHero Rogue = db.BaseHeroes.First(a => a.Id == 4);
        //Rogue.Health = new General.DTO.Dice(15, 21);
        //Rogue.Damage = new General.DTO.Dice(4, 23);

        //BaseHero Battle_orc = db.BaseHeroes.First(a => a.Id == 5);
        //Battle_orc.Health = new General.DTO.Dice(16, 58);
        //Battle_orc.Damage = new General.DTO.Dice(4, 21);

        //db.SaveChanges();


        //Random rand = new();

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
