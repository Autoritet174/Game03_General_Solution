using General.GameEntities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.GameData;

/// <summary>
/// Определяет интерфейс для поставщика глобальных функций,
/// предоставляющих доступ к данным, таким как список всех героев.
/// </summary>
public interface IGameData
{
    /// <summary>
    /// Асинхронно загружает список всех героев из внешнего источника.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>Задача, представляющая операцию загрузки.</returns>
    Task LoadListAllHeroesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получает доступный список всех загруженных базовых сущностей героев.
    /// </summary>
    /// <returns>Коллекция, содержащая все доступные базовые сущности героев.</returns>
    IEnumerable<HeroBase> AllHeroes { get; }

    /// <summary>
    /// Находит и возвращает базовую сущность героя по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор (int) искомого героя.</param>
    /// <returns>Базовая сущность героя (<see cref="HeroBase"/>) с указанным <paramref name="id"/>,
    /// или <c>null</c> (если поддерживается типом), если герой не найден.</returns>
    HeroBase GetHeroById(int id);
}
