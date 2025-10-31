using System.Text.Json.Nodes;

namespace Server_DB;

/// <summary>
/// Вспомогательный класс для извлечения значений из объекта <see cref="JsonObject"/> по ключу без учета регистра.
/// </summary>
public static class JsonObjectHelper
{

    /// <summary>
    /// Извлекает строковое значение из <see cref="JsonObject"/> по указанному ключу без учета регистра.
    /// Если ключ не найден или значение отсутствует, возвращает пустую строку.
    /// </summary>
    /// <param name="obj">Объект JSON, из которого извлекается значение.</param>
    /// <param name="key">Ключ, по которому выполняется поиск (без учета регистра).</param>
    /// <returns>Строковое представление значения или пустая строка, если ключ не найден или значение равно null.</returns>
    public static string GetString(JsonObject obj, string key)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                return kv.Value?.ToString() ?? string.Empty;
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
    public static string? GetStringN(JsonObject obj, string key, int maxLength = 0)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                if (kv.Value == null)
                {
                    return null;
                }
                string result = kv.Value.ToString();
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
    /// Значение типа <see cref="int"/> в виде Nullable (<see cref="int?"/>),
    /// если преобразование успешно; иначе — <c>null</c>.
    /// </returns>
    public static int? GetIntegerN(JsonObject obj, string key)
    {
        foreach (KeyValuePair<string, JsonNode?> kv in obj)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                if (kv.Value != null)
                {
                    string s = kv.Value.ToString();
                    return int.TryParse(s, out int n) ? n : null;
                }
                return null;
            }
        }
        return null;
    }

}
