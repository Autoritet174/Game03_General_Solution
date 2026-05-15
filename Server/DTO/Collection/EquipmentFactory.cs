
using General.DTO;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;

namespace Server.DTO.Collection;

public static class EquipmentFactory
{
    private static readonly Dictionary<int, int> countStatsByRarity = new()
    {
        [1] = 7,
        [2] = 1,
        [3] = 2,
        [4] = 3,
        [5] = 5,
        [6] = 7,
    };

    public static Equipment CreateFromBaseEquipment(BaseEquipment baseEquipment, Guid userId)
    {
        Equipment e = new()
        {
            BaseEquipmentId = baseEquipment.Id,
            UserId = userId
        };

        // Сгенерировать статы
        Dictionary<EStatType, Dice> pos = baseEquipment.PossibleStats ?? [];
        if (baseEquipment.EquipmentType.PossibleStats != null)
        {
            foreach (KeyValuePair<EStatType, Dice> item in baseEquipment.EquipmentType.PossibleStats)
            {
                if (!pos.ContainsKey(item.Key))
                {
                    pos.Add(item.Key, item.Value);
                }
            }
        }

        int countPossibleStats = pos.Count;
        if (countPossibleStats > 0)
        {
            e.Stats = [];
            for (int i = countStatsByRarity[baseEquipment.Rarity]; i > 0; i--)
            {
                EStatType randomKey = pos.Keys.ElementAt(Random.Shared.Next(countPossibleStats));
                List<float> list;
                if (e.Stats.TryGetValue(randomKey, out List<float>? value))
                {
                    list = value;
                }
                else
                {
                    list = [];
                    e.Stats.Add(randomKey, list);
                }

                Dice dice = pos[randomKey];
                float statValue = dice.GetRandomValue();
                list.Add(statValue);
            }

        }
        return e;
    }

}
