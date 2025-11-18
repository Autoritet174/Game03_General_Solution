using System;
using System.Net.Mail;

namespace General;

/// <summary>
/// Глобальные статические поля, константы и функции.
/// </summary>
public static class G
{

    

    /// <summary>
    /// Значение для mediaType StringContent.
    /// </summary>
    public const string APPLICATION_JSON = "application/json";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    public static string SecondsToTimeStr(long sec)
    {
        long min = sec / 60L;
        sec -= min * 60L;

        long hours = min / 60L;
        min -= hours * 60L;

        long days = hours / 24L;
        hours -= days * 24L;

        string result = $"{min:00}m {sec:00}s";
        if (days > 0)
        {
            result = $"{days}d {hours:00}h {result}";
        }
        else if (hours > 0)
        {
            result = $"{hours:00}h {result}";
        }

        return result;
    }
}
