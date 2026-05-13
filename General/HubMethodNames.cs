namespace General;

public static class HubMethodNames
{
    public const string PING = "Ping";
    public const string EQUIPMENT_TAKE_ON = "EquipmentTakeOn";
    public const string EQUIPMENT_TAKE_OFF = "EquipmentTakeOff";
    public const string COMBAT_START = "CombatStart";
    public const string COMBAT_BREAK = "CombatBreak";

    public enum EMethod { PING, EQUIPMENT_TAKE_ON, EQUIPMENT_TAKE_OFF, COMBAT_START, COMBAT_BREAK }

    public static string GetMethod(EMethod eMethod)
    {
        return eMethod switch
        {
            EMethod.PING => PING,
            EMethod.EQUIPMENT_TAKE_ON => EQUIPMENT_TAKE_ON,
            EMethod.EQUIPMENT_TAKE_OFF => EQUIPMENT_TAKE_OFF,
            EMethod.COMBAT_START => COMBAT_START,
            EMethod.COMBAT_BREAK => COMBAT_BREAK,
            _ => throw new Exception("HubMethodNames.GetMethod bad method"),
        };
    }

}
