using System.Text;
using System.Text.Json.Nodes;

namespace Server.Utilities;


/// <summary>
/// Расширение для работы с HTTP-запросами, предоставляющее методы для извлечения объекта JSON из тела запроса.
/// </summary>
public static class JsonObjectExtension
{

    /// <summary>
    /// Асинхронно извлекает и парсит тело HTTP-запроса как JSON-объект.
    /// </summary>
    /// <param name="httpRequest">HTTP-запрос, из которого читается тело.</param>
    /// <returns>
    /// Экземпляр <see cref="JsonObject"/>, если тело запроса содержит валидный JSON-объект; иначе — <c>null</c>.
    /// Возвращает <c>null</c>, если тело пустое, некорректно или не является объектом.
    /// </returns>
    /// <exception cref="ArgumentNullException">Возникает, если <paramref name="httpRequest"/> равен <c>null</c>.</exception>
    public static async Task<JsonObject?> GetJsonObjectFromRequest(HttpRequest httpRequest)
    {
        ArgumentNullException.ThrowIfNull(httpRequest);

        // Включаем буферизацию, чтобы можно было сбросить позицию потока после чтения
        httpRequest.EnableBuffering();

        using StreamReader reader = new(httpRequest.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync();

        // Проверяем, что тело не пустое и содержит данные
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        // Сбрасываем позицию тела запроса для последующих чтений (например, middleware)
        httpRequest.Body.Position = 0;

        JsonNode? data = null;
        try
        {
            data = JsonNode.Parse(body);
        }
        catch
        {
            // Если JSON повреждён или имеет неверный формат — возвращаем null
            return null;
        }

        // Проверяем, что распарсенный узел — именно объект, а не массив или примитив
        return data is JsonObject obj ? obj : null;
    }

    /// <summary>
    /// Извлекает строковое значение из <see cref="JsonObject"/> по ключу без учёта регистра.
    /// При необходимости удаляет поле из объекта.
    /// </summary>
    /// <param name="obj">Объект JSON, из которого извлекается значение.</param>
    /// <param name="key">Ключ для поиска (без учёта регистра).</param>
    /// <param name="removeAfterSuccessGetting">Если <c>true</c>, поле будет удалено из объекта после извлечения.</param>
    /// <returns>Строковое значение или пустая строка, если ключ не найден или значение null.</returns>
    public static string GetString(this JsonObject obj, string key, bool removeAfterSuccessGetting = false)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                if (removeAfterSuccessGetting)
                {
                    _ = obj.Remove(kv.Key);
                }
                return kv.Value?.ToString().Trim() ?? string.Empty;
            }
        }
        return string.Empty;
    }


    /// <summary>
    /// Извлекает строковое значение из <see cref="JsonObject"/> по указанному ключу без учета регистра.
    /// Если ключ не найден, возвращает <c>null</c>.
    /// Также поддерживает ограничение длины возвращаемой строки.
    /// </summary>
    /// <param name="obj">Объект JSON, из которого извлекается значение.</param>
    /// <param name="key">Ключ, по которому выполняется поиск (без учета регистра).</param>
    /// <param name="maxLength">Максимальная длина возвращаемой строки. Если значение больше, оно обрезается.
    /// Значение 0 или меньше означает отсутствие ограничения.</param>
    /// <returns>
    /// Строковое представление значения, обрезанное до <paramref name="maxLength"/>,
    /// или <c>null</c>, если ключ не найден или значение равно null.
    /// </returns>
    public static string? GetStringN(this JsonObject obj, string key, int maxLength = 0)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                if (kv.Value == null)
                {
                    return null;
                }
                string result = kv.Value.ToString().Trim();
                if (maxLength > 0 && result.Length > maxLength)
                {
                    result = result[..maxLength];
                }

                return result;

            }
        }
        return null;
    }


    /// <summary>
    /// Извлекает целочисленное значение из <see cref="JsonObject"/> по указанному ключу без учета регистра.
    /// Если ключ не найден, значение равно null или не может быть преобразовано в int, возвращает <c>null</c>.
    /// </summary>
    /// <param name="obj">Объект JSON, из которого извлекается значение.</param>
    /// <param name="key">Ключ, по которому выполняется поиск (без учета регистра).</param>
    /// <returns>
    /// если преобразование успешно; иначе — <c>null</c>.
    /// </returns>
    public static int? GetIntegerN(this JsonObject obj, string key)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                if (kv.Value != null)
                {
                    string s = kv.Value.ToString().Trim();
                    return int.TryParse(s, out int n) ? n : null;
                }
                return null;
            }
        }
        return null;
    }

    /// <summary>
    /// Извлекает булево значение из <see cref="JsonObject"/> по ключу без учёта регистра.
    /// Поддерживает значения: <c>true</c>, <c>false</c>, <c>1</c>, <c>0</c>, <c>"true"</c>, <c>"false"</c> (без учёта регистра).
    /// </summary>
    /// <param name="obj">JSON-объект, в котором выполняется поиск. Не должен быть <c>null</c>.</param>
    /// <param name="key">Ключ для поиска (без учёта регистра).</param>
    /// <returns>
    /// Булево значение или <c>null</c>, если ключ не найден, значение <c>null</c> или не удаётся интерпретировать.
    /// </returns>
    public static bool? GetBoolN(this JsonObject obj, string key)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                bool? result = (bool?)kv.Value;
                return result;
            }
        }
        return null;
    }
}
