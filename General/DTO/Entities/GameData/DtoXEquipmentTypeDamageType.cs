namespace General.DTO.Entities.GameData;

public class DtoXEquipmentTypeDamageType(int equipmentTypeId, int damageTypeId, int damageCoef)
{
    public int EquipmentTypeId { get; } = equipmentTypeId;
    public DtoEquipmentType? EquipmentType { get; } = null;

    public int DamageTypeId { get; } = damageTypeId;
    public DtoDamageType? DamageType { get; } = null;

    public int DamageCoef { get; } = damageCoef;
}
