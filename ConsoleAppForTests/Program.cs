using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UUIDNext;//7,7021584 //100_000_000

internal class Program
{
    private static void Main(string[] args)
    {
        DateTime dateTime = DateTime.Now;
        for (int i = 0; i < 100_000_000; i++)
        {
            Guid uuidV7 = Uuid.NewDatabaseFriendly(Database.PostgreSql);
        }
        Console.WriteLine((DateTime.Now - dateTime).TotalSeconds.ToString());
        //Console.WriteLine(ExtractTimestamp(Guid.Parse(
        //    Uuid.NewDatabaseFriendly(Database.PostgreSql).ToString()
        //    //"0197d0e6-c5d4-7955-8241-771e21747ffc"
        //    )));
    }
    //static string CalculateSha256(string input)
    //{
    //    using (SHA256 sha256 = SHA256.Create())
    //    {
    //        byte[] bytes = Encoding.UTF8.GetBytes(input);
    //        byte[] hashBytes = sha256.ComputeHash(bytes);
    //        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    //    }
    //}
    public static DateTime ExtractTimestamp(Guid uuid)
    {
        // Строка из 32 шестнадцатеричных символов без дефисов.
        string hex = uuid.ToString("N", CultureInfo.InvariantCulture);

        // Проверка версии: 13-й символ (index 12) должен быть '7'.
        if (hex[12] != '7')
            throw new ArgumentException("UUID is not version 7.", nameof(uuid));

        // Первые 12 символов (48 бит) — метка времени в миллисекундах.
        ulong msSinceUnixEpoch = ulong.Parse(hex.AsSpan(0, 12), NumberStyles.AllowHexSpecifier);

        // Преобразование в DateTime (UTC).
        return DateTime.UnixEpoch.AddMilliseconds(msSinceUnixEpoch);
    }


}