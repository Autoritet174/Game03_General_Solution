namespace Server_DB_UserData;

// 1. Класс для элемента массива Collections
/// <summary>
/// Настройки конкретной коллекции.
/// </summary>
public class CollectionSettings
{
    /// <summary>
    /// Имя коллекции.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

// 2. Класс для элемента массива DataBases
/// <summary>
/// Настройки конкретной базы данных.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Список коллекций в базе данных.
    /// </summary>
    // COLLECTIONS теперь List<CollectionSettings>
    public List<CollectionSettings> Collections { get; set; } = new List<CollectionSettings>();
}

// 3. Главный класс для секции "MongoDb"
/// <summary>
/// Общие настройки подключения к MongoDB.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// Строка подключения к MongoDB.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Список настроек баз данных.
    /// </summary>
    // DataBases теперь List<DatabaseSettings>
    public List<DatabaseSettings> DataBases { get; set; } = [];
}
