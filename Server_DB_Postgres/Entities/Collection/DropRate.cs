using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Users;

namespace Server_DB_Postgres.Entities.Collection;

public class DropRate : IVersion
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public long Version { get; set; }

    /// <summary>
    /// Тип вероятностей. 1 - Герой. 2 - Предмет.
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// Абстрактое количество из которого сервер вычислит вероятность.
    /// </summary>
    public int[] Counts { get; set; } = [];
}
