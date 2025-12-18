using General;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Users;

namespace Server_DB_Postgres.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
public class UserRepository
{
    private readonly DbContext_Game _dbContext;
    private readonly DbSet<User> _users;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для работы с пользователями.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="dbContext"/> равен null.
    /// </exception>
    public UserRepository(DbContext_Game dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _users = _dbContext.Users;
    }

    /// <summary>
    /// Асинхронно добавляет нового пользователя в базу данных.
    /// </summary>
    /// <param name="emailValidated">Подтвержденный email адрес пользователя.</param>
    /// <param name="passwordHashValidated">Хеш пароля пользователя.</param>
    /// <returns>Задача, представляющая асинхронную операцию добавления.</returns>
    /// <remarks>
    /// Создает нового пользователя с указанными email и хешем пароля,
    /// генерирует уникальный идентификатор и сохраняет в базу данных.
    /// </remarks>
    public async Task AddAsync(string emailValidated, string passwordHashValidated)
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Email = emailValidated,
            PasswordHash = passwordHashValidated,
        };

        _ = _users.Add(user);
        _ = await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно обновляет данные пользователя в базе данных.
    /// </summary>
    /// <param name="user">Сущность пользователя с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="user"/> равен null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если идентификатор пользователя является пустым GUID.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если сущность пользователя с указанным идентификатором не найдена в базе данных.
    /// </exception>
    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        ThrowHelper.ThrowIfRecordNotExists(await _users.AnyAsync(a => a.Id == user.Id));

        _ = _users.Update(user);
        _ = await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно получает всех пользователей из базы данных.
    /// </summary>
    /// <returns>
    /// Задача, результатом которой является список всех пользователей.
    /// Записи возвращаются без отслеживания изменений (AsNoTracking).
    /// </returns>
    public async Task<List<User>> GetAllAsync()
    {
        return await _users.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Асинхронно получает пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>
    /// Задача, результатом которой является найденный пользователь или null, если пользователь не найден.
    /// </returns>
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _users.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Асинхронно получает пользователя по email адресу вместе с активными блокировками.
    /// </summary>
    /// <param name="email">Email адрес пользователя (без учета регистра).</param>
    /// <returns>
    /// Задача, результатом которой является найденный пользователь с коллекцией активных блокировок
    /// или null, если пользователь не найден или email не указан.
    /// Запись возвращается без отслеживания изменений (AsNoTracking).
    /// </returns>
    /// <remarks>
    /// Поиск выполняется без учета регистра символов в email.
    /// Включает только активные блокировки (те, у которых дата создания меньше или равна текущей дате
    /// и либо не указана дата окончания, либо она еще не наступила).
    /// </remarks>
    public async Task<User?> GetByEmailWithBansAsync(string email)
    {
        return string.IsNullOrWhiteSpace(email) ? null :
            await _users.Include(u => u.UserBans.Where(b => b.CreatedAt <= DateTimeOffset.UtcNow && (b.ExpiresAt == null || b.ExpiresAt >= DateTimeOffset.UtcNow)))
                .AsNoTracking().FirstOrDefaultAsync(u => u.Email != null && EF.Functions.ILike(u.Email, email));
    }
}
