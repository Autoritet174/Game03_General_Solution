using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Collection;

namespace Server_DB_Postgres.Repositories;

/// <summary> Репозиторий для управления пользователями. </summary>
public class CollectionHeroRepository(DbContextGame dbContext)
{

    ///// <summary> Получить всех героев из коллекции игрока. </summary>
    //public IQueryable<Hero> GetCoolectionHeroes(Guid userId)
    //{
    //    return dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId);
    //}

}
