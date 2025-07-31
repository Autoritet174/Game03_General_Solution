using Microsoft.EntityFrameworkCore;
using Server.DB.Data.Entities;

namespace Server.DB.Data.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
/// <remarks>
/// Создаёт экземпляр <see cref="HeroRepository"/>.
/// </remarks>
public class HeroRepository
{
    private readonly DbContext_Game03Data _dbContext;
    private readonly DbSet<Hero> _heroes;

    /// <param name="dbContext">Контекст базы данных.</param>
    public HeroRepository(DbContext_Game03Data dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _heroes = _dbContext.Heroes;
    }

    //public async Task AddAsync(string name)
    //{
        //Hero hero = new()
        //{
        //    Id = Guid.NewGuid(),
        //    CreatedAt = DateTimeOffset.UtcNow,
        //    UpdatedAt = DateTimeOffset.UtcNow,
        //    Name = name.ToUpper(),
        //    Rarity = 0,
        //    CreatureType = 
        //};

        //_ = _heroes.Add(hero);
        //_ = await _dbContext.SaveChangesAsync();
    //}


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
        ThrowHelper.ThrowIfRecordNotExists(await _heroes.AnyAsync(a => a.Id == hero.Id));

        hero.UpdatedAt = DateTimeOffset.UtcNow;

        _ = _heroes.Update(hero);
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
        int affected = await _heroes
            .Where(a => a.Id == id && a.DeletedAt == null)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(a => a.DeletedAt, _ => DateTimeOffset.UtcNow));

        ThrowHelper.ThrowIfRecordNotExists(affected > 0);
    }


    public async Task<List<Hero>> GetAllAsync()
    {
        return await _heroes.ToListAsync();
    }


    public async Task<Hero?> GetByIdAsync(Guid id)
    {
        return id == Guid.Empty ? null : await _heroes.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Возвращает пользователя по e-mail (без учёта регистра).
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Найденная запись или null.</returns>
    public async Task<Hero?> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        name = name.ToUpper();
        return await _heroes.FirstOrDefaultAsync(a => a.Name == name);
    }

}
