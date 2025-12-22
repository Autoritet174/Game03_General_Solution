using General.DTO.Entities;
using General.DTO.Entities.Collection;
using System.Collections.Generic;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionCache
{
    public List<string> listGroupNameHero = [];
    public List<string> listGroupNameEquipment = [];
    public DtoContainerCollection collection = null!;
}
