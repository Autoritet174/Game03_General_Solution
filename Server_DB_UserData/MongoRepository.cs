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
    public async Task<List<object>> GetHeroesByUserIdAsync(Guid owner_id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("owner_id", owner_id);

        List<BsonDocument> documents = await _collection_heroes.Find(filter).ToListAsync();
        var result = documents.Select(d =>
        {
            return new
            {
                _id = d["_id"].AsObjectId.ToString(),
                owner_id = d.GetGuid("owner_id"),
                hero_id = d.GetInt("hero_id"),
                group_name = d.GetString("group_name"),

                //Уровень
                level = d.GetInt("level"),
                exp_now = d.GetLong("exp_now"),
                exp_max = d.GetLong("exp_max"),

                //Базовые характеристики
                health = d.GetLong("health"),
                attack = d.GetLong("attack"),
                strength = d.GetLong("strength"),
                agility = d.GetLong("agility"),
                intelligence = d.GetLong("intelligence"),
                haste = d.GetLong("haste"),
                crit_chance = d.GetDouble("crit_chance"),
                crit_power = d.GetDouble("crit_power"),
                endurance_physical = d.GetLong("endurance_physical"),
                endurance_magical = d.GetLong("endurance_magical"),
            };
        }).Cast<object>().ToList();

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
                owner_id = d.GetGuid("owner_id"),
                hero_id = d.GetInt("hero_id"),
                group_name = d.GetString("group_name"),

                //Уровень
                level = d.GetInt("level"),
                exp_now = d.GetLong("exp_now"),
                exp_max = d.GetLong("exp_max"),

                //Базовые характеристики
                health = d.GetLong("health"),
                attack = d.GetLong("attack"),
                strength = d.GetLong("strength"),
                agility = d.GetLong("agility"),
                intelligence = d.GetLong("intelligence"),
                haste = d.GetLong("haste"),
                crit_chance = d.GetDouble("crit_chance"),
                crit_power = d.GetDouble("crit_power"),
                endurance_physical = d.GetLong("endurance_physical"),
                endurance_magical = d.GetLong("endurance_magical"),
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
