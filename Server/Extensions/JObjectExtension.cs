using Newtonsoft.Json.Linq;

namespace Server.Extensions;

/// <summary>
/// Содержит расширения для работы с объектами <see cref="JObject"/>.
/// </summary>
public static class JObjectExtension
{

    /// <summary>
    /// Безопасно извлекает значение по указанному пути в структуре JSON.
    /// Если путь не существует или значение имеет другой тип, возвращает <c>null</c>.
    /// </summary>
    /// <param name="obj">Объект <see cref="JObject"/>, из которого извлекается значение.</param>
    /// <param name="path">Путь к значению в формате JSONPath (например: "$.user.name")</param>
    /// <returns>Значение как строка, если оно существует и может быть преобразовано в строку; иначе <c>null</c>.</returns>
    public static string? GetValueSafe(this JObject obj, string path)
    {
        //JToken? token = obj.SelectToken(path);
        //return token?.ToObject<string>();
        return obj.SelectToken(path)?.ToObject<string>();
    }
}
