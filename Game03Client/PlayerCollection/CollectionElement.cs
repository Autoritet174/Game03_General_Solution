using System;
using System.Collections.Generic;
using System.Text;

namespace Game03Client.PlayerCollection;

public class CollectionElement(Guid id, int baseId, int rarity, string name, string type)
{
    public Guid Id { get; } = id;
    public int BaseId { get; } = baseId;
    public int Rarity { get; } = rarity;
    public string Name { get; } = name;
    public string Type { get; } = type;
}
