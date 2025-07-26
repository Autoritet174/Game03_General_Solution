using Microsoft.EntityFrameworkCore;
using Server.DB.Users.Entities;

namespace Server.DB.Users.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
/// <remarks>
/// Создаёт экземпляр <see cref="UserRepository"/>.
/// </remarks>
/// <param name="dbContext">Контекст базы данных.</param>
public class UserRepository(DbContext_Game03Users dbContext)
{
    private readonly DbContext_Game03Users _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


    /// <summary>
    /// Добавляет нового пользователя.
    /// </summary>
    /// <param name="emailValidated"></param>
    /// <param name="passwordHashValidated"></param>
    /// <returns></returns>
    public async Task AddAsync(string emailValidated, string passwordHashValidated)
    {
        User user = new()
        {
            Id = UUIDv7.Generate(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Email = emailValidated,
            PasswordHash = passwordHashValidated
        };
        try
        {
            _ = _dbContext.Users.Add(user);
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
    /// <param name="user">Сущность пользователя с обновлёнными данными.</param>
    /// <exception cref="ArgumentNullException">Если равен null.</exception>
    /// <exception cref="ArgumentException">Если идентификатор пустой.</exception>
    /// <exception cref="InvalidOperationException">Если сущность с таким идентификатором не найдена.</exception>
    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        ThrowHelper.ThrowIfGuidEmpty(user.Id);
        ThrowHelper.ThrowIfRecordNotExists(await _dbContext.Users.AnyAsync(u => u.Id == user.Id));

        user.UpdatedAt = DateTimeOffset.UtcNow;
        _ = _dbContext.Users.Update(user);
        _ = await _dbContext.SaveChangesAsync();
    }


    /// <summary>
    /// Выполняет мягкое удаление записи по идентификатору без загрузки сущности.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <exception cref="ArgumentException">Если идентификатор некорректен.</exception>
    /// <exception cref="InvalidOperationException">Если запись не найдена.</exception>
    public async Task SoftDeleteUserAsync(Guid id)
    {
        int affected = await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.DeletedAt, _ => DateTimeOffset.UtcNow));

        // affected количество удаленных записей
        ThrowHelper.ThrowIfRecordNotExists(affected > 0);
    }


    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return id == Guid.Empty ? null : await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Возвращает пользователя по e-mail (без учёта регистра).
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Найденная запись или null.</returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : await _dbContext.Users.FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, email));
    }
}
