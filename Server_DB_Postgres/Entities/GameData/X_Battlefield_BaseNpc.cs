using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Таблица для связи Hero и CreatureType. </summary>
[Table(nameof(DbContextGame.x_Battlefields_BaseNpcs), Schema = nameof(GameData))]
public class X_Battlefield_BaseNpc
{
    public int Id { get; init; }

    public int BattlefieldId { get; set; }
    [ForeignKey(nameof(BattlefieldId))]
    public Battlefield Battlefield { get; set; } = null!;

    public int BaseNpcId { get; set; }
    [ForeignKey(nameof(BaseNpcId))]
    public BaseNpc BaseNpc { get; set; } = null!;

    /// <summary> Гарантированное появление на поле боя. </summary>
    public bool GuarantSpawn { get; set; }

    /// <summary> Вероятность появления на поле боя. Чем выше число тем выше шанс. Игнорируется при GuarantSpawn=true. </summary>
    public int ProbabilitySpawn { get; set; }

    /// <summary> При респауне может иметь ранг. </summary>
    public bool PossibleRank { get; set; }
}
