using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace General;

public static class Functions
{

    public static bool IsEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            _ = new MailAddress(email);
            return true;
        }
        //catch (FormatException)
        //{
        //    return false;
        //}
        catch (Exception)
        {
            return false;
        }
    }


    public static async Task<HttpResponseMessage> GetHttpResponseAsync(Uri uri, object content)
    {
        using HttpClient client = new();
        client.Timeout = TimeSpan.FromSeconds(10);
        string json = JsonConvert.SerializeObject(content);
        StringContent stringContent = new(json, Encoding.UTF8, "application/json");
        return await client.PostAsync(uri, stringContent);
    }

}
