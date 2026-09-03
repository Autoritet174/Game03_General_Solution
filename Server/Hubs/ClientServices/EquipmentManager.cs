using FluentResults;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.Hubs;
using Server_DB_Postgres;

namespace Server.Collection;

/// <summary>
/// Менеджер управления экипировкой персонажей.
/// </summary>
/// <param name="userId">Идентификатор владельца (текущего пользователя).</param>
/// <param name="dbContextFactory">Фабрика для создания контекста базы данных.</param>
/// <param name="logger">Интерфейс логирования.</param>
/// <param name="cacheService">Сервис для работы с кэшированными игровыми таблицами.</param>
public partial class EquipmentManager(
    Guid userId,
    IDbContextFactory<DbContextGame> dbContextFactory,
    ILogger<Client> logger,
    CacheService cacheService)
{
    #region Compiled Queries

    /// <summary>
    /// Скомпилированный запрос для получения героя по ID (без отслеживания изменений).
    /// </summary>
    private static readonly Func<DbContextGame, Guid, CancellationToken, Task<Hero?>> GetHeroByIdAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid id, CancellationToken ct) =>
            db.Heroes.AsNoTracking().FirstOrDefault(h => h.id == id));

    /// <summary>
    /// Скомпилированный запрос для получения предмета экипировки по ID.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, CancellationToken, Task<Equipment?>> GetEquipmentByIdAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid id, CancellationToken ct) =>
            db.Equipments.FirstOrDefault(e => e.id == id));

    /// <summary>
    /// Скомпилированный запрос для поиска предмета, надетого в конкретный слот героя.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, ESlot, CancellationToken, Task<Equipment?>> GetEquippedInSlotAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid heroId, ESlot slotId, CancellationToken ct) =>
            db.Equipments.FirstOrDefault(e => e.heroId == heroId && e.slotId == slotId));


    #endregion

    #region LoggerMessages

    [LoggerMessage(Level = LogLevel.Warning, Message = "Hero with id {HeroId} not found.")]
    private partial void LogHeroNotFound(Guid heroId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Hero with id {HeroId} access denied for user {UserId}.")]
    private partial void LogHeroAccessDenied(Guid heroId, Guid userId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Equipment with id {EquipmentId} not found or access denied for user {UserId}.")]
    private partial void LogEquipmentNotFound(Guid equipmentId, Guid userId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Equipment {EquipmentId} is already equipped by hero {HeroId}.")]
    private partial void LogEquipmentAlreadyEquipped(Guid equipmentId, Guid heroId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Equipment {EquipmentId} is not equipped on any hero.")]
    private partial void LogEquipmentNotEquipped(Guid equipmentId);

    #endregion

    /// <summary>
    /// Надевает предмет экипировки на указанного героя.
    /// </summary>
    /// <returns>Результат операции: Ok — успешно, Fail — ошибка валидации или БД.</returns>
    public async Task<Result> TakeOnAsync(Guid heroId, Guid equipmentId, bool? inAltSlot, CancellationToken cancellationToken)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        // Проверка экипировки (существование и принадлежность)
        Equipment? equipment = await GetEquipmentByIdAsync(db, equipmentId, cancellationToken).ConfigureAwait(false);
        if (equipment == null || equipment.userId != userId)
        {
            LogEquipmentNotFound(equipmentId, userId);
            return Result.Fail("Equipment not found or access denied.");
        }

        // Проверка героя (существование и принадлежность)
        Hero? hero = await GetHeroByIdAsync(db, heroId, cancellationToken).ConfigureAwait(false);
        if (hero == null)
        {
            LogHeroNotFound(heroId);
            return Result.Fail("Hero not found.");
        }
        if (hero.userId != userId)
        {
            LogHeroAccessDenied(heroId, userId);
            return Result.Fail("Hero access denied.");
        }

        // Если предмет уже одет на нужного героя, то просто возвращаем успех
        if (equipment.heroId == heroId)
        {
            // раньше я думал что тут нет смысла проверять принаджлежит ли герой игроку так как мы не меняем данные в базе
            // однако это позволяло бы любому игроку ручным управлением командами в вебсокете просматривать то какая экипировка надета на других героях других игроков, что могло давать нечесное преимущество например на арене
            return Result.Ok();
        }

        // Проверка, не надет ли уже предмет на кого-то другого
        if (equipment.heroId != null)
        {
            LogEquipmentAlreadyEquipped(equipmentId, equipment.heroId.Value);
            return Result.Fail("This equipment is already in use.");
        }

        // Определение целевого слота
        ESlot slotId = GetSlotId(equipment.baseEquipmentId, inAltSlot ?? false);

        // Обработка конфликта(если слот занят — снимаем текущий предмет)
        Equipment? currentSlotItem = await GetEquippedInSlotAsync(db, heroId, slotId, cancellationToken).ConfigureAwait(false);
        if (currentSlotItem != null)
        {
            currentSlotItem.heroId = null;
            currentSlotItem.slotId = null;

            // Приходиться вызывать отдельный SaveChangesAsync, так как это самый простой выход,
            // другие я уже попробовал и они суммарно хуже этого
            _ = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // Назначение новой экипировки
        equipment.slotId = slotId;
        equipment.heroId = heroId;

        int affectedRows = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return affectedRows > 0 ? Result.Ok() : Result.Fail("Database update failed during equipment assignment.");
    }

    /// <summary>
    /// Снимает указанный предмет экипировки с героя.
    /// </summary>
    /// <returns>Результат операции: Ok — предмет снят или уже был не надет.</returns>
    public async Task<Result> TakeOffAsync(Guid equipmentId, CancellationToken cancellationToken)
    {
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Equipment? equipment = await GetEquipmentByIdAsync(db, equipmentId, cancellationToken).ConfigureAwait(false);

        if (equipment == null || equipment.userId != userId)
        {
            LogEquipmentNotFound(equipmentId, userId);
            return Result.Fail("Equipment not found or access denied.");
        }

        // Оптимизация: если предмет и так не надет, не мучаем базу данных
        if (equipment.heroId == null)
        {
            LogEquipmentNotEquipped(equipmentId);
            return Result.Ok();
        }

        // Снимаем предмет с героя
        equipment.slotId = null;
        equipment.heroId = null;

        int countChanges = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return countChanges > 0 ? Result.Ok() : Result.Fail("Failed to update equipment state in database.");
    }

    /// <summary>
    /// Вычисляет ID слота на основе базового типа оборудования и флага альтернативного слота.
    /// </summary>
    /// <param name="baseEquipmentId">ID базового шаблона предмета.</param>
    /// <param name="inAltSlot">Флаг использования альтернативного слота (например, второе кольцо).</param>
    /// <returns>ID конкретного слота из базы данных.</returns>
    private ESlot GetSlotId(int baseEquipmentId, bool inAltSlot)
    {
        BaseEquipment baseEquip = cacheService.TableBaseEquipments[baseEquipmentId];
        ESlotType slotTypeId = baseEquip.equipmentType.slotType.id;

        return slotTypeId switch
        {
            ESlotType.weapon => inAltSlot ? ESlot.leftHand : ESlot.rightHand,     // Оружие
            ESlotType.ring => inAltSlot ? ESlot.ring2 : ESlot.ring1,    // Кольцо
            ESlotType.trinket => inAltSlot ? ESlot.trinket2 : ESlot.trinket1,  // Аксессуар
            _ => cacheService.TableSlots.Values.First(a => a.slotTypeId == slotTypeId).id
        };
    }
}
