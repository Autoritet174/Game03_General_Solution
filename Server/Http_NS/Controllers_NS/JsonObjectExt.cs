using System.Text;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS
{

    /// <summary>
    /// Расширение для работы с HTTP-запросами, предоставляющее методы для извлечения объекта JSON из тела запроса.
    /// </summary>
    public static class JsonObjectExt
    {

        /// <summary>
        /// Асинхронно извлекает и парсит тело HTTP-запроса как JSON-объект.
        /// </summary>
        /// <param name="httpRequest">HTTP-запрос, из которого читается тело.</param>
        /// <returns>
        /// Экземпляр <see cref="JsonObject"/>, если тело запроса содержит валидный JSON-объект; иначе — <c>null</c>.
        /// Возвращает <c>null</c>, если тело пустое, некорректно или не является объектом.
        /// </returns>
        /// <exception cref="ArgumentNullException">Возникает, если <paramref name="httpRequest"/> равен <c>null</c>.</exception>
        public static async Task<JsonObject?> GetJsonObjectFromRequest(HttpRequest httpRequest)
        {
            ArgumentNullException.ThrowIfNull(httpRequest);

            // Включаем буферизацию, чтобы можно было сбросить позицию потока после чтения
            httpRequest.EnableBuffering();

            using StreamReader reader = new(httpRequest.Body, Encoding.UTF8, leaveOpen: true);
            string body = await reader.ReadToEndAsync();

            // Проверяем, что тело не пустое и содержит данные
            if (string.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            // Сбрасываем позицию тела запроса для последующих чтений (например, middleware)
            httpRequest.Body.Position = 0;

            JsonNode? data = null;
            try
            {
                data = JsonNode.Parse(body);
            }
            catch
            {
                // Если JSON повреждён или имеет неверный формат — возвращаем null
                return null;
            }

            // Проверяем, что распарсенный узел — именно объект, а не массив или примитив
            return data is JsonObject obj ? obj : null;
        }
    }
}
