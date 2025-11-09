namespace Server_DB_UserData;

public class MongoHeroesSettings
{
    /// <summary>
    /// Строка подключения к MongoDB.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Имя коллекции.
    /// </summary>
    public string CollectionName { get; set; } = string.Empty;
}
