using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities;

/// <summary>
/// Таблица для связи Hero и CreatureType.
/// </summary>
public class HeroCreatureType
{
    public required Guid HeroId { get; set; }
    public required Guid CreatureTypeId { get; set; }

    [ForeignKey(nameof(HeroId))]
    public required Hero Hero { get; set; }

    [ForeignKey(nameof(CreatureTypeId))]
    public required CreatureType CreatureType { get; set; }
}
