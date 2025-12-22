using Newtonsoft.Json;

namespace General.DTO;

public class Stats
{
    [JsonProperty("health")]
    public Dice? Health { get; set; }

    [JsonProperty("damage")]
    public Dice? Damage { get; set; }
}
