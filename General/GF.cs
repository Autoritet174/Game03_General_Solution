using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace General;

/// <summary>
/// Global Functions
/// </summary>
public static class GF
{

    /// <summary>
    /// Пытается создать объект MailAddress из строки. Возвращает обратно входную строку без изменений при успехи, иначе null.
    /// </summary>
    /// <param name="email">Строка с предполагаемым адресом электронной почты.</param>
    /// <returns>email, если создание прошло успешно; иначе null.</returns>
    public static string? GetEmailOrNull(string email)
    {
        // Проверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        try
        {
            // Попытка создания MailAddress
            _ = new MailAddress(email);
            return email;
        }
        catch (FormatException)
        {
            // Невалидный формат email
            return null;
        }
        catch (Exception)
        {
            // другое исключение
            return null;
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
