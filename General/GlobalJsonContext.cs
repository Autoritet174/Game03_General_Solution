using General.DTO;
using General.DTO.Entities;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using System.Text.Json.Serialization;

namespace General;

/// <summary>
/// Глобальный контекст для Source Generation. 
/// Сюда нужно добавлять все типы, которые сохраняются в jsonb или передаются по API.
/// </summary>
[JsonSerializable(typeof(Dice))]
[JsonSerializable(typeof(DtoRequestAuthReg))]
[JsonSerializable(typeof(DtoResponseAuthReg))]
[JsonSerializable(typeof(DtoWebSocket))]
[JsonSerializable(typeof(DtoContainerGameData))]
[JsonSerializable(typeof(DtoContainerCollection))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class GlobalJsonContext : JsonSerializerContext
{

}
