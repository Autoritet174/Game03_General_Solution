using General.DTO.Entities.Collection;
using System.Collections.Generic;
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
