using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Server;

/// <summary> Причины блокировки пользователя </summary>
[Table(nameof(DbContextGame.UserBanReasons), Schema = nameof(Server))]
[Index(nameof(Name), IsUnique = true)]
public class UserBanReason
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Наименование причины блокировки. </summary>
    public required string Name { get; set; }

}
