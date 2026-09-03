using System.Collections.Generic;

namespace General.DTO.Battlefield;

public class SpawnedBattlefield(EBattleFiled eBattleFiled, List<SpawnedHero> SpawnedHeroPlayerList, List<SpawnedHero> SpawnedHeroEnemyList)
{
    public EBattleFiled eBattleFiled { get; } = eBattleFiled;
    public List<SpawnedHero> spawnedHeroPlayerList { get; } = SpawnedHeroPlayerList;
    public List<SpawnedHero> spawnedHeroEnemyList { get; } = SpawnedHeroEnemyList;
    public required List<BattlefieldLogRecordBase> battlefieldLog { get; set; }
}
