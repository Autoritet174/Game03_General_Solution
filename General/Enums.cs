namespace General;

public enum ESlotType : int
{
    None = 0,

    /// <summary> Оружие </summary>
    Weapon = 1,

    /// <summary> Щит </summary>
    Shield = 2,

    /// <summary> Голова </summary>
    Head = 4,

    /// <summary> Доспех </summary>
    Armor = 6,

    /// <summary> Руки </summary>
    Hands = 7,

    /// <summary> Ступни </summary>
    Feet = 9,

    /// <summary> Браслет </summary>
    Bracelet = 10,

    /// <summary> Кольцо </summary>
    Ring = 14,

    /// <summary> Аксессуар </summary>
    Trinket = 16,

    /// <summary> Шея </summary>
    Neck = 17
}

public enum EStatType : int
{
    None = 0,
    Health = 1,
    Damage = 2,
    Strength = 3,
    Agility = 4,
    Intelligence = 5,
    CritChance = 6,
    CritMultiplier = 7,
    Haste = 8,
    Versality = 9,
    Initiative = 10
}

public enum ESlot : int
{
    RightHand = 1,
    LeftHand = 2,
    Head = 3,
    Armor = 4,
    Hands = 5,
    Feet = 6,
    Bracelet = 7,
    Ring1 = 8,
    Ring2 = 9,
    Trinket1 = 10,
    Trinket2 = 11,
    Neck = 12
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
    Universal = 0,
    Strength = 1,
    Agility = 2,
    Intelligence = 3
}

public enum ERank : int
{
    None = 0,
    Elite = 1,
    Boss = 2
}

public enum EDungeonDifficulty : int
{
    Normal = 0,
    Hard = 1,
    Boss = 2
}
