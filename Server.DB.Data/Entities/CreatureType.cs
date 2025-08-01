namespace Server.DB.Data.Entities;

public class CreatureType
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public required string Name { get; set; }
    //public ICollection<HeroCreatureType> Heroes { get; set; } = []; // Обратная навигация
}
