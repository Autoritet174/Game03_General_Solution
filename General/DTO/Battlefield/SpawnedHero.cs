using General.DTO.Entities.GameData;
using System.Text.Json.Serialization;

namespace General.DTO.Battlefield;

public class SpawnedHero
{
    public required Guid SpawnedId { get; init; }
    public required int BaseHeroId { get; init; }
    public required int Level { get; set; }
    public float CoefPowerByLevel { get; set; }

    #region Характеристики
    public required float Health { get; set; }
    public required float HealthMax { get; set; }
    public required float Strength { get; set; }
    public required float Agility { get; set; }
    public required float Intelligence { get; set; }
    public required float CritChance { get; set; }
    public required float CritMultiplier { get; set; }
    public required float Haste { get; set; }
    public required float Versality { get; set; }
    public required float EndurancePhysical { get; set; }
    public required float EnduranceMagical { get; set; }
    public required float Initiative { get; set; }
    public bool IsAlive { get; set; } = true;
    #endregion Характеристики

    [JsonIgnore]
    public float HealthPercent
    {
        get
        {
            if (HealthMax > 0 && Health > 0)
            {
                return Health / HealthMax;
            }
            else
            {
                return 0;
            }
        }
    }
}
