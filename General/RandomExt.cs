using System;

namespace General;

public static class RandomExt
{
    public static int NextInclusive(this Random r, int min, int max)
    {
        return r.Next(min, max + 1);
    }
}
