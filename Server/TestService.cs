using FluentResults;
using General;
using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server.Game;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using System.Threading;

namespace Server;

public class TestService(LootGenerator lootGenerator)
{
    /// <summary>
 /// Скомпилированный запрос для получения первой записи Equipment.
 /// Позволяет избежать повторного парсинга LINQ-выражения.
 /// </summary>
    private static readonly Func<DbContextGame, CancellationToken, Task<Equipment?>> GetFirstEquipmentCompiled =
        EF.CompileAsyncQuery((DbContextGame db, CancellationToken ct) =>
            db.Equipments.FirstOrDefault());
    /// <summary>
    /// Выводит текущее состояние сущности в консоль.
    /// </summary>
    /// <param name="stage">Описание этапа теста.</param>
    /// <param name="state">Текущее состояние EntityState.</param>
    private static void PrintState(string stage, EntityState state)
    {
        // Используем интерполяцию строк (в C# 13 оптимизирована компилятором)
        Console.WriteLine($"[TEST] {stage}: {state}");
    }

    public async Task MainAsync(DbContextGame db, Cache.CacheService cacheService, CancellationToken cancellationToken)
    {
        //019b7d31-93fd-703f-a582-c82e6bd40036
        var userId = Guid.Parse("019b7d31-93fd-703f-a582-c82e6bd40036");
        //User user = db.Users.First(u => u.Id == userId);
        for (int i = 0; i < 5; i++)
        {
            //await lootGenerator.GenerateHeroAsync(userId, 5, 5, cancellationToken).ConfigureAwait(false);
            // await lootGenerator.GenerateEquipmentAsync(userId, General.ESlotType.None, 1, 4, cancellationToken).ConfigureAwait(false);

            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Silver bracelet"), userId, cancellationToken).ConfigureAwait(false);
            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron boots"), userId, cancellationToken).ConfigureAwait(false);
            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron helmet"), userId, cancellationToken).ConfigureAwait(false);
            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron gloves"), userId, cancellationToken).ConfigureAwait(false);
            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron armor"), userId, cancellationToken).ConfigureAwait(false);
            _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron sword"), userId, cancellationToken).ConfigureAwait(false);
        }
        _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Thunderfury"), userId, cancellationToken).ConfigureAwait(false);

        //cacheService.TableBaseEquipments.First(a => a.Name == "Iron sword");
        _ = await lootGenerator.AddNewEquipmentAsync(db, cacheService.TableBaseEquipments.First(a => a.Name == "Iron sword"), userId, cancellationToken).ConfigureAwait(false);




        // характеристики при выпадении предметов
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Доспех");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Health, new Dice(25, 7));
            eq.PossibleStats.Add(EStatType.Versality, new Dice(7, 3));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(2, 2));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(6, 4));
        }
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Шлем");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Health, new Dice(3, 19));
            eq.PossibleStats.Add(EStatType.Versality, new Dice(5, 3));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(2, 2));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(6, 4));
        }
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Руки");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Health, new Dice(5, 9));
            eq.PossibleStats.Add(EStatType.Versality, new Dice(5, 3));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(2, 2));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(6, 4));
        }
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Сапоги");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Versality, new Dice(5, 3));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(2, 2));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(6, 4));
        }
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Браслет");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Health, new Dice(3, 9));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(4, 4));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(3, 19));
            eq.PossibleStats.Add(EStatType.Intelligence, new Dice(3, 17));
            eq.PossibleStats.Add(EStatType.Initiative, new Dice(3, 11));
            eq.PossibleStats.Add(EStatType.Haste, new Dice(3, 11));
            eq.PossibleStats.Add(EStatType.Versality, new Dice(5, 3));
        }
        {
            EquipmentType eq = db.EquipmentTypes.First(a => a.NameRu == "Меч");
            eq.PossibleStats = [];
            eq.PossibleStats.Add(EStatType.Health, new Dice(3, 9));
            eq.PossibleStats.Add(EStatType.Versality, new Dice(5, 3));
            eq.PossibleStats.Add(EStatType.CritChance, new Dice(2, 2));
            eq.PossibleStats.Add(EStatType.CritMultiplier, new Dice(6, 4));
            eq.PossibleStats.Add(EStatType.Damage, new Dice(4, 4));
        }
        db.SaveChanges();
    }

}
