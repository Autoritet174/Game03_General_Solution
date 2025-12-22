using General.DTO.Entities.Collection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public interface IPlayerCollection
{
    Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken);
    IEnumerable<DtoHero> GetCollectionHeroesFromCache();
    IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames();
    int GetCountHeroes();
}
