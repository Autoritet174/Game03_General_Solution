using System.Drawing;
using System.Text;

namespace Server.Common;

public static class Console
{
    /// <summary>
    /// Окрашивает текст в указанный цвет и фон с помощью ANSI-кодов.
    /// </summary>
    /// <param name="text">Текст для вывода.</param>
    /// <param name="foregroundColor">Цвет текста (если null — цвет по умолчанию).</param>
    /// <param name="backgroundColor">Цвет фона (если null — фон по умолчанию).</param>
    /// <returns>Строка с ANSI-кодами для консоли.</returns>
    public static string ColorizeText(string text, Color? foregroundColor = null, Color? backgroundColor = null)
    {
        StringBuilder sb = new("\u001b[", 50); // Предварительный размер буфера

        // Добавляем цвет текста
        if (foregroundColor.HasValue)
        {
            Color fg = foregroundColor.Value;
            _ = sb.Append($"38;2;{fg.R};{fg.G};{fg.B};");
        }

        // Добавляем цвет фона
        if (backgroundColor.HasValue)
        {
            Color bg = backgroundColor.Value;
            _ = sb.Append($"48;2;{bg.R};{bg.G};{bg.B};");
        }

        // Удаляем последнюю ';' если есть
        if (sb[^1] == ';')
        {
            sb.Length--; // Эффективнее, чем sb.Remove(sb.Length - 1, 1)
        }

        // Добавляем завершающую часть
        _ = sb.Append('m').Append(text).Append("\u001b[0m");

        return sb.ToString();
    }
}
