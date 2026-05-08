namespace General.DTO.Entities.GameData;

public class X_EquipmentType_DamageType
{
    public int Id { get; set; }
    public int EquipmentTypeId { get; set; }
    public EquipmentType EquipmentType { get; set; } = null!;
    public int DamageTypeId { get; set; }
    public DamageType DamageType { get; set; } = null!;
    public int DamageCoef { get; set; } = 0;
}
