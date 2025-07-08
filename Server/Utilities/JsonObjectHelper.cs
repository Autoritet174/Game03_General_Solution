using System.Text.Json.Nodes;

namespace Server.Utilities;

public static class JsonObjectHelper
{
    public static string FindValueIgnoreCase(JsonObject obj, string key)
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
}
