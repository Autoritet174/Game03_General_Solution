using General.DTO.Entities.GameData;
using System.Collections.Generic;

namespace General.DTO;

public class DtoBattlefieldData(string name, List<Guid> playerHeroes, List<BaseHero> enemyNpc)
{
    public string name { get; set; } = name;
    public List<Guid> playerHeroes { get; set; } = playerHeroes;
    public List<BaseHero> enemyNpc { get; set; } = enemyNpc;
}
