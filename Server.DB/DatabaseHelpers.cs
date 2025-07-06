namespace Server.DB;

public static class DatabaseHelpers
{
    public static Guid CreateGuidPostgreSql()
    {
        return UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
    }
}
