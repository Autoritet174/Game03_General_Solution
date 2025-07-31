namespace Server.Common;
public static class Extensions
{
    public static string ToSnakeCase(this string input)
    {
        return string.Concat(input.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString())).ToLower();
    }
}
