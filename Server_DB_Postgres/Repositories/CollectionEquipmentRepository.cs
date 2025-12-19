using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Collection;

namespace Server_DB_Postgres.Repositories;

/// <summary> Репозиторий для управления пользователями. </summary>
public class CollectionEquipmentRepository(DbContext_Game dbContext)
{
    /// <summary> Получить всю экипировку из коллекции игрока. </summary>
    public IQueryable<Equipment> GetCoolectionEquipments(Guid userId)
    {
        return dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId);
    }

}
