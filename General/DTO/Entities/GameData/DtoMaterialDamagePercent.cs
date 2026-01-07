namespace General.DTO.Entities.GameData;

public class DtoMaterialDamagePercent(int id, int smithingMaterialId, int DamageTypeId, int percent)
{
    public int Id { get; } = id;
    public int SmithingMaterialId { get; } = smithingMaterialId;
    public DtoSmithingMaterial? SmithingMaterial { get; set; } = null;
    public int DamageTypeId { get; } = DamageTypeId;
    public DtoDamageType? DamageType { get; set; } = null;
    public int Percent { get; } = percent;
}
