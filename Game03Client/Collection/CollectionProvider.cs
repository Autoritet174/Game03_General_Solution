using General.DTO;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.Collection;

public class CollectionProvider
{

    private static readonly List<string> listGroupNameHero = [];
    private static readonly List<string> listGroupNameEquipment = [];
    private static DtoContainerCollection collection = null!;
    private static readonly Logger<CollectionProvider> logger = new();

    public static async Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogError("LoadAllCollectionFromServerAsync cancelled");
            return false;
        }

        // Получить коллекцию героев игрока
        string? response = await HttpRequester.GetResponseAsync(General.Url.Collection.ALL, null, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(response))
        {
            logger.LogError("response is null or empty");
            return false;
        }
        DtoContainerCollection? c = JsonConvert.DeserializeObject<DtoContainerCollection>(response);

        if (c == null)
        {
            logger.LogError("c is null");
            return false;
        }

        IEnumerable<DtoBaseEquipment> baseEquipments = GameData.Container.BaseEquipments;
        foreach (DtoEquipment i in c.CollectionEquipments)
        {
            i.BaseEquipment = baseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        IEnumerable<DtoBaseHero> baseHeroes = GameData.Container.BaseHeroes;
        foreach (DtoHero i in c.CollectionHeroes)
        {
            i.BaseHero = baseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
        }

        IEnumerable<DtoSlot> slots = GameData.Container.Slots;
        foreach (DtoEquipment i in c.CollectionEquipments)
        {
            i.Hero = c.CollectionHeroes.FirstOrDefault(a => a.Id == i.HeroId);
        }


        collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();

        // Сортировка героев по редкости, уровню и имени
        c.CollectionHeroes = [.. c.CollectionHeroes
            .OrderByDescending(a => a.Rarity)
            .ThenBy(a => a.Level)
            .ThenBy(a =>
            {
                if (a.BaseHero == null)
                {
                    logger.LogError("a.DtoBaseHero is null");
                    throw new Exception();
                }
                return a.BaseHero.Name;
            })];

        // Сортировка экипировки по редкости и имени

        c.CollectionEquipments = [.. c.CollectionEquipments
            .OrderByDescending(a =>
            {
                if (a.BaseEquipment == null)
                {
                    logger.LogError("a.DtoBaseEquipment is null");
                    throw new Exception();
                }
                return a.BaseEquipment.Rarity;
            })
            .ThenBy(a =>
            {
                if (a.BaseEquipment == null)
                {
                    logger.LogError("a.DtoBaseEquipment is null");
                    throw new Exception();
                }
                return a.BaseEquipment.Name;
            })];

        return true;
    }

    public static void RefreshListGroupNameHero()
    {
        List<string> list = listGroupNameHero;
        list.Clear();
        list.Add(string.Empty);
        foreach (DtoHero i in collection.CollectionHeroes)
        {
            string group_name = i.GroupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static void RefreshListGroupNameEquipment()
    {
        List<string> list = listGroupNameEquipment;
        list.Clear();
        list.Add(string.Empty);
        foreach (DtoEquipment i in collection.CollectionEquipments)
        {
            string group_name = i.GroupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static IEnumerable<DtoHero> GetCollectionHeroesFromCache() => collection.CollectionHeroes;
    public static IEnumerable<DtoEquipment> GetCollectionEquipmentsFromCache() => collection.CollectionEquipments;

    public static int GetCountHeroes() => collection.CollectionHeroes.Count();
    public static int GetCountEquipments() => collection.CollectionEquipments.Count();

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
    {
        List<GroupCollectionElement> result = [];
        IEnumerable<DtoHero> c = collection.CollectionHeroes;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }

        foreach (string groupName in listGroupNameHero)
        {
            IEnumerable<DtoHero> heroes = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (DtoHero hero in heroes)
            {
                if (hero.BaseHero == null)
                {
                    logger.LogError("hero.DtoBaseHero is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(hero.Id, hero.BaseHeroId, hero.Rarity, hero.BaseHero.Name, hero.BaseHero.IsUnique, TypeCollectionElement.Hero));
            }

            GroupCollectionElement groupCollectionElement = new(groupName, collectionElements);
            result.Add(groupCollectionElement);
            if (groupName == string.Empty)
            {
                groupCollectionElement.Priority = -1;
            }
        }
        return result.OrderByDescending(a => a.Priority);
    }

    /// <summary> Получить коллекцию экипировки сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionEquipmentesGroupByGroups(int page)
    {
        List<GroupCollectionElement> result = [];
        IEnumerable<DtoEquipment> c = collection.CollectionEquipments;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }
        foreach (string groupName in listGroupNameEquipment)
        {
            IEnumerable<DtoEquipment> equipments = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (DtoEquipment equipment in equipments)
            {
                if (equipment.BaseEquipment == null)
                {
                    logger.LogError("Equipment.DtoBaseEquipment is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(equipment.Id, equipment.BaseEquipmentId, equipment.BaseEquipment.Rarity, equipment.BaseEquipment.Name, equipment.BaseEquipment.IsUnique, TypeCollectionElement.Equipment));
            }

            GroupCollectionElement groupCollectionElement = new(groupName, collectionElements);
            result.Add(groupCollectionElement);
            if (groupName == string.Empty)
            {
                groupCollectionElement.Priority = -1;
            }
        }
        return result.OrderByDescending(a => a.Priority);
    }

    public static bool EquipmentIsEquipped(Guid equipmentId)
    {
        DtoEquipment? equipment = collection.CollectionEquipments.FirstOrDefault(a => a.Id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        return equipment.HeroId != null;
    }

    public static async Task<bool> EquipmentTakeOnAsync(Guid equipmentId, Guid heroId, bool? inAltSlot, CancellationToken cancellationToken)
    {
        DtoEquipment? equipment = collection.CollectionEquipments.FirstOrDefault(a => a.Id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        DtoHero? hero = collection.CollectionHeroes.FirstOrDefault(a => a.Id == heroId);
        if (hero == null)
        {
            logger.LogError("Hero not found in collection. Id: {heroId}", heroId.ToString());
            return false;
        }

        if (equipment.HeroId != null)
        {
            logger.LogError("Equipment is not equipped. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }

        DtoWSEquipmentTakeOnC2S equipmentTakeOnMessage = new(heroId, equipmentId, inAltSlot, Guid.NewGuid());
        bool result = await WebSocketProvider.SendMessageAsync(equipmentTakeOnMessage, cancellationToken).ConfigureAwait(false);
        if (result)
        {
            // тут мы знаем только то что сообщение ушло на сервер, но пока что понятие не имеем что сделал сервер
            // ждём сообщение по веб сокету от сервера о том была ли фактически одета экипировка
            // Ждем ответ не более 2 секунд

            var response = await WebSocketProvider.WaitForResponseAsync(
                equipmentTakeOnMessage.MessageId.Value,
                TimeSpan.FromSeconds(2),
                cancellationToken).ConfigureAwait(false);

            if (response?.Success == true)
            {
                // Обновляем локальное состояние
                equipment.HeroId = heroId;
                return true;
            }
            return true;
        }


        return false;
    }
}
