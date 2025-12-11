using MongoDB.Bson;

namespace Server_DB_UserData;

public static class BsonDocumentExt
{

    public static double GetDouble(this BsonDocument d, string key)
    {
        return d.Contains(key) ? d[key].AsDouble : 0.0;
    }
    public static long GetLong(this BsonDocument d, string key)
    {
        return d.Contains(key) ? Convert.ToInt64(d[key]) : 0L;//Используем Convert.ToInt64 на случай если значения в базе int а не long
    }
    public static int GetInt(this BsonDocument d, string key)
    {
        return d.Contains(key) ? d[key].AsInt32 : 0;
    }
    public static Guid GetGuid(this BsonDocument d, string key)
    {
        return d.Contains(key) ? d[key].AsGuid : Guid.Empty;
    }
    public static string? GetString(this BsonDocument d, string key)
    {
        return d.Contains(key) ? d[key].AsString : null;
    }
}
