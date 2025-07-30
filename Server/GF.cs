using System.Diagnostics;
using System.Net.Mail;

namespace Server;

public static class GF
{
    private static readonly bool Debugger_IsNotAttached;
    static GF()
    {
        Debugger_IsNotAttached = !Debugger.IsAttached;
    }

    public static bool IsValidEmail(string email)
    {
        return MailAddress.TryCreate(email, out _);
    }

    /// <summary>
    /// Проверка получился ли hash из пустой строки
    /// </summary>
    /// <param name="hash256"></param>
    /// <returns></returns>
    public static bool IsHashFromEmpty(string hash256)
    {
        return hash256.Equals("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", StringComparison.CurrentCultureIgnoreCase);
    }


    /// <summary>
    /// Добавляет задержку 2000 миллисекунд если работает без отладчика.
    /// </summary>
    /// <returns></returns>
    public static async Task DelayWithOutDebug2000()
    {
        if (Debugger_IsNotAttached)
        {
            await Task.Delay(2000);
        }
    }


    /// <summary>
    /// Добавляет задержку 2000 миллисекунд если работает без отладчика.
    /// </summary>
    /// <returns></returns>
    public static async Task DelayWithOutDebug500()
    {
        if (Debugger_IsNotAttached)
        {
            await Task.Delay(500);
        }
    }
}
