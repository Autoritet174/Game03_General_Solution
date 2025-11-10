using Newtonsoft.Json.Linq;
using System;

namespace Game03Client.HttpRequester;

/// <summary>
/// Возвращает ответ от HttpRequester. 
/// </summary>
public class HttpRequesterResult
{
    /// <summary>
    /// Флаг успешно полученного ответа от сервера.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Результат полученный от сервера, null если результат не получен.
    /// </summary>
    public JObject? JObject { get; }

    /// <summary>
    /// Ключ ошибки. Сопоставим с кодами локализации в ini файле.
    /// </summary>
    public string? KeyError { get; }

    /// <summary>
    /// Програмная ошибка.
    /// </summary>
    public Exception? Ex { get; }

    /// <param name="success"></param>
    /// <param name="jObject"></param>
    /// <param name="keyError"></param>
    /// <param name="ex"></param>
    public HttpRequesterResult(bool success, JObject? jObject = null, string? keyError = null, Exception? ex = null)
    {
        Success = success;
        JObject = jObject;
        KeyError = keyError;
        Ex = ex;
        bool bothNull = ex == null && keyError == null;
        if (success)
        {
            if (!bothNull)
            {
                throw new Exception("при success==true в ответе HttpRequester ex и keyError должны быть равны null");
            }
        }
        else
        {
            if (bothNull)
            {
                throw new Exception("при success==false в ответе HttpRequester либо ex либо keyError должен быть не равен null");
            }
        }
    }
}
