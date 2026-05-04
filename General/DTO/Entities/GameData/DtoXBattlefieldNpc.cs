using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

public class DtoXBattlefieldNpc(int id, int battlefieldId, int baseNpcId, bool guarantSpawn, int probabilitySpawn, bool possibleRank)
{
    public int Id { get; } = id;

    public int BattlefieldId { get; } = battlefieldId;
    [ForeignKey(nameof(BattlefieldId))]
    public DtoBattlefield Battlefield { get; set; } = null!;

    public int BaseNpcId { get; } = baseNpcId;
    [ForeignKey(nameof(BaseNpcId))]
    public DtoBaseNpc BaseNpc { get; set; } = null!;

    /// <summary> Гарантированное появление на поле боя. </summary>
    public bool GuarantSpawn { get; } = guarantSpawn;

    /// <summary> Вероятность появления на поле боя. Чем выше число тем выше шанс. Игнорируется при GuarantSpawn=true. </summary>
    public int ProbabilitySpawn { get; } = probabilitySpawn;

    /// <summary> При респауне может иметь ранг. </summary>
    public bool PossibleRank { get; } = possibleRank;
}
