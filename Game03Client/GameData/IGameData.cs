using Game03Client.DTO;
using General.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.GameData;

/// <summary> Определяет интерфейс для поставщика глобальных функций. </summary>
public interface IGameData
{
    /// <summary> Асинхронно загружает все константные игровые данные. </summary>
    Task LoadGameData(CancellationToken cancellationToken);

    IEnumerable<DtoBaseHero> BaseHeroes { get; }
    IEnumerable<DtoBaseEquipment> BaseEquipments { get; }
    IEnumerable<DtoSlotType> SlotTypes { get; }
    IEnumerable<DtoEquipmentType> EquipmentTypes { get; }

    /// <summary> Возвращает базовую сущность героя по его уникальному идентификатору. </summary>
    DtoBaseHero GetHeroById(int id);
}
