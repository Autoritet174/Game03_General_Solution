using General.DTO;
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace General;


/// <summary>
/// Глобальный контекст для Source Generation. 
/// Сюда нужно добавлять все типы, которые сохраняются в jsonb или передаются по API.
/// </summary>
[JsonSerializable(typeof(Dice))]
[JsonSerializable(typeof(Dictionary<int, List<float>>))]
[JsonSerializable(typeof(DtoRequestAuthReg))]
[JsonSerializable(typeof(DtoResponseAuthReg))]
[JsonSerializable(typeof(BaseHero))]
[JsonSerializable(typeof(DtoContainerGameData))]
[JsonSerializable(typeof(DtoContainerCollection))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class GlobalJsonContext : JsonSerializerContext
{

}




public static class JSON
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Отключаем экранирование Unicode

        // Важно для производительности: включаем кэширование сериализаторов
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonDocument Parse(string json) => JsonDocument.Parse(json);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, Options);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);

}



public static class JsonElementExt
{
    public static bool IsEmpty(this JsonElement? element)
    {
        return element is null || element.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined
            || (element.Value.ValueKind == JsonValueKind.String
                && string.IsNullOrWhiteSpace(element.Value.GetString()));
    }

    public static string GetString(this JsonElement? element)
    {
        return element is null
            ? string.Empty
            : element.Value.ValueKind == JsonValueKind.String
            ? element.Value.GetString() ?? string.Empty
            : element.Value.ToString();
    }

    public static int GetInt(this JsonElement? element)
    {
        return element is not null && element.Value.TryGetInt32(out int v) ? v : 0;
    }

    public static long GetLong(this JsonElement? element)
    {
        return element is not null && element.Value.TryGetInt64(out long v) ? v : 0L;
    }

    public static Guid GetGuid(this JsonElement? element)
    {
        return element is not null && element.Value.TryGetGuid(out Guid v) ? v : Guid.Empty;
    }

    public static double GetDouble(this JsonElement? element)
    {
        return element is not null && element.Value.TryGetDouble(out double v) ? v : 0.0;
    }
}
