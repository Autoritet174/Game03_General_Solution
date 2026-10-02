using System.Collections.Concurrent;

namespace DiceCombinationFinder;

public class DiceCombinationFinder
{
    public class DiceCombination
    {
        public required string notation { get; set; }
        public double mean { get; set; }
        public double cV { get; set; }
        public double meanError { get; set; }
        public double cVError { get; set; }
        public double totalError { get; set; }
        public required string jsonb { get; set; }
        public int count { get; set; }
        public int sides { get; set; }
        public int mod { get; set; }
    }

    public static List<DiceCombination> FindDiceCombination(
        double targetMean,
        int modificator = 0,
        double targetCVPercent = 5,
        int maxDice = 100,
        int maxSides = -1,
        bool exactly = true,
        int maxDegreeOfParallelism = -1)
    {
        if (maxSides == -1)
        {
            maxSides = (int)((targetMean * 2) - 1);
        }

        var results = new ConcurrentBag<DiceCombination>();

        // Настройка параллелизма
        var parallelOptions = new ParallelOptions();
        if (maxDegreeOfParallelism > 0)
        {
            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        // Параллельный перебор количества кубиков
        _ = Parallel.For(1, maxDice + 1, parallelOptions, numDice =>
        {
            for (int sides = 2; sides <= maxSides; sides++)
            {
                ProcessCombination(numDice, sides, modificator, targetMean, targetCVPercent, exactly, results);
            }
        });

        // Сортируем по суммарной ошибке и возвращаем 10 лучших
        return [.. results.OrderBy(r => r.totalError).Take(10)];
    }

    private static void ProcessCombination(
        int numDice,
        int sides,
        int modificator,
        double targetMean,
        double targetCVPercent,
        bool exactly,
        ConcurrentBag<DiceCombination> results)
    {
        // Базовое среднее без модификатора: μ = n * (s+1)/2
        double baseMean = numDice * (sides + 1) / 2.0;

        // Итоговое среднее с модификатором
        double actualMean = baseMean + modificator;

        // Стандартное отклонение (не зависит от модификатора)
        // σ = sqrt(n * (s² - 1) / 12)
        double variance = numDice * (Math.Pow(sides, 2) - 1) / 12.0;
        double actualStd = Math.Sqrt(variance);
        double actualCV = actualMean != 0 ? (actualStd / actualMean * 100.0) : 0;

        // Вычисляем ошибки
        double meanError = Math.Abs(actualMean - targetMean) / targetMean * 100.0;
        double cvError = Math.Abs(actualCV - targetCVPercent);

        // Общая ошибка (взвешенная)
        double totalError = (0.7 * meanError) + (0.3 * cvError);

        // Формируем нотацию
        string notation;
        if (modificator == 0)
        {
            notation = $"{numDice}d{sides}";
        }
        else
        {
            notation = $"{numDice}d{sides}";
            if (modificator != 0)
            {
                notation += $"+{modificator}";
            }
        }

        if (!exactly || Math.Abs(actualMean - targetMean) < 0.0001)
        {
            string jsonb = $"{{\"c\":{numDice},\"s\":{sides}";
            if (modificator != 0)
            {
                jsonb += $",\"m\":{modificator}";
            }
            jsonb += "}";
            results.Add(new DiceCombination
            {
                notation = notation,
                mean = actualMean,
                cV = actualCV,
                meanError = meanError,
                cVError = cvError,
                totalError = totalError,
                jsonb = jsonb,
                count = numDice,
                sides = sides,
                mod = modificator
            });
        }
    }
}
