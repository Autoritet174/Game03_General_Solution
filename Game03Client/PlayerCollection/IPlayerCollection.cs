using Game03Client.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public interface IPlayerCollection
{
    Task<bool> LoadAllCollectionFromServer(CancellationToken cancellationToken);
    IEnumerable<DtoCollectionHero> GetCollectionHeroesFromCache();
    IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames();
    int GetCountHeroes();
}
