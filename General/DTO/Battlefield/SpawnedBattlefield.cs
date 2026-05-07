using System;
using System.Collections.Generic;
using System.Text;

namespace General.DTO.Battlefield;

public class SpawnedBattlefield(EBattleFiled eBattleFiled, List<SpawnedHero> spawnedHeroes, List<SpawnedNpc> spawnedNpcs)
{
    public EBattleFiled eBattleFiled { get; } = eBattleFiled;
    public List<SpawnedHero> SpawnedHeroes { get; } = spawnedHeroes;
    public List<SpawnedNpc> SpawnedNpcs { get; } = spawnedNpcs;
}
