using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace General;

/// <summary>
/// Статический класс-расширение, предоставляющий полезные методы для работы со строками (<see cref="string"/>).
/// </summary>
public static partial class StringExt
{
    /// <summary>
    /// Trim and ToUpperInvariant
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string NormalizedValueGame03(this string s) {
        return s.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Заменить все символы '\' на '/'
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToDirectSlash(this string? s) {
        return s.IsEmpty() ? string.Empty : s.Replace("\\", "/");
    }

    /// <summary>
    /// Проверяет, является ли строка <see langword="null"/>, пустой (<see cref="string.Empty"/>) или содержит только пробельные символы.
    /// Является оберткой для <see cref="string.IsNullOrWhiteSpace(string?)"/>.
    /// </summary>
    /// <param name="s">Строка для проверки.</param>
    /// <returns>
    /// <see langword="true"/>, если строка является <see langword="null"/>, пустой или состоит только из пробельных символов;
    /// в противном случае — <see langword="false"/>.
    /// </returns>
    public static bool IsEmpty([NotNullWhen(false)] this string? s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    /// <summary>
    /// Преобразует первый символ строки в верхний регистр, оставляя остальные символы без изменений.
    /// Если строка <see langword="null"/> или пуста, возвращается исходная строка.
    /// </summary>
    /// <param name="s">Исходная строка.</param>
    /// <returns>
    /// Строка с первым символом в верхнем регистре.
    /// </returns>
    public static string ToUpper1Char(this string s)
    {
        // Проверка входных данных: string s должен быть не null, иначе IsEmpty() вызовет исключение при доступе к методу.
        // Однако в контексте методов расширения string s не будет null, если он вызывается через s.ToUpper1Char().
        // Проверка IsEmpty() обрабатывает случай, когда строка пустая или состоит из пробелов.
        return s is null
            ? throw new ArgumentNullException(nameof(s), "Строка не может быть null.")
            : s.IsEmpty() ? s : $"{s[..1].ToUpperInvariant()}{s[1..]}";
    }

    /// <summary>
    /// Проверяет, является ли строка корректным адресом электронной почты,
    /// пытаясь создать объект <see cref="MailAddress"/>.
    /// </summary>
    /// <param name="s">Строка, содержащая адрес электронной почты для проверки.</param>
    /// <returns>
    /// <see langword="true"/>, если строка является действительным адресом электронной почты;
    /// <see langword="false"/> в противном случае.
    /// </returns>
    public static bool IsEmail(this string s)
    {
        if (s is null || s.IsEmpty())
        {
            return false;
        }

        try
        {
            // Попытка создания объекта MailAddress. Если формат неверный, будет выброшено исключение.
            _ = new MailAddress(s);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Регулярное выражение для преобразования строки в snake_case.
    /// Ищет заглавные буквы, которые следуют после строчных букв.
    /// </summary>
    private static readonly Regex SnakeCaseRegex = new("(?<=[a-z])([A-Z])", RegexOptions.Compiled);

    /// <summary>
    /// Регулярное выражение для разделения строк на слова при преобразовании в PascalCase/camelCase.
    /// Сохраняет символы подчеркивания как часть итогового имени.
    /// </summary>
    private static readonly Regex WordSplitWithUnderscoresRegex = new(
        @"(?:(?<=[a-z])(?=[A-Z]))|(?:(?<=[A-Z])(?=[A-Z][a-z]))|([_\-\s]+)",
        RegexOptions.Compiled);

    /// <summary>
    /// Регулярное выражение для разделения строк на слова при полном разбиении.
    /// </summary>
    private static readonly Regex WordSplitRegex = new(
        @"[_\-\s]+|(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])",
        RegexOptions.Compiled);

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
    public static string ToSnakeCase(this string? input)
    {
        return string.IsNullOrWhiteSpace(input) ? string.Empty : SnakeCaseRegex.Replace(input, "_$1").ToLowerInvariant();
    }


    /// <summary>
    /// Преобразует строку в формат PascalCase (UpperCamelCase).
    /// </summary>
    /// <param name="input">Входная строка для преобразования.</param>
    /// <param name="keepOriginalAcronyms">Если true, сохраняет оригинальный регистр акронимов (например, "XML" останется "XML").</param>
    /// <param name="preserveUnderscores">Если true, сохраняет символы подчеркивания в результате (по умолчанию true).</param>
    /// <returns>
    /// Строка в формате PascalCase, где каждое слово начинается с заглавной буквы.
    /// Если preserveUnderscores = true, символы подчеркивания сохраняются.
    /// Возвращает пустую строку, если входная строка является null или пустой.
    /// </returns>
    /// <example>
    /// <code>
    /// // С сохранением подчеркиваний (по умолчанию)
    /// string result1 = "X_EquipmentType_DamageType_EquipmentTypeId_idx".ToPascalCase(); 
    /// // "X_EquipmentType_DamageType_EquipmentTypeId_idx"
    /// 
    /// // Без сохранения подчеркиваний
    /// string result2 = "X_EquipmentType_DamageType_EquipmentTypeId_idx".ToPascalCase(preserveUnderscores: false); 
    /// // "XEquipmentTypeDamageTypeEquipmentTypeIdIdx"
    /// 
    /// string result3 = "user_name".ToPascalCase(); // "User_Name"
    /// string result4 = "user_name".ToPascalCase(preserveUnderscores: false); // "UserName"
    /// </code>
    /// </example>
    public static string ToPascalCase(this string? input, bool preserveUnderscores = false, bool keepOriginalAcronyms = false)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Если нужно сохранить подчеркивания
        if (preserveUnderscores)
        {
            var result = new StringBuilder();
            string[] wordsAndSeparators = WordSplitWithUnderscoresRegex.Split(input);

            for (int i = 0; i < wordsAndSeparators.Length; i++)
            {
                string part = wordsAndSeparators[i];
                if (string.IsNullOrEmpty(part))
                {
                    continue;
                }

                // Проверяем, является ли часть разделителем
                if (part.Length == 1 && (part[0] == '_' || part[0] == '-' || char.IsWhiteSpace(part[0])))
                {
                    // Сохраняем только подчеркивания
                    if (part[0] == '_')
                    {
                        _ = result.Append('_');
                    }
                    // Дефисы и пробелы заменяем на подчеркивания
                    else if (part[0] == '-' || char.IsWhiteSpace(part[0]))
                    {
                        _ = result.Append('_');
                    }
                    continue;
                }

                // Обработка слова
                if (keepOriginalAcronyms && IsAllUpper(part))
                {
                    _ = result.Append(part);
                }
                else
                {
                    if (part.Length > 0)
                    {
                        _ = result.Append(char.ToUpperInvariant(part[0]));
                        if (part.Length > 1)
                        {
                            _ = result.Append(part[1..].ToLowerInvariant());
                        }
                    }
                }
            }

            return result.ToString();
        }
        else
        {
            // Старая логика без сохранения подчеркиваний
            string[] words = WordSplitRegex.Split(input);
            var result = new StringBuilder();

            foreach (string? word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }

                if (keepOriginalAcronyms && IsAllUpper(word))
                {
                    _ = result.Append(word);
                }
                else
                {
                    _ = result.Append(char.ToUpperInvariant(word[0]));
                    if (word.Length > 1)
                    {
                        _ = result.Append(word[1..].ToLowerInvariant());
                    }
                }
            }

            return result.ToString();
        }
    }

    /// <summary>
    /// Преобразует строку в формат camelCase (lowerCamelCase).
    /// </summary>
    /// <param name="input">Входная строка для преобразования.</param>
    /// <param name="keepOriginalAcronyms">Если true, сохраняет оригинальный регистр акронимов (например, "XML" останется "XML").</param>
    /// <param name="preserveUnderscores">Если true, сохраняет символы подчеркивания в результате (по умолчанию true).</param>
    /// <returns>
    /// Строка в формате camelCase.
    /// </returns>
    public static string ToCamelCase(this string? input, bool keepOriginalAcronyms = false, bool preserveUnderscores = true)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Если нужно сохранить подчеркивания
        if (preserveUnderscores)
        {
            var result = new StringBuilder();
            string[] wordsAndSeparators = WordSplitWithUnderscoresRegex.Split(input);
            bool isFirstWord = true;

            for (int i = 0; i < wordsAndSeparators.Length; i++)
            {
                string part = wordsAndSeparators[i];
                if (string.IsNullOrEmpty(part))
                {
                    continue;
                }

                // Проверяем, является ли часть разделителем
                if (part.Length == 1 && (part[0] == '_' || part[0] == '-' || char.IsWhiteSpace(part[0])))
                {
                    // Сохраняем только подчеркивания
                    if (part[0] == '_')
                    {
                        _ = result.Append('_');
                    }
                    // Дефисы и пробелы заменяем на подчеркивания
                    else if (part[0] == '-' || char.IsWhiteSpace(part[0]))
                    {
                        _ = result.Append('_');
                    }
                    continue;
                }

                // Обработка слова
                if (isFirstWord)
                {
                    _ = result.Append(part.ToLowerInvariant());
                    isFirstWord = false;
                }
                else if (keepOriginalAcronyms && IsAllUpper(part))
                {
                    _ = result.Append(part);
                }
                else
                {
                    if (part.Length > 0)
                    {
                        _ = result.Append(char.ToUpperInvariant(part[0]));
                        if (part.Length > 1)
                        {
                            _ = result.Append(part[1..].ToLowerInvariant());
                        }
                    }
                }
            }

            return result.ToString();
        }
        else
        {
            // Старая логика без сохранения подчеркиваний
            string[] words = WordSplitRegex.Split(input);
            var result = new StringBuilder();
            bool isFirstWord = true;

            foreach (string? word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }

                if (isFirstWord)
                {
                    _ = result.Append(word.ToLowerInvariant());
                    isFirstWord = false;
                }
                else if (keepOriginalAcronyms && IsAllUpper(word))
                {
                    _ = result.Append(word);
                }
                else
                {
                    _ = result.Append(char.ToUpperInvariant(word[0]));
                    if (word.Length > 1)
                    {
                        _ = result.Append(word[1..].ToLowerInvariant());
                    }
                }
            }

            return result.ToString();
        }
    }

    /// <summary>
    /// Преобразует строку в формат kebab-case (spinal-case).
    /// </summary>
    /// <param name="input">Входная строка для преобразования.</param>
    /// <param name="screaming">Если true, возвращает SCREAMING-KEBAB-CASE.</param>
    /// <returns>
    /// Строка в формате kebab-case, где слова разделены дефисами.
    /// Возвращает пустую строку, если входная строка является null или пустой.
    /// </returns>
    /// <example>
    /// <code>
    /// string result = "user_name".ToKebabCase(); // "user-name"
    /// string result2 = "HelloWorld".ToKebabCase(); // "hello-world"
    /// string result3 = "XML Parser".ToKebabCase(); // "xml-parser"
    /// string result4 = "user name".ToKebabCase(true); // "USER-NAME"
    /// </code>
    /// </example>
    public static string ToKebabCase(this string? input, bool screaming = false)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        string[] words = WordSplitRegex.Split(input);
        var result = new StringBuilder();

        foreach (string? word in words)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            if (result.Length > 0)
            {
                _ = result.Append('-');
            }

            if (screaming)
            {
                _ = result.Append(word.ToUpperInvariant());
            }
            else
            {
                _ = result.Append(word.ToLowerInvariant());
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Преобразует строку в формат SCREAMING_SNAKE_CASE.
    /// </summary>
    /// <param name="input">Входная строка для преобразования.</param>
    /// <returns>
    /// Строка в формате SCREAMING_SNAKE_CASE.
    /// Возвращает пустую строку, если входная строка является null или пустой.
    /// </returns>
    /// <example>
    /// <code>
    /// string result = "user_name".ToScreamingSnakeCase(); // "USER_NAME"
    /// string result2 = "HelloWorld".ToScreamingSnakeCase(); // "HELLO_WORLD"
    /// string result3 = "xml-parser".ToScreamingSnakeCase(); // "XML_PARSER"
    /// </code>
    /// </example>
    public static string ToScreamingSnakeCase(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        string[] words = WordSplitRegex.Split(input);
        var result = new StringBuilder();

        foreach (string? word in words)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            if (result.Length > 0)
            {
                _ = result.Append('_');
            }

            _ = result.Append(word.ToUpperInvariant());
        }

        return result.ToString();
    }

    /// <summary>
    /// Альтернативная версия ToPascalCase без сохранения подчеркиваний (старое поведение).
    /// </summary>
    private static string ToPascalCaseWithoutUnderscores(string input, bool keepOriginalAcronyms)
    {
        string[] words = WordSplitRegex.Split(input);
        var result = new StringBuilder();

        foreach (string? word in words)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            if (keepOriginalAcronyms && IsAllUpper(word))
            {
                _ = result.Append(word);
            }
            else
            {
                _ = result.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1)
                {
                    _ = result.Append(word[1..].ToLowerInvariant());
                }
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Альтернативная версия ToCamelCase без сохранения подчеркиваний (старое поведение).
    /// </summary>
    private static string ToCamelCaseWithoutUnderscores(string input, bool keepOriginalAcronyms)
    {
        string[] words = WordSplitRegex.Split(input);
        var result = new StringBuilder();
        bool isFirstWord = true;

        foreach (string? word in words)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                continue;
            }

            if (isFirstWord)
            {
                _ = result.Append(word.ToLowerInvariant());
                isFirstWord = false;
            }
            else if (keepOriginalAcronyms && IsAllUpper(word))
            {
                _ = result.Append(word);
            }
            else
            {
                _ = result.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1)
                {
                    _ = result.Append(word[1..].ToLowerInvariant());
                }
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Проверяет, является ли строка разделителем.
    /// </summary>
    private static bool IsSeparator(string part)
    {
        if (part.Length == 1)
        {
            char c = part[0];
            return c == '_' || c == '-' || char.IsWhiteSpace(c);
        }

        // Для длинных последовательностей разделителей
        foreach (char c in part)
        {
            if (!char.IsWhiteSpace(c) && c != '_' && c != '-')
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Проверяет, состоит ли строка только из заглавных букв.
    /// </summary>
    private static bool IsAllUpper(string input)
    {
        foreach (char c in input)
        {
            if (char.IsLetter(c) && !char.IsUpper(c))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Определяет стиль наименования строки.
    /// </summary>
    /// <param name="input">Входная строка для анализа.</param>
    /// <returns>
    /// <see cref="NamingStyle"/> - стиль наименования строки.
    /// </returns>
    public static NamingStyle DetectNamingStyle(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return NamingStyle.Unknown;
        }

        if (input.Contains('_'))
        {
            return input == input.ToUpperInvariant() ? NamingStyle.ScreamingSnakeCase : NamingStyle.SnakeCase;
        }

        if (input.Contains('-'))
        {
            return input == input.ToUpperInvariant() ? NamingStyle.ScreamingKebabCase : NamingStyle.KebabCase;
        }

        if (char.IsUpper(input[0]))
        {
            // Проверяем, есть ли заглавные буквы в середине слова
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    return NamingStyle.PascalCase;
                }
            }
            return NamingStyle.SingleWord; // Одно слово с заглавной буквы
        }

        // camelCase
        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                return NamingStyle.CamelCase;
            }
        }

        return char.IsLower(input[0]) ? NamingStyle.SingleWord : NamingStyle.Unknown;
    }

    /// <summary>
    /// Конвертирует строку из одного стиля наименования в другой.
    /// </summary>
    /// <param name="input">Входная строка.</param>
    /// <param name="targetStyle">Целевой стиль наименования.</param>
    /// <returns>Строка в целевом стиле наименования.</returns>
    public static string ConvertNamingStyle(this string? input, NamingStyle targetStyle)
    {
        return targetStyle switch
        {
            NamingStyle.SnakeCase => input.ToSnakeCase(),
            NamingStyle.PascalCase => input.ToPascalCase(),
            NamingStyle.CamelCase => input.ToCamelCase(),
            NamingStyle.KebabCase => input.ToKebabCase(),
            NamingStyle.ScreamingSnakeCase => input.ToScreamingSnakeCase(),
            NamingStyle.ScreamingKebabCase => input.ToKebabCase(true),
            _ => input ?? string.Empty
        };
    }

    /// <summary>
    /// Стили наименования.
    /// </summary>
    public enum NamingStyle
    {
        /// <summary>
        /// Неизвестный стиль.
        /// </summary>
        Unknown,

        /// <summary>
        /// snake_case
        /// </summary>
        SnakeCase,

        /// <summary>
        /// PascalCase (UpperCamelCase)
        /// </summary>
        PascalCase,

        /// <summary>
        /// camelCase (lowerCamelCase)
        /// </summary>
        CamelCase,

        /// <summary>
        /// kebab-case (spinal-case)
        /// </summary>
        KebabCase,

        /// <summary>
        /// SCREAMING_SNAKE_CASE
        /// </summary>
        ScreamingSnakeCase,

        /// <summary>
        /// SCREAMING-KEBAB-CASE
        /// </summary>
        ScreamingKebabCase,

        /// <summary>
        /// Одно слово без разделителей.
        /// </summary>
        SingleWord
    }
}
