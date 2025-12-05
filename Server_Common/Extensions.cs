using System.Text.RegularExpressions;

namespace Server_Common;

/// <summary>
/// Класс, предоставляющий расширяющие методы для работы со строками и другими типами.
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// Регулярное выражение для преобразования строки в snake_case.
    /// Ищет заглавные буквы, которые следуют после строчных букв.
    /// </summary>
    [GeneratedRegex("(?<=[a-z])([A-Z])", RegexOptions.Compiled)]
    private static partial Regex SnakeCaseRegex();

    /// <summary>
    /// Преобразует строку в формат snake_case.
    /// </summary>
    /// <param name="input">Входная строка для преобразования.</param>
    /// <returns>
    /// Строка в формате snake_case, где слова разделены символами подчеркивания,
    /// а все буквы приведены к нижнему регистру.
    /// Возвращает исходную строку, если она является null или пустой.
    /// </returns>
    /// <example>
    /// <code>
    /// string result = "CamelCaseString".ToSnakeCase(); // "camel_case_string"
    /// string result2 = "UserName".ToSnakeCase(); // "user_name"
    /// string result3 = "XMLParser".ToSnakeCase(); // "xmlparser"
    /// </code>
    /// </example>
    /// <remarks>
    /// Метод преобразует CamelCase строки в snake_case, вставляя символ подчеркивания
    /// перед каждой заглавной буквой, которая следует за строчной буквой,
    /// а затем приводит всю строку к нижнему регистру.
    /// </remarks>
    public static string ToSnakeCase(this string input)
    {
        return string.IsNullOrEmpty(input) ? input : SnakeCaseRegex().Replace(input, "_$1").ToLowerInvariant();
    }
}
