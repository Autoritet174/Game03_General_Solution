using System.Text.Json.Nodes;

namespace Server.DB;

public static class JsonObjectHelper
{

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
