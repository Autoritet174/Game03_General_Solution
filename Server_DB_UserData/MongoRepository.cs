using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Server_DB_UserData;

/// <summary>
/// Репозиторий коллекции героев.
/// </summary>
public class MongoRepository
{
    private readonly IMongoCollection<BsonDocument> _collection_heroes;
    private readonly IMongoCollection<BsonDocument> _collection_equipment;

    /// <summary>
    /// Конструктор репозитория, инициализирующий подключение к коллекции.
    /// </summary>
    public MongoRepository(IOptions<MongoDbSettings> settings)
    {
        if (settings?.Value == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        MongoDbSettings config = settings.Value;

        // 1. Найти нужную базу данных по имени
        DatabaseSettings? userDataDb = config.DataBases.FirstOrDefault(db => db.Name.Equals("userData", StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException("База данных 'userData' не найдена в конфигурации.");

        // 2. Найти нужные коллекции в этой базе данных
        string heroesCollectionName = userDataDb.Collections
            .FirstOrDefault(c => c.Name.Equals("heroes", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new ArgumentException("Коллекция 'heroes' не найдена.");

        string equipmentCollectionName = userDataDb.Collections
            .FirstOrDefault(c => c.Name.Equals("equipment", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new ArgumentException("Коллекция 'equipment' не найдена.");

        MongoClient client = new(settings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(userDataDb.Name);

        _collection_heroes = database.GetCollection<BsonDocument>(heroesCollectionName);
        _collection_equipment = database.GetCollection<BsonDocument>(equipmentCollectionName);
    }

    /// <summary>
    /// Статический метод для проверки подключения к базе данных.
    /// </summary>
    /// <param name="settings"></param>
    /// <exception cref="Exception">Генерирует исключение в случае ошибки подключения.</exception>
    public static async Task ThrowIfFailureConnectionAsync(IOptions<MongoDbSettings> settings)
    {
        try
        {
            DatabaseSettings? userDataDb = settings.Value.DataBases.FirstOrDefault(db => db.Name.Equals("UserData", StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException("База данных 'UserData' не найдена в конфигурации.");
            string heroesCollectionName = userDataDb.Collections
            .FirstOrDefault(c => c.Name.Equals("Heroes", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new ArgumentException("Коллекция 'Heroes' не найдена.");

            MongoClient client = new(settings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(userDataDb.Name);
            IMongoCollection<BsonDocument> collection_heroes = database.GetCollection<BsonDocument>(heroesCollectionName);
            _ = await collection_heroes.Find(new BsonDocument()).FirstOrDefaultAsync();
        }
        catch
        {
            System.Console.WriteLine($"\r\n\r\nFailureConnection in {nameof(MongoRepository)}, connectionString={settings.Value.ConnectionString}\r\n\r\n");
            throw;
        }
    }


    ///// <summary>
    ///// 
    ///// </summary>
    //public async Task<List<dynamic>> GetAllHeroesByUserIdAsync()
    //{
    //    List<BsonDocument> documents = await _collection.Find(new BsonDocument()).ToListAsync();

    //    List<dynamic> result = [.. documents.Select(doc => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<dynamic>(doc))];
    //    return result;
    //}

    /// <summary>
    /// Получить всех героев.
    /// </summary>
    public async Task<List<object>> GetHeroesByUserIdAsync(Guid ownerId)
    {
        DateTime now = DateTime.Now;
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("OwnerId", ownerId);

        List<BsonDocument> documents = await _collection_heroes.Find(filter).ToListAsync();
        var result = documents.Select(d =>
        {
            return new
            {
                _id = d["_id"].AsObjectId.ToString(),
                OwnerId = d.GetGuid("OwnerId"),
                HeroId = d.GetInt("HeroId"),
                GroupName = d.GetString("GroupName"),

                //Уровень
                Level = d.GetInt("Level"),
                ExpNow = d.GetLong("ExpNow"),
                ExpMax = d.GetLong("ExpMax"),

                //Базовые характеристики
                Health = d.GetLong("Health"),
                Attack = d.GetLong("Attack"),
                Str = d.GetLong("Str"),
                Agi = d.GetLong("Agi"),
                Int = d.GetLong("Int"),
                Haste = d.GetLong("Haste"),
                CritChance = d.GetDouble("CritChance"),
                CritPower = d.GetDouble("CritPower"),
                EndurancePhysical = d.GetLong("EndurancePhysical"),
                EnduranceMagical = d.GetLong("EnduranceMagical"),
            };
        }).Cast<object>().ToList();

        Console.WriteLine($"ЗАГРУЖЕНО {result.Count} за {(DateTime.Now - now).TotalSeconds} секунд");
        return result;
    }


    /// <summary>
    /// Получить всю экипировку.
    /// </summary>
    public async Task<List<object>> GetEquipmentByUserIdAsync(Guid owner_id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("owner_id", owner_id);

        List<BsonDocument> documents = await _collection_equipment.Find(filter).ToListAsync();
        var result = documents.Select(d =>
        {
            return new
            {
                _id = d["_id"].AsObjectId.ToString(),
                OwnerId = d.GetGuid("OwnerId"),
                EquipId = d.GetInt("EquipId"),
                GroupName = d.GetString("GroupName"),

                //Базовые характеристики
                Health = d.GetLong("Health"),
                Attack = d.GetLong("Attack"),
                Str = d.GetLong("Str"),
                Agi = d.GetLong("Agi"),
                Int = d.GetLong("Int"),
                Haste = d.GetLong("Haste")
            };
        }).Cast<object>().ToList();

        return result;
    }


    /// <summary>
    /// Асинхронно добавляет новый документ в коллекцию.
    /// </summary>
    public async Task InsertAsync(BsonDocument bd)
    {
        await _collection_heroes.InsertOneAsync(bd);
    }



}
