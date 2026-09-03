using System.Text.Json.Serialization;

namespace General.DTO.Battlefield;

public class SpawnedHero
{
    public required Guid spawnedId { get; init; }
    public required int baseHeroId { get; init; }
    public required int level { get; set; }
    public float coefPowerByLevel { get; set; }
    public int actionPoints { get; set; }
    public int team { get; set; }

    #region Характеристики
    public required float health { get; set; }
    public required float healthMax { get; set; }
    public required float damage { get; set; }
    public required float strength { get; set; }
    public required float agility { get; set; }
    public required float intelligence { get; set; }
    public required float critChance { get; set; }
    public required float critMultiplier { get; set; }
    public required float haste { get; set; }
    public required float versality { get; set; }
    public required float endurancePhysical { get; set; }
    public required float enduranceMagical { get; set; }
    public required float initiative { get; set; }
    //public bool IsAlive { get; set; } = true;
    #endregion Характеристики

    [JsonIgnore]
    public float healthPercent
    {
        get
        {
            if (healthMax > 0 && health > 0)
            {
                return health / healthMax;
            }
            else
            {
                return 0;
            }
        }
    }
}
