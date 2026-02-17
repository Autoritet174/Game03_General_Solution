using FluentResults;
using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server.WebSocket_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Collection;
using Server_DB_Postgres.Entities.GameData;

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
    ILogger<WebSocketConnection> logger,
    CacheService cacheService)
{
    #region Compiled Queries

    /// <summary>
    /// Скомпилированный запрос для получения героя по ID (без отслеживания изменений).
    /// </summary>
    private static readonly Func<DbContextGame, Guid, CancellationToken, Task<Hero?>> GetHeroByIdAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid id, CancellationToken ct) =>
            db.Heroes.AsNoTracking().FirstOrDefault(h => h.Id == id));

    /// <summary>
    /// Скомпилированный запрос для получения предмета экипировки по ID.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, CancellationToken, Task<Equipment?>> GetEquipmentByIdAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid id, CancellationToken ct) =>
            db.Equipments.FirstOrDefault(e => e.Id == id));

    /// <summary>
    /// Скомпилированный запрос для поиска предмета, надетого в конкретный слот героя.
    /// </summary>
    private static readonly Func<DbContextGame, Guid, int, CancellationToken, Task<Equipment?>> GetEquippedInSlotAsync =
        EF.CompileAsyncQuery((DbContextGame db, Guid heroId, int slotId, CancellationToken ct) =>
            db.Equipments.FirstOrDefault(e => e.HeroId == heroId && e.SlotId == slotId));

    #endregion

    #region LoggerMessages

    [LoggerMessage(Level = LogLevel.Warning, Message = "Hero with id {HeroId} not found or access denied for user {UserId}.")]
    private partial void LogHeroNotFound(Guid heroId, Guid userId);

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
    /// <param name="dto">Объект передачи данных, содержащий ID героя и предмета.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Результат операции: Ok — успешно, Fail — ошибка валидации или БД.</returns>
    public async Task<Result> TakeOnAsync(DtoWSEquipmentTakeOnC2S dto, CancellationToken cancellationToken)
    {
        Guid heroId = dto.HeroId;
        Guid equipmentId = dto.EquipmentId;
        
        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        // Проверка экипировки (существование и принадлежность)
        Equipment? equipment = await GetEquipmentByIdAsync(db, equipmentId, cancellationToken).ConfigureAwait(false);
        if (equipment == null || equipment.UserId != userId)
        {
            LogEquipmentNotFound(equipmentId, userId);
            return Result.Fail("Equipment not found or access denied.");
        }

        // Если предмет уже одет на нужного героя, то просто возвращаем успех
        if (equipment.HeroId == heroId)
        {
            // тут нет смысла проверять принаджлежит ли герой игроку так как мы не меняем данные в базе
            return Result.Ok();
        }

        // Проверка героя (существование и принадлежность)
        Hero? hero = await GetHeroByIdAsync(db, heroId, cancellationToken).ConfigureAwait(false);
        if (hero == null || hero.UserId != userId)
        {
            LogHeroNotFound(heroId, userId);
            return Result.Fail("Hero not found or access denied.");
        }

        // Проверка, не надет ли уже предмет на кого-то другого
        if (equipment.HeroId != null)
        {
            LogEquipmentAlreadyEquipped(equipmentId, equipment.HeroId.Value);
            return Result.Fail("This equipment is already in use.");
        }

        // Определение целевого слота
        int slotId = GetSlotId(equipment.BaseEquipmentId, dto.InAltSlot ?? false);

        // Обработка конфликта (если слот занят — снимаем текущий предмет)
        Equipment? currentSlotItem = await GetEquippedInSlotAsync(db, heroId, slotId, cancellationToken).ConfigureAwait(false);
        if (currentSlotItem != null)
        {
            currentSlotItem.HeroId = null;
            currentSlotItem.SlotId = null;
        }

        // Назначение новой экипировки
        equipment.SlotId = slotId;
        equipment.HeroId = heroId;

        int affectedRows = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return affectedRows > 0 ? Result.Ok() : Result.Fail("Database update failed during equipment assignment.");
    }

    /// <summary>
    /// Снимает указанный предмет экипировки с героя.
    /// </summary>
    /// <param name="dto">Объект передачи данных, содержащий ID предмета.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Результат операции: Ok — предмет снят или уже был не надет.</returns>
    public async Task<Result> TakeOffAsync(DtoWSEquipmentTakeOffC2S dto, CancellationToken cancellationToken)
    {
        Guid equipmentId = dto.EquipmentId;

        await using DbContextGame db = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        Equipment? equipment = await GetEquipmentByIdAsync(db, equipmentId, cancellationToken).ConfigureAwait(false);

        if (equipment == null || equipment.UserId != userId)
        {
            LogEquipmentNotFound(equipmentId, userId);
            return Result.Fail("Equipment not found or access denied.");
        }

        // Оптимизация: если предмет и так не надет, не мучаем базу данных
        if (equipment.HeroId == null)
        {
            LogEquipmentNotEquipped(equipmentId);
            return Result.Ok();
        }

        // Снимаем предмет с героя
        equipment.SlotId = null;
        equipment.HeroId = null;

        int countChanges = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return countChanges > 0 ? Result.Ok() : Result.Fail("Failed to update equipment state in database.");
    }

    /// <summary>
    /// Вычисляет ID слота на основе базового типа оборудования и флага альтернативного слота.
    /// </summary>
    /// <param name="baseEquipmentId">ID базового шаблона предмета.</param>
    /// <param name="inAltSlot">Флаг использования альтернативного слота (например, второе кольцо).</param>
    /// <returns>ID конкретного слота из базы данных.</returns>
    private int GetSlotId(int baseEquipmentId, bool inAltSlot)
    {
        BaseEquipment baseEquip = cacheService.TableBaseEquipment.First(a => a.Id == baseEquipmentId);
        int slotTypeId = baseEquip.EquipmentType.SlotType.Id;

        return slotTypeId switch
        {
            1 => inAltSlot ? 2 : 1,     // Оружие
            14 => inAltSlot ? 9 : 8,    // Кольцо
            16 => inAltSlot ? 11 : 10,  // Аксессуар
            _ => cacheService.TableSlots.First(a => a.SlotTypeId == slotTypeId).Id
        };
    }
}
