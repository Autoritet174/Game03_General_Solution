using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

public class DtoEquipmentType(int id, string name, int massPhysical, int massMagical, ESlotType slotTypeId, bool canCraftSmithing, bool canCraftJewelcrafting, int spendActionPoints, bool? blockOtherHand, DtoSlotType? slotType, Dictionary<EStatType, Dice>? PossibleStats)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int MassPhysical { get; } = massPhysical;
    public int MassMagical { get; } = massMagical;
    public ESlotType SlotTypeId { get; } = slotTypeId;
    public DtoSlotType? SlotType { get; set; } = slotType;
    public bool CanCraftSmithing { get; } = canCraftSmithing;
    public bool CanCraftJewelcrafting { get; } = canCraftJewelcrafting;
    public int SpendActionPoints { get; } = spendActionPoints;
    public bool? BlockOtherHand { get; } = blockOtherHand;
    public Dictionary<EStatType, Dice>? PossibleStats { get; set; } = PossibleStats;
}
