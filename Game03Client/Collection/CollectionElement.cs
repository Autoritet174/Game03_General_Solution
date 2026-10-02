using System;

namespace Game03Client.Collection;

public enum TypeCollectionElement { Hero, Equipment }
public class CollectionElement
{
    public Guid id { get; init; }
    public int baseId { get; init; }
    public int rarity { get; init; }
    public required string name { get; init; }
    public bool isUnique { get; set; }
    public TypeCollectionElement typeCollectionElement { get; set; }
}
