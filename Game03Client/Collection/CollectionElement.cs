using System;

namespace Game03Client.Collection;

public enum TypeCollectionElement { Hero, Equipment}
public class CollectionElement(Guid id, int baseId, int rarity, string name, bool isUnique, TypeCollectionElement typeCollectionElement)
{
    public Guid Id { get; } = id;
    public int BaseId { get; } = baseId;
    public int Rarity { get; } = rarity;
    public string Name { get; } = name;
    public bool IsUnique { get; set; } = isUnique;
    public TypeCollectionElement TypeCollectionElement { get; set; } = typeCollectionElement;
}
