using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Server.DB.UserData;
public class MongoRepository
{
    private readonly IMongoCollection<BsonDocument> _collection;

    /// <summary>
    /// Конструктор репозитория, инициализирующий подключение к коллекции.
    /// </summary>
    public MongoRepository(IOptions<MongoSettings> settings)
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
    /// Асинхронно получает все документы коллекции.
    /// </summary>
    public async Task<List<dynamic>> GetAllAsync()
    {
        List<BsonDocument> documents = await _collection.Find(new BsonDocument()).ToListAsync();

        return [.. documents.Select(doc => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<dynamic>(doc))];
    }

    /// <summary>
    /// Асинхронно добавляет новый документ в коллекцию.
    /// </summary>
    public async Task InsertAsync(BsonDocument bd)
    {
        await _collection.InsertOneAsync(bd);
    }
}
