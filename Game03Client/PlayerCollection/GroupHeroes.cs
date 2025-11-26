using General.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game03Client.PlayerCollection;

/// <summary>
/// Группа героев.
/// </summary>
public class GroupHeroes(string name, IEnumerable<CollectionHero> list)
{
    public string Name { get; } = name;

    public IEnumerable<CollectionHero> List { get; } = list;

    public int Priority { get; set; } = 0;
}
