using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace General;

public static class JsonSettings
{
    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Отключаем экранирование Unicode

        // Важно для производительности: включаем кэширование сериализаторов
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };

}
