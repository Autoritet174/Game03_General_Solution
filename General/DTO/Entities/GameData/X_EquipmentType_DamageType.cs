namespace General.DTO.Entities.GameData;

public class X_EquipmentType_DamageType
{
    public int id { get; set; }
    public int equipmentTypeId { get; set; }
    public EquipmentType equipmentType { get; set; } = null!;
    public int damageTypeId { get; set; }
    public DamageType damageType { get; set; } = null!;
    public int damageCoef { get; set; } = 0;
}
