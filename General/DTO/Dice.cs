using System;
using System.Text.Json.Serialization;

namespace General.DTO;

/// <summary>
/// Класс, представляющий бросок кубиков в формате "2d3+5",
/// </summary>
public record Dice
{
    /// <summary> Count. Количество бросаемых кубиков. </summary>
    [JsonPropertyName("c")]
    public int Count { get; set; }

    /// <summary> Sides. Размер кубика (число граней). </summary>
    [JsonPropertyName("s")]
    public int Sides { get; set; }

    /// <summary> Modificator. Модификатор к броску кубиков. Первые три знака, тысячные доли от единицы видимой игроку. </summary>
    [JsonPropertyName("m")]
    public long? Modificator_1000 { get; set; } = null;

    /// <summary> Минимальное значение. </summary>
    public long Min_1000 => (Count * 1000L) + (Modificator_1000 ?? 0L);

    /// <summary> Максимальное значение. </summary>
    public long Max_1000 => (1000L * (Count * Sides)) + (Modificator_1000 ?? 0L);

    /// <summary> Ожидаемое значение. </summary>
    public long Expected_1000 => (500L * (Count * (Sides + 1))) + (Modificator_1000 ?? 0L); // Count * (Sides + 1) / 2 * 1000

    public Dice() { }

    private const string MESSAGE_EXCEPTION = "Invalid Dice format";

    /// <summary>
    /// Примеры строк: "2d6+3", "20d358_123+9999".
    /// </summary>
    /// <param name="diceStr"></param>
    /// <exception cref="System.FormatException"></exception>
    public Dice(string diceStr)
    {
        ReadOnlySpan<char> span = diceStr.AsSpan();
        int i_d = span.IndexOf('d');

        if (i_d < 1)
        {
            throw new FormatException(MESSAGE_EXCEPTION);
        }

        Count = int.Parse(span[..i_d]);

        int i_p = span[i_d..].IndexOf('+');
        int i_d1 = i_d + 1;

        if (i_p == -1)
        {
            Sides = int.Parse(span[i_d1..]);
        }
        else
        {
            Sides = int.Parse(span[i_d1..(i_d + i_p)]);

            int mod = int.Parse(span[(i_d + i_p + 1)..]);
            if (mod != 0)
            {
                Modificator_1000 = mod;
            }
        }
    }

    public Dice(int count, int sides, long? modificator_1000 = null)
    {
        Count = count;
        Sides = sides;
        Modificator_1000 = modificator_1000;
    }
}
