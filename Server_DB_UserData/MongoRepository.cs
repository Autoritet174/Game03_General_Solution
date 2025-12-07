using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Server_DB_UserData;

public class MongoRepository
{
    private readonly IMongoCollection<BsonDocument> _collection_heroes;
    private readonly IMongoCollection<BsonDocument> _collection_items;

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
        DatabaseSettings? userDataDb = config.DataBases.FirstOrDefault(db => db.Name.Equals("UserData", StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException("База данных 'UserData' не найдена в конфигурации.");

        // 2. Найти нужные коллекции в этой базе данных
        string heroesCollectionName = userDataDb.Collections
            .FirstOrDefault(c => c.Name.Equals("Heroes", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new ArgumentException("Коллекция 'Heroes' не найдена.");

        string itemsCollectionName = userDataDb.Collections
            .FirstOrDefault(c => c.Name.Equals("Items", StringComparison.OrdinalIgnoreCase))?.Name
            ?? throw new ArgumentException("Коллекция 'Items' не найдена.");

        MongoClient client = new(settings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(userDataDb.Name);

        _collection_heroes = database.GetCollection<BsonDocument>(heroesCollectionName);
        _collection_items = database.GetCollection<BsonDocument>(itemsCollectionName);
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


    /// <summary>
    /// 
    /// </summary>
    //public async Task<List<dynamic>> GetAllHeroesByUserIdAsync()
    //{
    //    List<BsonDocument> documents = await _collection.Find(new BsonDocument()).ToListAsync();

    //    List<dynamic> result = [.. documents.Select(doc => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<dynamic>(doc))];
    //    return result;
    //}
    public async Task<List<object>> GetHeroesByUserIdAsync(Guid owner_id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("owner_id", owner_id);

        List<BsonDocument> documents = await _collection_heroes.Find(filter).ToListAsync();
        var result = documents.Select(d =>
        {
            double GetDouble(string key)
            {
                return d.Contains(key) ? d[key].AsDouble : 0.0;
            }
            long GetLong(string key)
            {
                return d.Contains(key) ? Convert.ToInt64(d[key]) : 0L;//Используем Convert.ToInt64 на случай если значения в базе int а не long
            }
            int GetInt(string key)
            {
                return d.Contains(key) ? d[key].AsInt32 : 0;
            }
            Guid GetGuid(string key)
            {
                return d.Contains(key) ? d[key].AsGuid : Guid.Empty;
            }
            string? GetString(string key)
            {
                return d.Contains(key) ? d[key].AsString : null;
            }

            return new
            {
                _id = d["_id"].AsObjectId.ToString(),
                owner_id = GetGuid("owner_id"),
                hero_id = GetInt("hero_id"),

                level = GetInt("level"),
                exp_now = GetLong("exp_now"),
                exp_max = GetLong("exp_max"),

                group_name = GetString("group_name"),
                health = GetLong("health"),
                //attack = GetLong("attack"),
                strength = GetLong("strength"),
                agility = GetLong("agility"),
                intelligence = GetLong("intelligence"),
                haste = GetLong("haste"),
                crit_chance = GetDouble("crit_chance"),
                crit_power = GetDouble("crit_power"),
                endurance_physical = GetLong("endurance_physical"),
                endurance_magical = GetLong("endurance_magical"),
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
