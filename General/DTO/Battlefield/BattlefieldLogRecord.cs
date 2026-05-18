namespace General.DTO.Battlefield;

public class BattlefieldLogRecord
{

    /// <summary> Индекс лога. </summary>
    public int Index { get; init; }
    public int? BattlefieldTurn { get; init; }
    public EAbility? eAbility { get; init; }

    /// <summary> Герой нанёсший урон. </summary>
    public Guid? H1 { get; init; }

    /// <summary> Герой получивший урон. </summary>
    public Guid? H2 { get; init; }

    public float? Damage { get; init; }

    public bool IsCrit { get; set; } = false;
}
