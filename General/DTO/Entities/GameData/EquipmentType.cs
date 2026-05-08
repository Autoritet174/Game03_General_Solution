using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

public class EquipmentType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public int MassPhysical { get; set; } = 0;
    public int MassMagical { get; set; } = 0;
    public ESlotType SlotTypeId { get; set; }
    public SlotType SlotType { get; set; } = null!;
    public bool CanCraftSmithing { get; set; } = false;
    public bool CanCraftJewelcrafting { get; set; } = false;
    public int SpendActionPoints { get; set; } = 0;
    public bool? BlockOtherHand { get; set; }
    public Dice? Damage { get; set; }
    public Dictionary<EStatType, Dice>? PossibleStats { get; set; }
}
