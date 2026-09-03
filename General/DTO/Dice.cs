using System.Text.Json.Serialization;

namespace General.DTO;

/// <summary>
/// Класс, представляющий бросок кубиков в формате "2d3+5",
/// </summary>
public record Dice
{
    /// <summary> Count. Количество бросаемых кубиков. </summary>
    [JsonPropertyName("c")]
    public int count { get; set; }

    /// <summary> Sides. Размер кубика (число граней). </summary>
    [JsonPropertyName("d")] // названо этой буквой потому что postgres сортирует jsonb по алфавиту, сортировку задаём для наглядности
    public int sides { get; set; }

    /// <summary> Modificator. Модификатор к броску кубиков. </summary>
    [JsonPropertyName("m")]
    public float? modificator { get; set; } = null;

    /// <summary> Минимальное значение. </summary>
    [JsonIgnore]
    public float min => count + (modificator ?? 0f);

    /// <summary> Максимальное значение. </summary>
    [JsonIgnore]
    public float max => (count * sides) + (modificator ?? 0f);

    /// <summary> Ожидаемое значение. </summary>
    [JsonIgnore]
    public float expected => (count * (sides + 1) / 2f) + (modificator ?? 0f);

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

        count = int.Parse(span[..i_d]);

        int i_p = span[i_d..].IndexOf('+');
        int i_d1 = i_d + 1;

        if (i_p == -1)
        {
            sides = int.Parse(span[i_d1..]);
        }
        else
        {
            sides = int.Parse(span[i_d1..(i_d + i_p)]);

            float mod = int.Parse(span[(i_d + i_p + 1)..]);
            if (mod != 0)
            {
                modificator = mod;
            }
        }
    }

    public Dice(int count, int sides, float? modificator = null)
    {
        this.count = count;
        this.sides = sides;
        this.modificator = modificator;
    }

    public float GetRandomValue()
    {
        if (count < 1 || sides < 1)
        {
            return modificator ?? 0f;
        }

        if (sides == 1)
        {
            return count + (modificator ?? 0f);
        }

        int sum = count;
        for (int i = 0; i < count; i++)
        {
            sum += RandomShared.Next(sides);
        }

        return sum + (modificator ?? 0f);
    }

    public string ToStr()
    {
        return $"{count}d{sides}{(modificator != null && modificator != 0 ? (modificator < 0 ? modificator.ToString() : "+" + modificator.ToString()) : "")}";
    }
}
