using System.Text.Json.Serialization;

namespace General.DataBaseModels;

/// <summary>
/// Базовые характеристики героя
/// </summary>
public class HeroStats {
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

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

    public static string Sql { get; } = """
            SELECT id AS Id
            , name AS Name
            , health AS Health
            , attack AS Attack
            , strength AS Strength
            , agility AS Agility
            , intelligence AS Intelligence
            FROM heroes
            """;
}
