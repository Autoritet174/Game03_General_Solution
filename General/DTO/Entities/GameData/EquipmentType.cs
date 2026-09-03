using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

public class EquipmentType
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string nameRu { get; set; } = string.Empty;
    public int massPhysical { get; set; } = 0;
    public int massMagical { get; set; } = 0;
    public ESlotType slotTypeId { get; set; }
    public SlotType slotType { get; set; } = null!;
    public bool canCraftSmithing { get; set; } = false;
    public bool canCraftJewelcrafting { get; set; } = false;
    public int spendActionPoints { get; set; } = 0;
    public bool? blockOtherHand { get; set; }
    public Dice? damage { get; set; }
    public Dictionary<EStatType, Dice>? possibleStats { get; set; }
}
