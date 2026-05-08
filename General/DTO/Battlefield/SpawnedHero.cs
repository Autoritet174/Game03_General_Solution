using General.DTO.Entities.Collection;
using System;

namespace General.DTO.Battlefield;

public class SpawnedHero(Hero hero, Guid spawnedId)
{
    public Guid SpawnedId { get; } = spawnedId;

    public Hero Hero { get; set; } = hero;

    #region Характеристики
    public float Health { get; set; }
    public float Strength { get; set; }
    public float Agility { get; set; }
    public float Intelligence { get; set; }
    public float CritChance { get; set; }
    public float CritMultiplier { get; set; }
    public float Haste { get; set; }
    public float Versality { get; set; }
    public float EndurancePhysical { get; set; }
    public float EnduranceMagical { get; set; }
    public float Initiative { get; set; }
    #endregion Характеристики
}
