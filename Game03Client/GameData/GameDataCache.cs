using General.DTO.Entities;
using System.Collections.Generic;

namespace Game03Client.GameData;

/// <summary>
/// Класс для кэширования глобальных данных, используемых поставщиком функций.
/// </summary>
internal class GameDataCache
{
    internal DtoContainerGameData DtoContainer = null!;
}
