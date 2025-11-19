using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppForTests;

internal class NewRandom
{

    public sealed class UltraFastDiceRandom
    {
        private uint _state;

        /// <summary>
        /// Инициализация генератора.
        /// </summary>
        /// <param name="seed">Начальный сид (должен быть ненулевым).</param>
        public UltraFastDiceRandom(uint seed = 1)
        {
            if (seed == 0) seed = 1;
            _state = seed;
        }

        /// <summary>
        /// Возвращает сумму count случайных целых в диапазоне [1, size].
        /// Максимально оптимизировано для скорости.
        /// </summary>
        /// <param name="count">Количество бросков.</param>
        /// <param name="size">Верхняя граница.</param>
        /// <returns>Сумма.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetRandomSum(int count, int size)
        {
            uint s = _state;
            long sum = 0;

            uint usize = (uint)size;
            for (int i = 0; i < count; i++)
            {
                // xorshift32 — 3 простейших операции
                s ^= s << 13;
                s ^= s >> 17;
                s ^= s << 5;

                // масштабирование: равномерное 0..size-1
                // (uint)(((ulong)s * usize) >> 32) — самое быстрое и корректное преобразование
                sum += (uint)(((ulong)s * usize) >> 32) + 1;
            }

            _state = s;

            if (sum > int.MaxValue)
                throw new OverflowException();

            return (int)sum;
        }
    }

    public sealed class UltraFastDiceRandom2
    {
        private int _state;

        /// <summary>
        /// Инициализация генератора.
        /// </summary>
        /// <param name="seed">Начальный сид (должен быть ненулевым).</param>
        public UltraFastDiceRandom2(int seed = 1)
        {
            if (seed == 0) seed = 1;
            _state = seed;
        }

        /// <summary>
        /// Возвращает сумму count случайных целых в диапазоне [1, size].
        /// Максимально оптимизировано для скорости.
        /// </summary>
        /// <param name="count">Количество бросков.</param>
        /// <param name="size">Верхняя граница.</param>
        /// <returns>Сумма.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetRandomSum(int count, int size)
        {
            int s = _state;
            long sum = 0;

            int usize = size;
            for (int i = 0; i < count; i++)
            {
                // xorshift32 — 3 простейших операции
                s ^= s << 13;
                s ^= s >> 17;
                s ^= s << 5;

                // масштабирование: равномерное 0..size-1
                // (uint)(((ulong)s * usize) >> 32) — самое быстрое и корректное преобразование
                sum += (int)(((long)s * usize) >> 32) + 1;
            }

            _state = s;

            return (int)sum;
        }
    }



    private static readonly UltraFastDiceRandom rand1 = new((uint)DateTime.Now.Ticks);
    private static readonly UltraFastDiceRandom2 rand2 = new();
    private static readonly Random random = new();
    internal static int GetRandomInt(int count, int size)//13.7
    {
        int result = 0;
        for (int i = 0; i < count; i++)
        {
            result += random.Next(size) + 1;
        }
        return result;
    }
    internal static void Start() {
        for (int sim = 0; sim < 1000; sim++)
        {
            _ = GetRandomInt(10, 10);
        }
        int rand1Min = int.MaxValue, rand1Max = int.MinValue;

        int cube = 6;
        int rand1MinNeed = 10, rand1MaxNeed = rand1MinNeed * cube;
        int iter = 0;
        while (true)
        {
            int v = rand1.GetRandomSum(10, 6);
            if (rand1Min > v)
            {
                rand1Min = v;
            }
            if (rand1Max < v)
            {
                rand1Max = v;
            }
            iter++;
            if (rand1Min == rand1MinNeed && rand1Max == rand1MaxNeed)
            {
                break;
            }
        }
        Console.WriteLine(rand1Min.ToString() + " " + rand1Max.ToString());
        Console.WriteLine(iter.ToString());

        for (int sim = 0; sim < 1000; sim++)
        {
            //Console.Write(rand2.GetRandomSum(1, 5).ToString() + "; ");
        }


        Console.WriteLine($"Console log.");
        Console.WriteLine($"100000000 итераций для 3 вариантов.");

        DateTime start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = GetRandomInt(10, 10);
        }
        double var0 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"вариант GetRandomInt(10, 10) = {var0} секунд");




        start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = rand1.GetRandomSum(10, 10);
        }
        var var1 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"вариант GetRandomSum(10, 10) = {var1} секунд");
        //Console.WriteLine($"{(var0 / var1 - 1) * 100:0.00}%");




        start = DateTime.Now;
        for (int sim = 0; sim < 100000000; sim++)
        {
            _ = rand2.GetRandomSum(10, 10);
        }
        var var2 = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"последний вариант = {var2} секунд");//
        //Console.WriteLine($"{(var0 / var2 - 1) * 100:0.00}%");



        Console.ReadLine();
    }
}
