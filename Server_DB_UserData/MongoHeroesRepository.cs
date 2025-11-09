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
    public async Task<List<object>> GetAllHeroesByUserIdAsync(Guid owner)
    {
        // Фильтр по полю "o" (owner). Поле в базе хранится как UUID, поэтому сравниваем с Guid напрямую
        var filter = Builders<BsonDocument>.Filter.Eq("o", owner);

        List<BsonDocument> documents = await _collection.Find(filter).ToListAsync();

        var result = documents.Select(d => new
        {
            _id = d["_id"].AsObjectId.ToString(),
            o = d["o"].AsGuid,   // владелец
            h = d["h"].AsGuid    // id героя
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
