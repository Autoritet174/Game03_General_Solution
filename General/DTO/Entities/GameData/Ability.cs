using System.ComponentModel;

namespace General.DTO.Entities.GameData;

/// <summary>Способность героя или нпс.</summary>
public class Ability(int id, string name, int cost, EBattlefieldLogAbility code, int cooldown)
{
    public int id { get; set; } = id;
    public string name { get; set; } = name;
    public int cost { get; set; } = cost;
    public EBattlefieldLogAbility code { get; set; } = code;
    public int cooldown { get; set; } = cooldown;
}
