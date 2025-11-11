using Newtonsoft.Json.Linq;
using System;

namespace Game03Client.JwtToken;

/// <summary>
/// Результат при получении токена.
/// </summary>
public class JwtTokenResult
{
    /// <summary>
    /// Jwt токен. null если не получилось извлечь токен.
    /// </summary>
    public string? Result { get; }

    /// <summary>
    /// Ключ ошибки если не удалось извлечь токен.
    /// </summary>
    public JObject? JObject { get; }

    /// <param name="result"></param>
    /// <param name="jObject"></param>
    public JwtTokenResult(string? result, JObject? jObject = null)
    {
        Result = result;
        JObject = jObject;
        if (!(result == null ^ jObject == null))
        {
            throw new Exception($"допустимо чтобы ровно одна из двух переменных {nameof(result)} и {nameof(jObject)} была null");
        }
    }
}
