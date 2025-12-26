using System;
using System.Collections.Generic;
using System.IO;

namespace General;

public static class PathGame03
{
    public static string Combine(IEnumerable<string> paths)
    {
        return Path.Combine(System.Linq.Enumerable.ToArray(paths)).Replace('\\', '/');
    }
    public static string Combine(string path1, string path2)
    {
        return Path.Combine(path1, path2).Replace('\\', '/');
    }
    public static string Combine(string path1, string path2, string path3)
    {
        return Path.Combine(path1, path2, path3).Replace('\\', '/');
    }
    public static string Combine(string path1, string path2, string path3, string path4)
    {
        return Path.Combine(path1, path2, path3, path4).Replace('\\', '/');
    }
}
