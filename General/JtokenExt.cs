using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace General;

public static class JtokenExt
{
    public static bool IsEmpty(this JToken? token)
    {
        return token == null || string.IsNullOrWhiteSpace(token.ToString());
    }

    /// <summary>
    /// Получить строковое представление JToken или пустую строку, если токен равен null.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string GetString(this JToken? token)
    {
        return token?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Получить целочисленное значение JToken или 0, если токен равен null.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static int GetInt(this JToken? token)
    {
        return token != null ? (int)token : 0;
    }

    public static long GetLong(this JToken? token)
    {
        return token != null ? (long)token : 0L;
    }

    public static Guid GetGuid(this JToken? token)
    {
        return token != null ? new Guid(token.ToString()) : Guid.Empty;
    }
    public static double GetDouble(this JToken? token)
    {
        return token != null ? (double)token : 0.0;
    }
}
