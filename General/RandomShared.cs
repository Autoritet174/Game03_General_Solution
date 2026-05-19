using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;

namespace General;

/// <summary>
/// Высокопроизводительный потокобезопасный аналог "Random.Shared" для .NET Standard 2.1.
/// </summary>
public static class RandomShared
{
    // RandomNumberGenerator.Fill даёт криптографически стойкий сид —
    // никакой корреляции между потоками даже при одновременном старте,
    // без lock и без счётчика.
    private static readonly ThreadLocal<Random> _threadLocal = new(CreateRandom);

    private static Random CreateRandom()
    {
        Span<byte> buf = stackalloc byte[4];
        RandomNumberGenerator.Fill(buf);
        return new Random(BitConverter.ToInt32(buf));
    }

    private static Random Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _threadLocal.Value!;
    }

    // === int ===

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int Next() => Current.Next();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int Next(int maxValue) => Current.Next(maxValue);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int Next(int minValue, int maxValue) => Current.Next(minValue, maxValue);

    // === double / float ===

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static double NextDouble() => Current.NextDouble();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float NextSingle() => (float)Current.NextDouble();

    public static double NextDouble(double minValue, double maxValue)
    {
        return minValue > maxValue
            ? throw new ArgumentOutOfRangeException(nameof(minValue))
            : minValue + (Current.NextDouble() * (maxValue - minValue));
    }

    public static float NextSingle(float minValue, float maxValue)
    {
        return minValue > maxValue
            ? throw new ArgumentOutOfRangeException(nameof(minValue))
            : minValue + ((float)Current.NextDouble() * (maxValue - minValue));
    }

    // === bytes ===

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void NextBytes(byte[] buffer) => Current.NextBytes(buffer);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void NextBytes(Span<byte> buffer) => Current.NextBytes(buffer);

    // === bool ===

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool NextBool() => Current.Next(2) == 0;

    // === long ===

    // Сбрасываем знаковый бит — гарантируем неотрицательность.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long NextInt64() => (long)(NextUInt64() >> 1);

    public static long NextInt64(long maxValue)
    {
        // Поведение идентично Random.Shared: отрицательный maxValue — исключение.
        return maxValue < 0 ? throw new ArgumentOutOfRangeException(nameof(maxValue)) : maxValue == 0 ? 0 : NextInt64(0, maxValue);
    }

    public static long NextInt64(long minValue, long maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue));
        }

        if (minValue == maxValue)
        {
            return minValue;
        }

        ulong range = (ulong)(maxValue - minValue);

        // Rejection sampling — стандартный алгоритм из исходников .NET runtime.
        // Отбрасываем значения из "хвоста", который не делится на range нацело,
        // чтобы получить равномерное распределение без modulo bias.
        ulong limit = ulong.MaxValue - (ulong.MaxValue % range);
        ulong result;
        do { result = NextUInt64(); } while (result > limit);

        return (long)(result % range) + minValue;
    }

    // === Shuffle ===

    public static void Shuffle<T>(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        Shuffle(array.AsSpan());
    }

    public static void Shuffle<T>(Span<T> span)
    {
        Random rng = Current;
        for (int i = span.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (span[j], span[i]) = (span[i], span[j]);
        }
    }

    // === internal ===

    // Random.Next() возвращает [0, int.MaxValue] — 31 бит, старший всегда 0.
    // Три вызова: 31 + 31 + 2 = 64 бита с равномерным распределением.
    private static ulong NextUInt64()
    {
        Random rng = Current;
        ulong a = (uint)rng.Next(); // биты  0–30
        ulong b = (uint)rng.Next(); // биты 31–61
        ulong c = (uint)rng.Next() & 0b11uL; // биты 62–63
        return a | (b << 31) | (c << 62);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInclusive(int maxValue) => Current.Next(maxValue + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInclusive(int minValue, int maxValue) => Current.Next(minValue, maxValue + 1);
}
