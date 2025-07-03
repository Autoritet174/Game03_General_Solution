using Microsoft.EntityFrameworkCore;
using Server.DB.Users.Entities;
using System;

namespace Server.DB.Users.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
/// <remarks>
/// Создаёт экземпляр <see cref="UserRepository"/>.
/// </remarks>
/// <param name="dbContext">Контекст базы данных.</param>
public class UserRepository(DB_Users dbContext)
{
    private readonly DB_Users _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


    /// <summary>
    /// Добавляет нового пользователя.
    /// </summary>
    /// <param name="user">Новая сущность пользователя.</param>
    /// <exception cref="ArgumentNullException">Если пользователь не передан.</exception>
    public async Task AddAsync(string email, string passwordHash)
    {
        //ArgumentNullException.ThrowIfNull(user);
        User user = new() {
            Id = UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Email = email,
            PasswordHash = passwordHash
        };


        //user.Id = UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        //user.CreatedAt = DateTimeOffset.UtcNow;
        //user.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            _ = _dbContext.Users.Add(user);
            _ = await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex) { 
            string s = ex.ToString();
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

        bool exists = await _dbContext.Users.AsNoTracking()
            .AnyAsync(u => u.Id == user.Id && u.DeletedAt == null);

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
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <exception cref="ArgumentException">Если идентификатор некорректен.</exception>
    /// <exception cref="InvalidOperationException">Если пользователь не найден.</exception>
    public async Task SoftDeleteUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Некорректный идентификатор пользователя.", nameof(userId));
        }

        int affected = await _dbContext.Users
            .Where(u => u.Id == userId && u.DeletedAt == null)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.DeletedAt, _ => DateTimeOffset.UtcNow));

        if (affected == 0)
        {
            throw new InvalidOperationException("Пользователь не найден или уже удалён.");
        }
    }


}
