using General;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Users;

namespace Server_DB_Postgres.Repositories;

/// <summary>
/// Репозиторий для управления пользователями.
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
/// </remarks>
/// <param name="dbContext">Контекст базы данных для работы с пользователями.</param>
/// <exception cref="ArgumentNullException">
/// Выбрасывается, если <paramref name="dbContext"/> равен null.
/// </exception>
public class UserRepository(DbContext_Game dbContext)
{
    
}
