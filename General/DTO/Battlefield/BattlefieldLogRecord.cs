using System.Text.Json.Serialization;

namespace General.DTO.Battlefield;

[JsonPolymorphic]
//[JsonDerivedType(typeof(BattlefieldLogRecord), "base")]
[JsonDerivedType(typeof(BattlefieldLogRecord_TurnStart), "turn_start")]
[JsonDerivedType(typeof(BattlefieldLogRecord_ChangeActionPoints), "change_ap")]
[JsonDerivedType(typeof(BattlefieldLogRecord_Damage), "damage")]
[JsonDerivedType(typeof(BattlefieldLogRecord_UseAbility), "use_ability")]
public abstract class BattlefieldLogRecordBase
{
    public abstract int index { get; set; }
}

//public class BattlefieldLogRecord : BattlefieldLogRecordBase
//{
//    public override int Index { get; set; }
//    public required COMBAT_LOG_EVENT Action { get; init; }

//    public EBattlefieldLogAbility? Ability { get; init; }

//    public Guid? H1 { get; init; }
//    public Guid? H2 { get; init; }

//    public float? FloatValue1 { get; init; }
//    public int? IntValue1 { get; init; }
//    public bool? BoolValue1 { get; init => field = value == true ? true : null; } = null;
//    public bool? BoolValue2 { get; init => field = value == true ? true : null; } = null;
//}

public class BattlefieldLogRecord_TurnStart : BattlefieldLogRecordBase
{
    public override int index { get; set; }
    public required int turn { get; init; }
}

public class BattlefieldLogRecord_ChangeActionPoints : BattlefieldLogRecordBase
{
    public override int index { get; set; }
    public required Guid spawnedHeroId { get; init; }
    public required int countAP { get; init; }
}

public class BattlefieldLogRecord_Damage : BattlefieldLogRecordBase
{
    public override int index { get; set; }
    public required int indexReason { get; set; }
    public required Guid hero1Id { get; init; }
    public required Guid hero2Id { get; init; }
    public required float damage { get; init; }
    public bool isCrit { get; init; }
    public bool isPerodic { get; init; }
}

public class BattlefieldLogRecord_UseAbility : BattlefieldLogRecordBase
{
    public override int index { get; set; }
    public required Guid spawnedHero1Id { get; init; }
    public required EBattlefieldLogAbility ability { get; init; }
    public Guid[]? spawnedHeroTargets { get; init; }
    public float[]? damage { get; init; }
}
