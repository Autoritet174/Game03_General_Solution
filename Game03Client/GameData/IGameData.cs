using General.DTO.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.GameData;

/// <summary> Определяет интерфейс для поставщика глобальных функций. </summary>
public interface IGameData
{
    /// <summary> Асинхронно загружает все константные игровые данные. </summary>
    Task LoadGameData(CancellationToken cancellationToken);
    DtoContainerGameData GetDtoContainer();
}
