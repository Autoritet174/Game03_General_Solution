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
    public string? KeyError { get; }

    /// <param name="result"></param>
    /// <param name="keyError"></param>
    public JwtTokenResult(string? result, string? keyError = null)
    {
        Result = result;
        KeyError = keyError;
        if (!(result == null ^ keyError == null))
        {
            throw new Exception("допустимо чтобы ровно одна из двух переменных result и keyError была true");
        }
    }
}
