namespace Game03Client.DTO;

public class Dice(int count, int sizes, int modificator)
{
    /// <summary> Количество бросаемых кубиков. </summary>
    public int Count { get; } = count;

    /// <summary> Размер кубика (число граней). </summary>
    public int Sizes { get; } = sizes;

    /// <summary> Модификатор к броску кубиков. </summary>
    public int Modificator { get; } = modificator;
}
