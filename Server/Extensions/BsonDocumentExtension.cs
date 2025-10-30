using MongoDB.Bson;

namespace Server.Extensions;

/// <summary>
/// Содержит расширения для работы с объектами <see cref="BsonDocument"/>.
/// </summary>
public static class BsonDocumentExtension
{
    /// <summary>
    /// Преобразует строковое значение в поле документа в UUID-тип <see cref="BsonBinaryData"/>
    /// с подтипом <see cref="BsonBinarySubType.UuidStandard"/>, если это возможно.
    /// </summary>
    /// <param name="document">Документ <see cref="BsonDocument"/>.</param>
    /// <param name="fieldName">Имя поля, содержащего строковое значение.</param>
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

    /// <summary>
    /// Применяет преобразование строковых значений в UUID-тип <see cref="BsonBinaryData"/>
    /// ко всем указанным полям документа, если это возможно.
    /// </summary>
    /// <param name="document">Документ <see cref="BsonDocument"/>.</param>
    /// <param name="fieldNames">Массив имен полей для обработки.</param>
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
