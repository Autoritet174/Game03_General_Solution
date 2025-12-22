namespace General.DTO.Entities.GameData;

public class DtoEquipmentType(int id, string name, int massPhysical, int massMagical, int dtoSlotTypeId, bool canCraftSmithing, bool canCraftJewelcrafting, int spendActionPoints, bool? blockOtherHand)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int MassPhysical { get; } = massPhysical;
    public int MassMagical { get; } = massMagical;
    public int DtoSlotTypeId { get; } = dtoSlotTypeId;
    public DtoSlotType? DtoSlotType { get; set; } = null;
    public bool CanCraftSmithing { get; } = canCraftSmithing;
    public bool CanCraftJewelcrafting { get; } = canCraftJewelcrafting;
    public int SpendActionPoints { get; } = spendActionPoints;
    public bool? BlockOtherHand { get; } = blockOtherHand;
}
