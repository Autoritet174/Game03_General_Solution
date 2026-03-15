using FluentResults;
using General.DTO;

namespace Server.Utilities;

public class DiceHelper
{
    public static long GetRandomValue(Dice? dice)
    {
        if (dice == null)
        {
            return 0L;
        }

        int count = dice.Count;
        int sides = dice.Sides;
        if (count < 1 || sides < 1)
        {
            return 0L;
        }

        int sum = 0;
        Random rand = Random.Shared;
        for (int i = count - 1; i > -1; i--)
        {
            sum += rand.Next(sides) + 1;
        }

        return (sum * 1000L) + (dice.Modificator_1000 ?? 0L);
    }
}
