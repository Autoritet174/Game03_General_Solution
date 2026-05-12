using FluentResults;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.DTO.Collection;
using Server_DB_Postgres;

namespace Server.Game;

public class LootGenerator(
    ILogger<LootGenerator> logger,
    IDbContextFactory<DbContextGame> dbContextFactory,
    CacheService cacheService)
{
    // ПАРАМЕТРЫ ГЕНЕРАТОРА
    private static readonly int rarity1 = 3125, rarity2 = 625, rarity3 = 125, rarity4 = 25, rarity5 = 5, rarity6 = 1;



    /// <summary>
    /// Возвращает список id базовых героев, которых уже имеет пользователь, из заданного списка id базовых героев.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, List<int>, IEnumerable<int>> _getUserHeroIdsQuery =
    EF.CompileQuery((DbContextGame db, Guid userId, List<int> baseHeroIds) =>
            db.Heroes.Where(h => h.UserId == userId && baseHeroIds.Contains(h.BaseHeroId))
                .Select(h => h.BaseHeroId).Distinct());

    /// <summary>
    /// Возвращает список id базовых предметов, которых уже имеет пользователь, из заданного списка id базовых предметов.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, List<int>, IEnumerable<int>> _getUserEquipmentIdsQuery =
    EF.CompileQuery((DbContextGame db, Guid userId, List<int> baseEquipmentIds) =>
            db.Equipments.Where(h => h.UserId == userId && baseEquipmentIds.Contains(h.BaseEquipmentId))
                .Select(h => h.BaseEquipmentId).Distinct());



    private static int SelectRandomRarity(int minRarity = 1, int maxRarity = 6)
    {
        if (minRarity > maxRarity)
        {
            throw new ArgumentException("minRarity cannot be greater than maxRarity");
        }

        int rarity1 = LootGenerator.rarity1,
            rarity2 = LootGenerator.rarity2,
            rarity3 = LootGenerator.rarity3,
            rarity4 = LootGenerator.rarity4,
            rarity5 = LootGenerator.rarity5,
            rarity6 = LootGenerator.rarity6;
        if (minRarity > 1)
        {
            rarity1 = 0;
        }
        if (minRarity > 2)
        {
            rarity2 = 0;
        }
        if (minRarity > 3)
        {
            rarity3 = 0;
        }
        if (minRarity > 4)
        {
            rarity4 = 0;
        }
        if (minRarity > 5)
        {
            rarity5 = 0;
        }
        if (maxRarity < 6)
        {
            rarity6 = 0;
        }
        if (maxRarity < 5)
        {
            rarity5 = 0;
        }
        if (maxRarity < 4)
        {
            rarity4 = 0;
        }
        if (maxRarity < 3)
        {
            rarity3 = 0;
        }
        if (maxRarity < 2)
        {
            rarity2 = 0;
        }

        int raritySumFrom5 = rarity5 + rarity6;
        int raritySumFrom4 = rarity4 + raritySumFrom5;
        int raritySumFrom3 = rarity3 + raritySumFrom4;
        int raritySumFrom2 = rarity2 + raritySumFrom3;

        int raritySumm = rarity1 + raritySumFrom2;
        int rarityRandom = Random.Shared.Next(0, raritySumm);
        return rarityRandom < rarity6 ? 6
            : rarityRandom < raritySumFrom5 ? 5
            : rarityRandom < raritySumFrom4 ? 4
            : rarityRandom < raritySumFrom3 ? 3
            : rarityRandom < raritySumFrom2 ? 2 : 1;
    }

    /// <summary>
    /// Генерирует нового героя для пользователя, выбирая случайного базового героя с учетом редкости и наличия уникальных героев, которых пользователь уже имеет. Добавляет нового героя в базу данных и возвращает результат операции.
    /// </summary>
    public async Task<Result> GenerateHeroAsync(Guid userId, int minRarity = 1, int maxRarity = 6, CancellationToken cancellationToken = default)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        int raritySelected = SelectRandomRarity(minRarity, maxRarity);
        BaseHero? baseHero = await SelectRandomBaseHeroAsync(db, userId, raritySelected, cancellationToken).ConfigureAwait(false);
        if (baseHero == null)
        {
            logger.LogError("Failed to select random base hero for user {UserId}", userId);
            return Result.Fail("No base hero selected");
        }
        Result addHeroResult = await AddNewHeroAsync(db, baseHero, userId, cancellationToken).ConfigureAwait(false);
        if (addHeroResult.IsFailed)
        {
            addHeroResult.Errors.ForEach(e => logger.LogError("Error adding new hero for user {UserId} based on base hero {BaseHeroId}: {ErrorMessage}", userId, baseHero.Id, e.Message));
            return Result.Fail("Failed to add new hero");
        }
        return Result.Ok();
    }

    /// <summary>
    /// Генерирует новую экипировку для пользователя, выбирая случайный базовый предмет с учетом редкости и наличия уникальных предметов, которых пользователь уже имеет. Добавляет новый предмет в базу данных и возвращает результат операции.
    /// </summary>
    /// <returns></returns>
    public async Task<Result> GenerateEquipmentAsync(Guid userId, ESlotType slotTypeId, int minRarity = 1, int maxRarity = 6, CancellationToken cancellationToken = default)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        int raritySelected = SelectRandomRarity(minRarity, maxRarity);
        BaseEquipment? baseEquipment = await SelectRandomBaseEquipmentAsync(db, userId, slotTypeId, raritySelected, cancellationToken).ConfigureAwait(false);
        if (baseEquipment == null)
        {
            logger.LogError("Failed to select random base equipment for user {UserId}", userId);
            return Result.Fail("No base equipment selected");
        }
        Result addEquipmentResult = await AddNewEquipmentAsync(db, baseEquipment, userId, cancellationToken).ConfigureAwait(false);
        if (addEquipmentResult.IsFailed)
        {
            addEquipmentResult.Errors.ForEach(e => logger.LogError("Error adding new equipment for user {UserId} based on base equipment {BaseEquipmentId}: {ErrorMessage}", userId, baseEquipment.Id, e.Message));
            return Result.Fail("Failed to add new equipment");
        }
        return Result.Ok();
    }

    private async Task<BaseHero?> SelectRandomBaseHeroAsync(DbContextGame db, Guid userId, int raritySelected, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }


        for (int r = raritySelected; r > 0; r--)
        {
            // Получаем id базовых героев с выбранной редкостью, разделяя их на уникальных и неуникальных
            List<int> baseHeroUniqueIds = [.. cacheService.TableBaseHeroesOnlyPlayable.Values
                    .Where(a => a.Rarity == r && a.IsUnique)
                    .Select(b => b.Id)];
            List<int> baseHeroNotUniqueIds = [.. cacheService.TableBaseHeroesOnlyPlayable.Values
                    .Where(a => a.Rarity == r && !a.IsUnique)
                    .Select(b => b.Id)];

            // Получаем список id базовых героев с выбранной редкостью, которых уже имеет пользователь, из списка уникальных героев
            List<int> existingUniqueIds = [.. _getUserHeroIdsQuery(db, userId, baseHeroUniqueIds)];

            // Получаем список id базовых уникальных героев, которых нет у пользователя
            List<int> notExistingUniqueHeroesId = [.. baseHeroUniqueIds.Except(existingUniqueIds)];

            // К списку id базовых героев, которых нет у пользователя, добавляем всех неуникальных героев
            List<int> heroesId = [.. notExistingUniqueHeroesId.Union(baseHeroNotUniqueIds)];

            if (heroesId.Count < 1)
            {
                continue; // Если нет доступных героев с данной редкостью, переходим к редкости ниже уровнем
                // фактически это заглушка, так как герои на всех редкостях должны быть доступны хотябы один
            }

            // Выбираем случайного героя из списка доступных
            int randomIndex = Random.Shared.Next(heroesId.Count);
            int selectedBaseHeroId = heroesId[randomIndex];
            return cacheService.TableBaseHeroesOnlyPlayable[selectedBaseHeroId];
        }

        return null;
    }

    private async Task<BaseEquipment?> SelectRandomBaseEquipmentAsync(DbContextGame db, Guid userId, ESlotType slotTypeId, int raritySelected, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        for (int iR = raritySelected; iR > 0; iR--)
        {
            int r = iR;
            // Получаем id базовых предметов с выбранной редкостью, разделяя их на уникальных и неуникальных
            List<int> baseEquipmentUniqueIds = [.. cacheService.TableBaseEquipments.Values
                    .Where(a => a.Rarity == r && a.IsUnique && (slotTypeId == ESlotType.None || a.EquipmentType.SlotTypeId == slotTypeId))
                    .Select(b => b.Id)];
            List<int> baseEquipmentNotUniqueIds = [.. cacheService.TableBaseEquipments.Values
                    .Where(a => a.Rarity == r && !a.IsUnique && (slotTypeId == ESlotType.None || a.EquipmentType.SlotTypeId == slotTypeId))
                    .Select(b => b.Id)];

            // Получаем список id базовых предметов с выбранной редкостью, которых уже имеет пользователь, из списка уникальных предметов
            List<int> existingUniqueIds = [.. _getUserEquipmentIdsQuery(db, userId, baseEquipmentUniqueIds)];

            // Получаем список id базовых уникальных предметов, которых нет у пользователя
            List<int> notExistingUniqueEquipmentsId = [.. baseEquipmentUniqueIds.Except(existingUniqueIds)];

            // К списку id базовых предметов, которых нет у пользователя, добавляем все неуникальные предметы
            List<int> EquipmentsId = [.. notExistingUniqueEquipmentsId.Union(baseEquipmentNotUniqueIds)];

            if (EquipmentsId.Count < 1)
            {
                continue; // Если нет доступных предметов с данной редкостью, переходим к редкости ниже уровнем
            }

            // Выбираем случайный предмет из списка доступных
            int randomIndex = Random.Shared.Next(EquipmentsId.Count);
            int selectedBaseEquipmentId = EquipmentsId[randomIndex];
            return cacheService.TableBaseEquipments[selectedBaseEquipmentId];
        }

        return null;
    }

    private async Task<Result> AddNewHeroAsync(DbContextGame db, BaseHero baseHero, Guid userId, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Fail("IsCancellationRequested");
        }

        Hero hero = HeroFactory.CreateFromBaseHero(baseHero, userId);

        _ = await db.Heroes.AddAsync(hero, cancellationToken).ConfigureAwait(false);
        int rowsChanged = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        if (rowsChanged < 1)
        {
            logger.LogError("Failed to add new hero for user {UserId}", userId);
            return Result.Fail("Hero not added");
        }

        return Result.Ok();
    }

    public async Task<Result> AddNewEquipmentAsync(DbContextGame db, BaseEquipment baseEquipment, Guid userId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Result.Fail("IsCancellationRequested");
        }

        Equipment equipment = EquipmentFactory.CreateFromBaseEquipment(baseEquipment, userId);

        _ = await db.Equipments.AddAsync(equipment, cancellationToken).ConfigureAwait(false);
        int rowsChanged = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        if (rowsChanged < 1)
        {
            logger.LogError("Failed to add new equipment for user {UserId}", userId);
            return Result.Fail("Equipment not added");
        }

        return Result.Ok();
    }



}
