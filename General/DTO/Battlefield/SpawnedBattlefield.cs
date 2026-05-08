using System.Collections.Generic;

namespace General.DTO.Battlefield;

public class SpawnedBattlefield(EBattleFiled eBattleFiled, List<SpawnedHero> spawnedHeroesPlayer, List<SpawnedHero> spawnedHeroesEnemy)
{
    public EBattleFiled eBattleFiled { get; } = eBattleFiled;
    public List<SpawnedHero> SpawnedHeroesPlayer { get; } = spawnedHeroesPlayer;
    public List<SpawnedHero> SpawnedHeroesEnemy { get; } = spawnedHeroesEnemy;
}
