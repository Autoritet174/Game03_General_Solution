namespace Server_DB_Postgres.Interfaces;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; set; }
}
