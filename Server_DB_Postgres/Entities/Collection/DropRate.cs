using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Collection;

[Table(nameof(DbContextGame.DropRates), Schema = nameof(Collection))]
public class DropRate : IVersion
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

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
