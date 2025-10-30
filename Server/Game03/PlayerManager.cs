using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Server.DB.UserData;
using Server.Extensions;

namespace Server.Game03;

public class PlayerManager(MongoRepository mongoRepository)
{
    public async Task Command(string jsonString)
    {
        var jsonData = JObject.Parse(jsonString);
        if (jsonData == null)
        {
            return;
        }

        var value = jsonData.GetValueSafe("command");
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        switch (value)
        {
            case "GetAllItem":
                PushAllItems();
                break;
            case "AddItem":
                await AddItem_Admin(jsonData);
                break;
        }
    }

    /// <summary>
    /// Отправить пользователю все его предметы
    /// </summary>
    private void PushAllItems()
    {

    }
    private async Task AddItem_Admin(JObject jObject)
    {
        if (jObject != null && jObject["item"] is JObject itemJObject)
        {
            BsonDocument itemBson = BsonSerializer.Deserialize<BsonDocument>(itemJObject.ToString());
            itemBson.ConvertStringToUuid("location");
            try
            {
                await mongoRepository.InsertAsync(itemBson);
            }
            catch (MongoWriteException ex) when (ex.WriteError?.Code == 2)
            {
                // Код 2 - документ слишком большой
                Console.WriteLine("Документ превышает 16MB");
            }

        }

        //if (jObject != null && jObject["item"] != null)
        //{
        //    JToken? item = jObject["item"];
        //    if (item != null && item.Type == JTokenType.Object)
        //    {
        //        JObject itemJObject = (JObject)item;
        //        BsonDocument bsonElements = itemJObject.ToBsonDocument();
        //        string s = itemJObject.ToString();
        //        await mongoRepository.InsertAsync(bsonElements);
        //    }
        //}
    }
}

