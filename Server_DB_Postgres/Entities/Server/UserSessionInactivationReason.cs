namespace Server_DB_Postgres.Entities.Server;

/// <summary> Причины завершения сессии </summary>
public class UserSessionInactivationReason
{
    public int Id { get; init; }
    public required string Name { get; set; }
    //public InactivationReason Code { get; set; }
}

//public enum InactivationReason : int
//{
//    Rotation = 1,
//    UserLogout = 2,
//    //MANUAL_REVOCATION = 4,
//    ServerRevoke = 3,
//    Expired = 4,
//}
