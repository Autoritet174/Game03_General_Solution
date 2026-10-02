namespace Server_DB_Postgres.Entities.Server;

/// <summary> Причины блокировки пользователя </summary>
public class UserBanReason
{
    public int id { get; init; }

    /// <summary> Наименование причины блокировки. </summary>
    public required string name { get; set; }

}
