using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities;

/// <summary>
/// Экипировка. Меч.
/// </summary>
internal class EquipmentSword : Entity
{
    public required string Attack { get; set; }
    public required int TypeDamageId { get; set; }

    [ForeignKey(nameof(TypeDamageId))]
    public required TypeDamage TypeDamage { get; set; }

}
