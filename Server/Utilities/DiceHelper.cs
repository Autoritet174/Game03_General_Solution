using General.DTO;

namespace Server.Utilities;

public class DiceHelper
{
    public static float GetRandomValue(Dice? dice)
    {
        if (dice == null)
        {
            return 0f;
        }

        int count = dice.Count;
        int sides = dice.Sides;
        if (count < 1 || sides < 1)
        {
            return 0f;
        }

        int sum = 0;
        Random rand = Random.Shared;
        for (int i = count - 1; i > -1; i--)
        {
            sum += rand.Next(sides) + 1;
        }

        return sum + (dice.Modificator ?? 0f);
    }
}
