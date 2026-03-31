using General.DTO;

namespace Server.Utilities;

public static class DiceHelper
{
    public static float GetRandom(this Dice? dice)
    {
        if (dice == null)
        {
            return 0f;
        }

        int count = dice.Count;
        int sides = dice.Sides;
        if (count < 1 || sides < 1)
        {
            return (dice.Modificator ?? 0f);
        }

        if (sides == 1)
        {
            return count + (dice.Modificator ?? 0f);
        }

        Random rand = Random.Shared;
        int sum = count;
        for (int i = 0; i < count; i++)
        {
            sum += rand.Next(sides);
        }

        return sum + (dice.Modificator ?? 0f);
    }
}
