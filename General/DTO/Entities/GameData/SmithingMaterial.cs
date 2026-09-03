using System.Text.Json.Serialization;

namespace General.DTO.Entities.GameData;

public class SmithingMaterial
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    [JsonIgnore]
    public string? nameRu { get; set; }
}
