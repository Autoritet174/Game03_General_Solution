using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO;

/// <summary>
/// Класс, представляющий бросок кубиков в формате "2d3+5",
/// </summary>
//[ComplexType]
public record Dice
{
    public Dice()
    {
    }

    public Dice(int count, int sides, int? modificator = null)
    {
        Count = count;
        Sides = sides;
        Modificator = modificator;
    }

    /// <summary> Count. Количество бросаемых кубиков. </summary>
    [JsonProperty("c")]
    public int Count { get; set; }

    /// <summary> Sides. Размер кубика (число граней). </summary>
    [JsonProperty("s")]
    public int Sides { get; set; }

    /// <summary> Modificator. Модификатор к броску кубиков. </summary>
    [JsonProperty("m")]
    public int? Modificator { get; set; } = null;
    //private const string MESSAGE_EXCEPTION = "Invalid dice format";

    ///// <summary> Количество бросаемых кубиков. </summary>
    //public int Count { get; }

    ///// <summary> Размер кубика (число граней). </summary>
    //public int Sizes { get; }

    ///// <summary> Модификатор к броску кубиков. </summary>
    //public int? Modificator { get; } = null;

    ///// <param name="count">Количество бросаемых кубиков.</param>
    ///// <param name="sizes">Размер кубика (число граней).</param>
    ///// <param name="modificator">Модификатор к броску кубиков.</param>
    //public Dice(int count, int sizes, int? modificator = null)
    //{
    //    Count = count;
    //    Sizes = sizes;
    //    Modificator = modificator;
    //}

    ///// <summary>
    ///// Примеры строк: "2d6_10+3", "20d358_123+9999".
    ///// </summary>
    ///// <param name="diceStr"></param>
    ///// <exception cref="System.FormatException"></exception>
    //public Dice(string diceStr)
    //{
    //    int i_d = diceStr.IndexOf('d');
    //    int i__ = diceStr.IndexOf('_');
    //    int i_p = diceStr.IndexOf('+');

    //    if (i_d < 1)
    //    {
    //        throw new System.FormatException(MESSAGE_EXCEPTION);
    //    }
    //    Count = int.Parse(diceStr[..i_d]);

    //    int bigNumber = 100000;
    //    if (i__ == -1)
    //    {
    //        i__ = bigNumber; // условно большой индекс
    //    }
    //    if (i_p == -1)
    //    {
    //        i_p = bigNumber; // условно большой индекс
    //    }

    //    int i_d1 = i_d + 1;
    //    int i_min = System.Math.Min(i__, i_p);
    //    if (i_min == bigNumber)
    //    {
    //        Sizes = int.Parse(diceStr[i_d1..]);
    //    }
    //    else
    //    {
    //        Sizes = int.Parse(diceStr[i_d1..i_min]);
    //    }

    //    if (i_p != bigNumber)
    //    {
    //        int mod = int.Parse(diceStr[(i_p + 1)..]);
    //        if (mod != 0)
    //        {
    //            Modificator = mod;
    //        }
    //    }
    //}

}
