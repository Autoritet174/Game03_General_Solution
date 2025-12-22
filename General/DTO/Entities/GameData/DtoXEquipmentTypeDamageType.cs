namespace General.DTO.Entities.GameData;

public class DtoXEquipmentTypeDamageType(int dtoEquipmentTypeId, int dtoDamageTypeId, int damageCoef)
{
    public int DtoEquipmentTypeId { get;  } = dtoEquipmentTypeId;
    public DtoEquipmentType? DtoEquipmentType { get; set; } = null;

    public int DtoDamageTypeId { get; } = dtoDamageTypeId;
    public DtoDamageType? DtoDamageType { get; set; } = null;

    public int DamageCoef { get; } = damageCoef;
}
