using Newtonsoft.Json.Linq;

namespace Server.Utilities;

public static class JObjectExtensions
{
    public static string? GetValueSafe(this JObject obj, string path)
    {
        //JToken? token = obj.SelectToken(path);
        //return token?.ToObject<string>();
        return obj.SelectToken(path)?.ToObject<string>();
    }
}