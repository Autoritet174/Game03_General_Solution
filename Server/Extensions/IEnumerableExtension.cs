namespace Server.Extensions;

public static class IEnumerableExtension
{
    public static T? GetRandomElement<T>(this IEnumerable<T> source)
    {
        if (source is IList<T> list)
        {
            return list.Count < 1 ? default : list[Random.Shared.Next(list.Count)];
        }

        T[] array = [.. source];
        return array.Length < 1 ? default : array[Random.Shared.Next(array.Length)];
    }
}
