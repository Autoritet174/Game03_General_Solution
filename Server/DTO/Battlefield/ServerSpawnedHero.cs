using System.Text.Json.Serialization;
using General.DTO.Battlefield;
using Server.BattleField;

namespace Server.DTO.Battlefield;

/// <summary>Хранит серверное состояние участника боя и его собственные экземпляры способностей.</summary>
public sealed class ServerSpawnedHero : SpawnedHero
{
    /// <summary>Состояние способностей существует только в текущем бою и не передаётся клиенту.</summary>
    [JsonIgnore]
    public List<BattleAbility> abilities { get; init; } = [];
}
