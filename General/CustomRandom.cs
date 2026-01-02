using System;
using System.Runtime.CompilerServices;

namespace General;

/// <summary>
/// Сверхвысокопроизводительный генератор псевдослучайных чисел.
/// Оптимизирован для обхода по скорости System.Random в плотных циклах.
/// </summary>
public sealed class CustomRandom
{
    private int _state;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CustomRandom"/>.
    /// </summary>
    /// <param name="seed">Начальное зерно.</param>
    public CustomRandom(int seed = 0)
    {
        _state = seed == 0 ? Environment.TickCount : seed;
        if (_state == 0)
        {
            _state = 0xACE1;
        }
    }

    /// <summary>
    /// Генерирует случайное число в диапазоне [min, max] включительно.
    /// </summary>
    /// <remarks>
    /// Внимание: проверка min > max отсутствует для достижения максимальной скорости.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int NextInclusive(int min, int max)
    {
        // 1. Быстрое обновление состояния (LCG: x = a*x + c)
        // Параметры выбраны для обеспечения периода 2^32
        _state = (_state * 1664525) + 1013904223;

        // 2. Расчет диапазона (включая обе границы)
        // 3. Fast Range Reduction (Lemire)
        // (x * range) >> 32 заменяет медленную операцию x % range
        return ((_state * (max - min + 1)) >> 32) + min;
    }
}
