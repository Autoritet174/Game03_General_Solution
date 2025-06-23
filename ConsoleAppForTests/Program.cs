using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        string emptyStringHash = CalculateSha256("");
        Console.WriteLine(emptyStringHash); // Выведет e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
    }
    static string CalculateSha256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

   

}