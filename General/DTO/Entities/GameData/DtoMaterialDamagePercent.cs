using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

public class DtoMaterialDamagePercent(int id, int dtoSmithingMaterialId, int dtoDamageTypeId, int percent)
{
    public int Id { get; } = id;
    public int DtoSmithingMaterialId { get; } = dtoSmithingMaterialId;
    public DtoSmithingMaterial? DtoSmithingMaterial { get; set; } = null;
    public int DtoDamageTypeId { get; } = dtoDamageTypeId;
    public DtoDamageType? DtoDamageType { get; set; } = null;
    public int Percent { get; } = percent;
}
