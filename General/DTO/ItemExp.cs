using System.Text.Json.Serialization;

namespace General.DTO;

public class ItemExp
{
    [JsonPropertyName("l")]
    public int Level { get; set; }

    [JsonPropertyName("e")]
    public long Experience_1000 { get; set; }
}
