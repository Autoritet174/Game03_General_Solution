using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Game03Client.HttpRequester;

internal class HttpRequesterProvider : IHttpRequesterProvider
{
    private readonly HttpClient _httpClient;
    private readonly IIniFileProvider _iniFileProvider;
    private readonly IInternetCheckerProvider _internetCheckerProvider;

    private static void Error(string error) {
        Console.WriteLine($"[{nameof(HttpRequesterProvider)}] {error}");
    }

    internal HttpRequesterProvider(IIniFileProvider iniFileProvider, IInternetCheckerProvider internetCheckerProvider)
    {
        _iniFileProvider = iniFileProvider;
        _internetCheckerProvider = internetCheckerProvider;
        _httpClient = new();
        double timeout = _iniFileProvider.ReadDouble("Http", "timeout", 10d);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeout);
    }

    public async Task<HttpRequesterResult?> GetResponceAsync(Uri uri, string? jsonBody = null, string? jwtToken = null)
    {
        if (uri == null)
        {
            Error("uri is null");
            throw new ArgumentNullException(nameof(uri));
        }

        try
        {
            using HttpRequestMessage request = new(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonBody ?? "{}", Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                // Если был передан токен то подставляем его в заголовок как авторизацию
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                Error("responseContent is null");
                return new HttpRequesterResult(false, keyError: L.Error.Server.InvalidResponse);
            }

            try
            {
                JObject jObject = JObject.Parse(responseContent);
                return new HttpRequesterResult(response.IsSuccessStatusCode, jObject);
            }
            catch
            {
                Error("jObject can't be parced");
                return new HttpRequesterResult(false, keyError: L.Error.Server.InvalidResponse);
            }
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpRequesterResult(false, keyError: L.Error.Server.Timeout);
        }
        catch (HttpRequestException ex) when (ex.InnerException is WebException)
        {
            bool haveInternet = await _internetCheckerProvider.CheckInternetConnectionAsync();

            return new HttpRequesterResult(false, keyError: haveInternet ? L.Error.Server.Unavailable : L.Error.Server.NoInternetConnection);
        }
        catch (Exception ex)
        {
            return new HttpRequesterResult(false, ex: ex);
        }
    }


    /// <summary>
    /// Метод для преобразования заголовков в строку
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static string GetHeadersAsString(HttpResponseMessage response)
    {
        HttpResponseHeaders headers = response.Headers;
        HttpContentHeaders contentHeaders = response.Content.Headers;

        StringBuilder headersString = new();
        _ = headersString.AppendLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");

        // Добавляем основные заголовки ответа
        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.IEnumerable<string>> header in headers)
        {
            _ = headersString.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }

        // Добавляем заголовки содержимого (если есть)
        if (contentHeaders != null)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.IEnumerable<string>> header in contentHeaders)
            {
                _ = headersString.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }

        return headersString.ToString();
    }

   
}
