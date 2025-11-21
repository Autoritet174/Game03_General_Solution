using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace General;

/// <summary>
/// Статический класс-расширение, предоставляющий полезные методы для работы со строками (<see cref="string"/>).
/// </summary>
public static class StringExtension
{

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
        if (s is null)
        {
            throw new ArgumentNullException(nameof(s), "Строка не может быть null.");
        }

        return s.IsEmpty() ? s : $"{s[..1].ToUpperInvariant()}{s[1..]}";
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
}
