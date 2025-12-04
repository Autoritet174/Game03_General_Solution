using System.Text;
using System.Text.RegularExpressions;

namespace Server_Common;
public static class Extensions
{
    public static string ToSnakeCase(this string input)
    {
        //int iEnd = input.Length;
        //StringBuilder result = new(iEnd + 10);

        //char c = input[0];
        //if (char.IsUpper(c))
        //{
        //    _ = result.Append('_');
        //}
        //_ = result.Append(char.ToLower(c));

        //for (int i = 1; i < iEnd; i++)
        //{
        //    c = input[i];
        //    if (char.IsUpper(c))
        //    {
        //        _ = result.Append('_');
        //    }
        //    _ = result.Append(char.ToLower(c));
        //}

        //return result.ToString();
        // Преобразует CamelCase в snake_case
        return Regex.Replace(input,
            "(?<=[a-z])([A-Z])",
            "_$1",
            RegexOptions.Compiled).ToLower();
    }
}
