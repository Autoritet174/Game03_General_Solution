using General;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;


namespace Game03Client;

using LOGGER = LOGGER<HttpRequester>;

public class HttpRequester
{
    private static readonly HttpClient httpClient = new();

    public static void Init() {
        double timeout = IniFile.ReadDouble("Http", "Timeout", 30d);
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
    }


    public static async Task<string?> GetResponseAsync(string url, string? jsonBody = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            string e = "url IsEmpty";
            LOGGER.LogError(e);
            throw new Exception();
        }

        Uri uri = new(url);
        if (uri == null)
        {
            string e = "uri is null";
            LOGGER.LogError(e);
            throw new Exception();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            using HttpRequestMessage request = new(HttpMethod.Post, uri);
            if (jsonBody != null)
            {
                request.Content = new StringContent(jsonBody, Encoding.UTF8, GlobalHelper.APPLICATION_JSON);
            }
            string? accessToken = Auth.AccessToken;
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // Если был передан токен то подставляем его в заголовок как авторизацию
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }


            using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                LOGGER.LogError($"responseContent IsEmpty, StatusCode={response.StatusCode}", L.Error.Server.InvalidResponse);
                throw new Exception();
            }
            return responseContent;

            //try
            //{
            //    var jObject = JObject.Parse(responseContent);
            //    if (jObject is null)
            //    {
            //        Log("jObject is null", L.Error.Server.InvalidResponse);
            //        return null;
            //    }

            //    return jObject;
            //}
            //catch
            //{
            //    Log("jObject can't be parced", L.Error.Server.InvalidResponse);
            //    return null;
            //}
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            LOGGER.LogError(ex.ToString(), L.Error.Server.Timeout);
            return null;
        }
        catch (HttpRequestException ex) when (ex.InnerException is WebException)
        {
            bool haveInternet = await InternetChecker.CheckInternetConnectionAsync(cancellationToken);
            string key = haveInternet ? L.Error.Server.Unavailable : L.Error.Server.NoInternetConnection;
            LOGGER.LogError(ex.ToString(), key);
            return null;
        }
        catch (Exception ex)
        {
            LOGGER.LogError(ex.ToString(), L.Error.Server.InvalidResponse);
            return null;
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
