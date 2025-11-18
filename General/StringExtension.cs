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
    /// Если возможно, преобразуем первый символ в верхний регистр, остальные без изменений.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToUpper1Char(this string s)
    {
        return string.IsNullOrWhiteSpace(s) || s.Length == 0 ? s : $"{s[..1].ToUpper()}{s[1..]}";
    }

    extension([NotNullWhen(false)] string? s) {
        /// <summary>
        /// True если IsNullOrWhiteSpace
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(s);
    }

    extension(string s)
    {

        /// <summary>
        /// Пытается создать объект MailAddress из строки. Возвращает значение результата.
        /// </summary>
        public bool IsEmail
        {
            get
            {
                if (s.IsEmpty)
                {
                    return false;
                }

                try
                {
                    _ = new MailAddress(s);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

    public static bool StringIsEmail(this string s) {
        return s.IsEmail;
    }
}
