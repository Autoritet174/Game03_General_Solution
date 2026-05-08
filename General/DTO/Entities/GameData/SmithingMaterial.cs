using System.Text.Json.Serialization;

namespace General.DTO.Entities.GameData;

public class SmithingMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonIgnore]
    public string? NameRu { get; set; }
}
