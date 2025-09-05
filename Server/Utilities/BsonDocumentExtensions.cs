using MongoDB.Bson;
using System.Reflection.Metadata;

namespace Server.Utilities;

public static class BsonDocumentExtensions
{
    public static void ConvertStringToUuid(this BsonDocument document, string fieldName)
    {
        if (document.Contains(fieldName) &&
            document[fieldName].IsString &&
            Guid.TryParse(document[fieldName].AsString, out Guid guid))
        {
            document[fieldName] = new BsonBinaryData(
                guid.ToByteArray(),
                BsonBinarySubType.UuidStandard
            );
        }
    }

    public static void ConvertUuidFields(BsonDocument document, params string[] fieldNames)
    {
        foreach (var fieldName in fieldNames)
        {
            if (document.Contains(fieldName) &&
                document[fieldName].IsString &&
                Guid.TryParse(document[fieldName].AsString, out Guid guid))
            {
                document[fieldName] = new BsonBinaryData(
                    guid.ToByteArray(),
                    BsonBinarySubType.UuidStandard
                );
            }
        }
    }

}
