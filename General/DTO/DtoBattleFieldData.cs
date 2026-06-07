using General.DTO.Entities.GameData;
using System;
using System.Collections.Generic;

namespace General.DTO;

public class DtoBattlefieldData(string Name, List<Guid> PlayerHeroes, List<BaseHero> EnemyNpc)
{
    public string Name { get; set; } = Name;
    public List<Guid> PlayerHeroes { get; set; } = PlayerHeroes;
    public List<BaseHero> EnemyNpc { get; set; } = EnemyNpc;
}
