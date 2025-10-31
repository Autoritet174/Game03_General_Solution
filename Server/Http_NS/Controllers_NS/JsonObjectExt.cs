using System.Text;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS
{
    public static class JsonObjectExt
    {
        public static async Task<JsonObject?> GetJsonObjectFromRequest(HttpRequest httpRequest)
        {
            httpRequest.EnableBuffering();

            using StreamReader reader = new(httpRequest.Body, Encoding.UTF8, leaveOpen: true);
            string body = await reader.ReadToEndAsync();
            if (body == null || body.Trim() == string.Empty)
            {
                return null;
            }

            httpRequest.Body.Position = 0;
            JsonNode? data = null;
            try
            {
                data = JsonNode.Parse(body);
            }
            catch
            {
                return null;
            }

            return data is JsonObject obj ? obj : null;
        }
    }
}
