using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO;

/// <summary>
/// Класс, представляющий бросок кубиков в формате "2d3+5",
/// </summary>

public record Dice
{
    /// <summary> Count. Количество бросаемых кубиков. </summary>
    [JsonProperty("c")]
    public int Count { get; set; }

    /// <summary> Sides. Размер кубика (число граней). </summary>
    [JsonProperty("s")]
    public int Sides { get; set; }

    /// <summary> Modificator. Модификатор к броску кубиков. Первые три знака, тысячные доли от единицы видимой игроку. </summary>
    [JsonProperty("m")]
    public long? Modificator1000 { get; set; } = null;
}
