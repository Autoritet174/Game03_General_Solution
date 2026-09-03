namespace General.DTO.Entities.GameData;

public class MaterialDamagePercent
{
    public int id { get; set; }
    public int smithingMaterialsId { get; set; }
    public SmithingMaterial smithingMaterials { get; set; } = null!;
    public int damageTypeId { get; set; }
    public DamageType damageType { get; set; } = null!;
    public int percent { get; set; }
}
