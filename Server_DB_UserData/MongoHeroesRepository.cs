using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Server_DB_UserData;
public class MongoHeroesRepository
{
    private readonly IMongoCollection<BsonDocument> _collection;

    /// <summary>
    /// Конструктор репозитория, инициализирующий подключение к коллекции.
    /// </summary>
    public MongoHeroesRepository(IOptions<MongoHeroesSettings> settings)
    {
        if (settings?.Value == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        MongoClient client = new(settings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);

        _collection = database.GetCollection<BsonDocument>(settings.Value.CollectionName);
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

        List<BsonDocument> documents = await _collection.Find(filter).ToListAsync();

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
        await _collection.InsertOneAsync(bd);
    }
}
