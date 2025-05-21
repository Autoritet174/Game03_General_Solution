using System.Text.Json.Serialization;

namespace General.DataBaseModels;

/// <summary>
/// Базовые характеристики героя
/// </summary>
public class HeroStats {
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("nameEn")]
    public string? NameEn { get; set; }

    [JsonPropertyName("nameRu")]
    public string? NameRu { get; set; }

    [JsonPropertyName("health")]
    public float Health { get; set; }

    [JsonPropertyName("attack")]
    public float Attack { get; set; }

    [JsonPropertyName("strength")]
    public float Strength { get; set; }

    [JsonPropertyName("agility")]
    public float Agility { get; set; }

    [JsonPropertyName("intelligence")]
    public float Intelligence { get; set; }
}
