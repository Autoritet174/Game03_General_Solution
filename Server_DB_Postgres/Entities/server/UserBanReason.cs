using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Server;

/// <summary> Причины блокировки пользователя </summary>
[Table("UserBanReasons", Schema = nameof(Server))]
[Index(nameof(Name), IsUnique = true)]
public class UserBanReason
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Наименование причины блокировки. </summary>
    public required string Name { get; set; }

    /// <summary> Коллекция <see cref="UserBan"/>. </summary>
    public ICollection<UserBan> UserBans { get; set; } = [];

}
