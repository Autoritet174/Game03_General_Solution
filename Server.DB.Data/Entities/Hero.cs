using General.GameEntities;
using System.ComponentModel.DataAnnotations;

namespace Server.DB.Data.Entities;

public class Hero
{
   
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public required string Name { get; set; }

    public required HeroBaseEntity.RarityLevel Rarity { get; set; }

    public ICollection<HeroCreatureType> CreatureTypes { get; set; } = [];

}
