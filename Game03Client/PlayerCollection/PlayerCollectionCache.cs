using System;
using System.Collections.Generic;
using System.Text;
using Game03Client.DTO;
using General.GameEntities;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionCache
{
    public List<DtoCollectionHero> listHero = [];
    public List<string> listHeroGroupName = [];

    public List<DtoCollectionEquipment> listEquipment = [];
    public List<string> listEquipmentGroupName = [];
}
