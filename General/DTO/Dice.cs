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
    [JsonPropertyName("d")] // названо этой буквой потому что postgres сортирует jsonb по алфавиту, сортировку задаём для наглядности
    public int Sides { get; set; }

    /// <summary> Modificator. Модификатор к броску кубиков. </summary>
    [JsonPropertyName("m")]
    public float? Modificator { get; set; } = null;

    /// <summary> Минимальное значение. </summary>
    public float Min => Count + (Modificator ?? 0f);

    /// <summary> Максимальное значение. </summary>
    public float Max => (Count * Sides) + (Modificator ?? 0f);

    /// <summary> Ожидаемое значение. </summary>
    public float Expected => (Count * (Sides + 1) / 2f) + (Modificator ?? 0f);

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

            float mod = int.Parse(span[(i_d + i_p + 1)..]);
            if (mod != 0)
            {
                Modificator = mod;
            }
        }
    }

    public Dice(int count, int sides, float? modificator = null)
    {
        Count = count;
        Sides = sides;
        Modificator = modificator;
    }
}
