namespace General.DTO;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, int mass, int slotTypeId, bool canCraftJewelcrafting, bool canCraftSmithing, Dice? health, Dice? attack)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public int Mass { get; } = mass;
    public int SlotTypeId { get; } = slotTypeId;
    public bool CanCraftJewelcrafting { get; } = canCraftJewelcrafting;
    public bool CanCraftSmithing { get; } = canCraftSmithing;
    public Dice? Health { get; } = health;
    public Dice? Attack { get; } = attack;
}
