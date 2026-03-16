using System.Text.Json.Serialization;

namespace General.DTO;

public class ItemExp
{
    [JsonPropertyName("l")]
    public int Level { get; set; }

    [JsonPropertyName("x")] // названо этой буквой потому что postgres сортирует jsonb по алфавиту, сортировку задаём для наглядности
    public float Experience { get; set; }
}
