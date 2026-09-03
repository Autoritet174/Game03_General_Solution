namespace General.DTO.Entities.GameData;

public class DamageType
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? nameRu { get; set; }
    public string? devHintRu { get; set; }
    public int category { get; set; } = 0;
}
