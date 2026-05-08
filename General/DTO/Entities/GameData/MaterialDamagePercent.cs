namespace General.DTO.Entities.GameData;

public class MaterialDamagePercent
{
    public int Id { get; set; }
    public int SmithingMaterialsId { get; set; }
    public SmithingMaterial SmithingMaterials { get; set; } = null!;
    public int DamageTypeId { get; set; }
    public DamageType DamageType { get; set; } = null!;
    public int Percent { get; set; }
}
