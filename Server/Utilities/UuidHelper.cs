namespace Server.Utilities;

public static class UuidHelper
{
    public static Guid NewV7() {
        return UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
    }
}
