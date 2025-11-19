using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace General;

/// <summary>
/// Класс-расширение для string.
/// </summary>
public static class StringExtension
{

    /// <summary>
    /// True если IsNullOrWhiteSpace.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsEmpty([NotNullWhen(false)] this string? s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    /// <summary>
    /// Если возможно, преобразуем первый символ в верхний регистр, остальные без изменений.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToUpper1Char(this string s)
    {
        return s.IsEmpty() ? s : $"{s[..1].ToUpper()}{s[1..]}";
    }

    /// <summary>
    /// Пытается создать объект MailAddress из строки. Возвращает значение результата.
    /// </summary>
    public static bool IsEmail(this string s) {
        if (s.IsEmpty())
        {
            return false;
        }

        try
        {
            _ = new MailAddress(s);
            return true;
        }
        //catch (FormatException)
        //{
        //    return false;
        //}
        catch// (Exception)
        {
            return false;
        }
    }
}
