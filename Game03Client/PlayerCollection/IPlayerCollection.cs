using General.GameEntities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public interface IPlayerCollection
{
    Task<bool> LoadAllCollectionFromServer(CancellationToken cancellationToken);
    IEnumerable<CollectionHero> GetCollectionHeroesFromCache();
    IEnumerable<GroupHeroes> GetCollectionHeroesGroupByGroups();
    int GetCountHeroes();
}
