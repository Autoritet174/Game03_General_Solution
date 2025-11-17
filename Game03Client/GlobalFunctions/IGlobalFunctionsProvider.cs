using General.GameEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game03Client.GlobalFunctions;
public interface IGlobalFunctionsProvider
{
    /// <summary>
    /// Загрузить список всех героев.
    /// </summary>
    /// <returns></returns>
    Task LoadListAllHeroes();

    /// <summary>
    /// Получить список героев.
    /// </summary>
    /// <returns></returns>
    IEnumerable<HeroBaseEntity> AllHeroes { get; }
}
