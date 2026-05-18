namespace General;

public static class HubMethodNames
{
    public const string PING = "Ping";
    public const string EQUIPMENT_TAKE_ON = "EquipmentTakeOn";
    public const string EQUIPMENT_TAKE_OFF = "EquipmentTakeOff";
    public const string COMBAT_START = "CombatStart";
    public const string COMBAT_BREAK = "CombatBreak";
    public const string USE_ABILITY = "UseAbility";
    public const string GET_BATTLE_LOG = "GetBattleLog";

    public enum EMethod { PING, EQUIPMENT_TAKE_ON, EQUIPMENT_TAKE_OFF, COMBAT_START, COMBAT_BREAK,
        USE_ABILITY, GET_BATTLE_LOG }

    public static string GetMethod(EMethod eMethod)
    {
        return eMethod switch
        {
            EMethod.PING => PING,
            EMethod.EQUIPMENT_TAKE_ON => EQUIPMENT_TAKE_ON,
            EMethod.EQUIPMENT_TAKE_OFF => EQUIPMENT_TAKE_OFF,
            EMethod.COMBAT_START => COMBAT_START,
            EMethod.COMBAT_BREAK => COMBAT_BREAK,
            EMethod.USE_ABILITY => USE_ABILITY,
            EMethod.GET_BATTLE_LOG => GET_BATTLE_LOG,
            _ => throw new Exception("HubMethodNames.GetMethod bad method"),
        };
    }

}
