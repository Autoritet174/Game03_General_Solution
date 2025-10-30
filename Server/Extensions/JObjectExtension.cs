using Newtonsoft.Json.Linq;

namespace Server.Extensions;

public static class JObjectExtension
{
    public static string? GetValueSafe(this JObject obj, string path)
    {
        //JToken? token = obj.SelectToken(path);
        //return token?.ToObject<string>();
        return obj.SelectToken(path)?.ToObject<string>();
    }
}
