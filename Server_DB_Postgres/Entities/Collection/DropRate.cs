using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Users;

namespace Server_DB_Postgres.Entities.Collection;

public class DropRate : IVersion
{
    public Guid id { get; set; }

    public Guid userId { get; set; }
    public User? user { get; set; }

    public long version { get; set; }

    /// <summary>
    /// Тип вероятностей. 1 - Герой. 2 - Предмет.
    /// </summary>
    public int type { get; set; }

    /// <summary>
    /// Абстрактое количество из которого сервер вычислит вероятность.
    /// </summary>
    public int[] counts { get; set; } = [];
}
