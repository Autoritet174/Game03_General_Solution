using FluentResults;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.Utilities;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;

namespace Server.Game;

public class LootGenerator(
    ILogger<LootGenerator> logger,
    IDbContextFactory<DbContextGame> dbContextFactory,
    CacheService cacheService)
{
    /// <summary>
    /// Возвращает список id базовых героев, которых уже имеет пользователь, из заданного списка id базовых героев.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, List<int>, IEnumerable<int>> _getUserHeroIdsQuery =
    EF.CompileQuery(
        (DbContextGame db, Guid userId, List<int> baseHeroIds) =>
            db.Heroes
                .Where(h => h.UserId == userId && baseHeroIds.Contains(h.BaseHeroId))
                .Select(h => h.BaseHeroId)
                .Distinct()
    );


    public async Task<Result> GenerateHeroAsync(Guid userId, CancellationToken cancellationToken)
    {
        BaseHero? baseHero = await SelectRandomBaseHeroAsync(userId, cancellationToken).ConfigureAwait(false);
        if (baseHero == null)
        {
            logger.LogError("Failed to select random base hero for user {UserId}", userId);
            return Result.Fail("No base hero selected");
        }

        Result addHeroResult = await AddNewHeroAsync(baseHero, userId, cancellationToken).ConfigureAwait(false);
        if (addHeroResult.IsFailed)
        {
            addHeroResult.Errors.ForEach(e => logger.LogError("Error adding new hero for user {UserId} based on base hero {BaseHeroId}: {ErrorMessage}", userId, baseHero.Id, e.Message));
            return Result.Fail("Failed to add new hero");
        }

        return Result.Ok();
    }

    private async Task<BaseHero?> SelectRandomBaseHeroAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Random rand = Random.Shared;
        int rarity1 = 1000;
        int rarity2 = 250;
        int rarity3 = 0;
        int rarity4 = 0;
        int rarity5 = 0;

        int rarity45 = rarity4 + rarity5;
        int rarity345 = rarity3 + rarity45;
        int rarity2345 = rarity2 + rarity345;

        int raritySumm = rarity1 + rarity2 + rarity3 + rarity4 + rarity5;
        int rarityRandom = rand.Next(0, raritySumm);
        int raritySelected;
        if (rarityRandom < rarity5)
        {
            raritySelected = 5;
        }
        else if (rarityRandom < rarity45)
        {
            raritySelected = 4;
        }
        else if (rarityRandom < rarity345)
        {
            raritySelected = 3;
        }
        else if (rarityRandom < rarity2345)
        {
            raritySelected = 2;
        }
        else
        {
            raritySelected = 1;
        }

        for (int r = raritySelected; r > 0; r--)
        {
            // Получаем id базовых героев с выбранной редкостью, разделяя их на уникальных и неуникальных
            var baseHeroUniqueIds = cacheService.TableBaseHeroes
                    .Where(a => a.Rarity == r && a.IsUnique)
                    .Select(b => b.Id).ToList();
            var baseHeroNotUniqueIds = cacheService.TableBaseHeroes
                    .Where(a => a.Rarity == r && !a.IsUnique)
                    .Select(b => b.Id).ToList();

            // Получаем список id базовых героев с выбранной редкостью, которых уже имеет пользователь, из списка уникальных героев
            var existingIds = _getUserHeroIdsQuery(db, userId, baseHeroUniqueIds).ToList();

            // Получаем список id базовых уникальных героев, которых нет у пользователя
            var notExistingUniqueHeroesId = baseHeroUniqueIds.Except(existingIds).ToList();

            // К списку id базовых героев, которых нет у пользователя, добавляем всех неуникальных героев
            var heroesId = notExistingUniqueHeroesId.Union(baseHeroNotUniqueIds).ToList();

            if (heroesId.Count < 1)
            {
                // Если нет доступных героев с данной редкостью, переходим к редкости ниже уровнем
                continue;
            }

            // Выбираем случайного героя из списка доступных
            int randomIndex = rand.Next(0, heroesId.Count);
            int selectedBaseHeroId = heroesId[randomIndex];
            return cacheService.TableBaseHeroes.FirstOrDefault(b => b.Id == selectedBaseHeroId);
        }

        return null;
    }

    private async Task<Result> AddNewHeroAsync(BaseHero baseHero, Guid userId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Fail("IsCancellationRequested");
        }

        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Hero hero = CreateNewHeroByBase(baseHero);
        hero.UserId = userId;

        _ = await db.Heroes.AddAsync(hero, cancellationToken).ConfigureAwait(false);
        int rowsChanged = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        if (rowsChanged < 1)
        {
            logger.LogError("Failed to add new hero for user {UserId}", userId);
            return Result.Fail("Hero not added");
        }

        return Result.Ok();
    }

    private static Hero CreateNewHeroByBase(BaseHero baseHero)
    {
        return new Hero()
        {
            BaseHeroId = baseHero.Id,
            Level = 1,
            ExperienceNow = 0,
            Health1000 = DiceHelper.GetRandomValue(baseHero.Health1000),
            Strength1000 = DiceHelper.GetRandomValue(baseHero.Strength1000),
            Agility1000 = DiceHelper.GetRandomValue(baseHero.Agility1000),
            Intelligence1000 = DiceHelper.GetRandomValue(baseHero.Intelligence1000),
            CritChance1000 = DiceHelper.GetRandomValue(baseHero.CritChance1000),
            CritPower1000 = DiceHelper.GetRandomValue(baseHero.CritPower1000),
            Haste1000 = DiceHelper.GetRandomValue(baseHero.Haste1000),
            Versality1000 = DiceHelper.GetRandomValue(baseHero.Versality1000),
            EndurancePhysical1000 = DiceHelper.GetRandomValue(baseHero.EndurancePhysical1000),
            EnduranceMagical1000 = DiceHelper.GetRandomValue(baseHero.EnduranceMagical1000),
        };
    }
}
