using General.DTO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace General;

/// <summary>
/// Глобальный статический класс, содержащий константы и вспомогательные функции,
/// которые должны быть легко доступны из любого места приложения.
/// </summary>
public static class GlobalHelper
{

    /// <summary>
    /// Константа для типа медиа "application/json", часто используемого в HTTP-запросах.
    /// </summary>
    public const string APPLICATION_JSON = "application/json";

    /// <summary>
    /// Преобразует общее количество секунд в удобочитаемую строку,
    /// форматируя их в дни, часы, минуты и секунды (d hh mm s).
    /// </summary>
    /// <param name="sec">Общее количество секунд (<see cref="long"/>), которое необходимо преобразовать.</param>
    /// <returns>
    /// Строка, представляющая время в формате "dd hh mm s".
    /// Пример: "05m 10s", "01h 05m 10s", "1d 01h 05m 10s".
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если <paramref name="sec"/> меньше нуля.</exception>
    public static string SecondsToTimeStr(long sec)
    {
        string sign;
        if (sec < 0)
        {
            sign = "-";
            sec = -sec;
        }
        else
        {
            sign = string.Empty;
        }
        long min = sec / 60L;
        sec -= min * 60L;

        long hours = min / 60L;
        min -= hours * 60L;

        long days = hours / 24L;
        hours -= days * 24L;

        string result = $"{min:00}m {sec:00}s";
        if (days > 0)
        {
            result = $"{days}d {hours:00}h {result}";
        }
        else if (hours > 0)
        {
            result = $"{hours:00}h {result}";
        }

        return $"{sign}{result}";
    }


}
