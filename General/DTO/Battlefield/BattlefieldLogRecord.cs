namespace General.DTO.Battlefield;

public class BattlefieldLogRecord
{

    /// <summary> Индекс лога. </summary>
    public required int Index { get; init; }
    public required EBattlefieldLogAction Action { get; init; }
    //public int? Turn { get; init; }

    public EBattlefieldLogAbility? Ability { get; init; }

    public Guid? H1 { get; init; }
    public Guid? H2 { get; init; }

    public float? FloatValue1 { get; init; }
    public int? IntValue1 { get; init; }

    public bool? BoolValue1 { get; init => field = value == true ? true : null; } = null;
}
