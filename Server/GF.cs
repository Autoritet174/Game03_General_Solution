using System.Diagnostics;
using System.Net.Mail;

namespace Server;

public static class GF
{
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
    /// Добавляет задержку если работает без отладчика
    /// </summary>
    /// <returns></returns>
    public static async Task DelayWithOutDebug() {
        if (!Debugger.IsAttached)
        {
            await Task.Delay(2000);
        }
    }
}
