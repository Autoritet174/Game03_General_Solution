using System.Text;

namespace Server_Common;
public static class Extensions
{
    public static string ToSnakeCase(this string input)
    {
        //return string.Concat(input.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString())).ToLower();
        int iEnd = input.Length;
        StringBuilder result = new(iEnd + 10);

        char c = input[0];
        if (char.IsUpper(c))
        {
            _ = result.Append('_');
        }
        _ = result.Append(char.ToLower(c));

        for (int i = 1; i < iEnd; i++)
        {
            c = input[i];
            if (char.IsUpper(c))
            {
                _ = result.Append('_');
            }
            _ = result.Append(char.ToLower(c));
        }

        return result.ToString();
    }
}
