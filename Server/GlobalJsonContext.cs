using General.DTO;
using General.DTO.Entities;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Server_DB_Postgres.Entities.GameData;
using System.Text.Json.Serialization;

namespace Server;

/// <summary>
/// Глобальный контекст для Source Generation. 
/// Сюда нужно добавлять все типы, которые сохраняются в jsonb или передаются по API.
/// </summary>
[JsonSerializable(typeof(Dice))]
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
