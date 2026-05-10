using System.Collections.Generic;

namespace General.DTO.Battlefield;

public class SpawnedBattlefield(EBattleFiled eBattleFiled, List<SpawnedHero> SpawnedHeroPlayerList, List<SpawnedHero> SpawnedHeroEnemyList)
{
    public EBattleFiled eBattleFiled { get; } = eBattleFiled;
    public List<SpawnedHero> SpawnedHeroPlayerList { get; } = SpawnedHeroPlayerList;
    public List<SpawnedHero> SpawnedHeroEnemyList { get; } = SpawnedHeroEnemyList;
}
