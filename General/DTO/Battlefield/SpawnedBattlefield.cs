using System.Collections.Generic;

namespace General.DTO.Battlefield;

public class SpawnedBattlefield(EBattlefield eBattleFiled, List<SpawnedHero> SpawnedHeroPlayerList, List<SpawnedHero> SpawnedHeroEnemyList)
{
    public EBattlefield eBattleFiled { get; } = eBattleFiled;
    public List<SpawnedHero> spawnedHeroPlayerList { get; } = SpawnedHeroPlayerList;
    public List<SpawnedHero> spawnedHeroEnemyList { get; } = SpawnedHeroEnemyList;
    public required List<BattlefieldLogRecordBase> battlefieldLog { get; set; }
}
