using System;
using System.Collections.Generic;
using System.Text;

namespace General.DTO.Entities.GameData;

public class DtoNpc(int Id, string Name, int Rarity, ERank Rank, EMainStat MainStat, Guid SpawnId)
{
    public int Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public int Rarity { get; set; } = Rarity;
    public ERank Rank { get; set; } = Rank;
    public EMainStat MainStat { get; set; } = MainStat;
    public Guid SpawnId { get; set; } = SpawnId;
}
