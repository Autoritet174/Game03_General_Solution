using Microsoft.EntityFrameworkCore;
using Server.DB.Game.Entities;

namespace Server.DB.Game.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
/// <remarks>
/// Создаёт экземпляр <see cref="HeroRepository"/>.
/// </remarks>
/// <param name="dbContext">Контекст базы данных.</param>
public class HeroRepository(DbContext_Game03Game dbContext)
{
    private readonly DbContext_Game03Game _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


    public async Task AddAsync(string name)
    {
        Hero user = new()
        {
            Id = UUIDv7.Generate(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Name = name
        };
        try
        {
            _ = _dbContext.Heroes.Add(user);
            _ = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _ = ex.ToString();
        }
    }


    /// <summary>
    /// Обновляет переданную сущность пользователя в базе данных.
    /// </summary>
    /// <param name="hero">Сущность с обновлёнными данными.</param>
    /// <exception cref="ArgumentNullException">Если равен null.</exception>
    /// <exception cref="ArgumentException">Если идентификатор пустой.</exception>
    /// <exception cref="InvalidOperationException">Если сущность с таким идентификатором не найдена.</exception>
    public async Task UpdateAsync(Hero hero)
    {
        ArgumentNullException.ThrowIfNull(hero);
        ThrowHelper.ThrowIfGuidEmpty(hero.Id);
        ThrowHelper.ThrowIfRecordNotExists(await _dbContext.Heroes.AnyAsync(a => a.Id == hero.Id));

        hero.UpdatedAt = DateTimeOffset.UtcNow;

        _ = _dbContext.Heroes.Update(hero);
        _ = await _dbContext.SaveChangesAsync();
    }


    /// <summary>
    /// Выполняет мягкое удаление записи по идентификатору без загрузки сущности.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <exception cref="ArgumentException">Если идентификатор некорректен.</exception>
    /// <exception cref="InvalidOperationException">Если запись не найдена.</exception>
    public async Task SoftDeleteAsync(Guid id)
    {
        int affected = await _dbContext.Heroes
            .Where(u => u.Id == id && u.DeletedAt == null)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(u => u.DeletedAt, _ => DateTimeOffset.UtcNow));

        ThrowHelper.ThrowIfRecordNotExists(affected > 0);
    }


    public async Task<List<Hero>> GetAllAsync()
    {
        return await _dbContext.Heroes.ToListAsync();
    }

    public async Task<Hero?> GetByIdAsync(Guid id)
    {
        return id == Guid.Empty ? null : await _dbContext.Heroes.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Возвращает пользователя по e-mail (без учёта регистра).
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Найденная запись или null.</returns>
    public async Task<Hero?> GetByNameAsync(string name)
    {
        return string.IsNullOrWhiteSpace(name) ? null : await _dbContext.Heroes.FirstOrDefaultAsync(u => EF.Functions.ILike(u.Name, name));
    }
}
