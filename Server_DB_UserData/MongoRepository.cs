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
    /// 
    /// </summary>
    //public async Task<List<dynamic>> GetAllHeroesByUserIdAsync()
    //{
    //    List<BsonDocument> documents = await _collection.Find(new BsonDocument()).ToListAsync();

    //    List<dynamic> result = [.. documents.Select(doc => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<dynamic>(doc))];
    //    return result;
    //}
    public async Task<List<object>> GetAllHeroesByUserIdAsync(Guid owner_id)
    {
        // Фильтр по полю "o" (owner). Поле в базе хранится как UUID, поэтому сравниваем с Guid напрямую
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("owner_id", owner_id);

        List<BsonDocument> documents = await _collection_heroes.Find(filter).ToListAsync();

        var result = documents.Select(d =>
        {
            string? groupName = d.Contains("group_name") ? d["group_name"].AsString : null;
            return new
            {
                _id = d["_id"].AsObjectId.ToString(),
                owner_id = d["owner_id"].AsGuid,
                hero_id = d["hero_id"].AsGuid,
                health = d["health"].AsInt32,
                attack = d["attack"].AsInt32,
                speed = d["speed"].AsInt32,
                strength = d["strength"].AsInt32,
                agility = d["agility"].AsInt32,
                intelligence = d["intelligence"].AsInt32,
                group_name = groupName,
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
