using General.DTO.Entities.Collection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public interface IPlayerCollection
{
    Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken);
    IEnumerable<DtoHero> GetCollectionHeroesFromCache();
    IEnumerable<DtoEquipment> GetCollectionEquipmentsFromCache();
    IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames();
    IEnumerable<GroupCollectionElement> GetCollectionEquipmentesGroupByGroups();
    int GetCountHeroes();
    int GetCountEquipments();
}
