namespace General;

public enum ESlotType : int
{
    none = 0,

    /// <summary> Оружие </summary>
    weapon = 1,

    /// <summary> Щит </summary>
    shield = 2,

    /// <summary> Голова </summary>
    head = 4,

    /// <summary> Доспех </summary>
    armor = 6,

    /// <summary> Руки </summary>
    hands = 7,

    /// <summary> Ступни </summary>
    feet = 9,

    /// <summary> Браслет </summary>
    bracelet = 10,

    /// <summary> Кольцо </summary>
    ring = 14,

    /// <summary> Аксессуар </summary>
    trinket = 16,

    /// <summary> Шея </summary>
    neck = 17
}

public enum EStatType : int
{
    none = 0,
    health = 1,
    damage = 2,
    strength = 3,
    agility = 4,
    intelligence = 5,
    critChance = 6,
    critMultiplier = 7,
    haste = 8,
    versality = 9,
    initiative = 10
}

public enum ESlot : int
{
    rightHand = 1,
    leftHand = 2,
    head = 3,
    armor = 4,
    hands = 5,
    feet = 6,
    bracelet = 7,
    ring1 = 8,
    ring2 = 9,
    trinket1 = 10,
    trinket2 = 11,
    neck = 12
}

//public enum int : int
//{
//    Common = 1,
//    Uncommon = 2,
//    Rare = 3,
//    Epic = 4,
//    Legendary = 5,
//    Mythic = 6,
//    Divine = 7,
//    Absolute = 8
//}

public enum EMainStat : int
{
    universal = 0,
    strength = 1,
    agility = 2,
    intelligence = 3
}

public enum ERank : int
{
    none = 0,
    amplified = 1,
    elite = 2,
    champion = 3,
}

//public enum EBattlefieldDifficulty : int
//{
//    Normal = 0,
//    Hard = 1,
//    Boss = 2
//}

public enum EBattleFiled
{
    TestPlatforms__Polygon = 1,
    Mines__Iron = 2,
}

public enum COMBAT_LOG_EVENT
{
    startTurn = 1,
    changeActionPoints = 2,
    useAbility = 3,
    changeHealth_Damage = 4,
    changeHealth_Healing = 5,

}

public enum EBattlefieldLogAbility
{
    attack = 1,
    healing = 2,
}
