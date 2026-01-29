using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server;

public static class GlobalJsonOptions
{
    public static JsonSerializerOptions jsonOptions;
    static GlobalJsonOptions()
    {
        //JsonSerializerSettings = new JsonSerializerSettings
        //{
        //    // Использование CamelCase для всех полей, если атрибуты не заданы явно
        //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //    // Компактный вид (аналог WriteIndented = false)
        //    Formatting = Formatting.None,
        //    // Игнорирование null значений для уменьшения веса JSON
        //    NullValueHandling = NullValueHandling.Ignore
        //};
        jsonOptions = new JsonSerializerOptions
        {
            // Аналог CamelCasePropertyNamesContractResolver
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            // Аналог NullValueHandling.Ignore
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

            // Аналог Formatting.None (false — значение по умолчанию, можно не указывать)
            WriteIndented = false,

            // Важно: подключаем сгенерированный контекст
            TypeInfoResolver = GlobalJsonContext.Default
        };
    }
}
