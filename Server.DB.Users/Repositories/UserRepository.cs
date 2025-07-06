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
            Id = DatabaseHelpers.CreateGuidPostgreSql(),
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
    /// <exception cref="ArgumentNullException">Если пользователь равен null.</exception>
    /// <exception cref="ArgumentException">Если идентификатор пустой.</exception>
    /// <exception cref="InvalidOperationException">Если пользователь с таким идентификатором не найден.</exception>
    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (user.Id == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор пользователя не может быть пустым.", nameof(user));
        }

        bool exists = await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == user.Id && u.DeletedAt == null);

        if (!exists)
        {
            throw new InvalidOperationException("Пользователь не найден или удалён.");
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;

        _ = _dbContext.Users.Update(user);
        _ = await _dbContext.SaveChangesAsync();
    }


    /// <summary>
    /// Выполняет мягкое удаление пользователя по идентификатору без загрузки сущности.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <exception cref="ArgumentException">Если идентификатор некорректен.</exception>
    /// <exception cref="InvalidOperationException">Если пользователь не найден.</exception>
    public async Task SoftDeleteUserAsync(Guid id)
    {
        int affected = await _dbContext.Users
            .Where(u => u.Id == id && u.DeletedAt == null)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.DeletedAt, _ => DateTimeOffset.UtcNow));

        if (affected == 0)
        {
            throw new InvalidOperationException("Пользователь не найден или уже удалён.");
        }
    }


    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return id == Guid.Empty ? null : await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Возвращает пользователя по e-mail (без учёта регистра).
    /// </summary>
    /// <param name="email">E-mail для поиска.</param>
    /// <returns>Найденный пользователь или null.</returns>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : await _dbContext.Users.FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, email));
    }
}
