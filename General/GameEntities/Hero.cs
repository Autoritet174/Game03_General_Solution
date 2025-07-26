using System;
using System.Collections.Generic;
using System.Text;

namespace General.GameEntities
{
    public class HeroBaseEntity(string name)
    {
        public string Name { get; } = name;
    }
}
