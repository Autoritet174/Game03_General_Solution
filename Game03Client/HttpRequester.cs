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


public class HttpRequester
{
    private static readonly Logger<HttpRequester> logger = new();
    private static readonly HttpClient httpClient = new();

    public static void Init()
    {
        double timeout = IniFile.ReadDouble("Http", "Timeout", 30d);
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
    }


    public static async Task<string?> GetResponseAsync(string url, string? jsonBody, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            logger.LogError("url IsEmpty");
            return null;
        }

        Uri uri = new(url);
        if (uri == null)
        {
            logger.LogError("uri is null");
            return null;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogError("IsCancellationRequested");
            return null;
        }

        try
        {
            using HttpRequestMessage request = new(HttpMethod.Post, uri);
            if (jsonBody != null)
            {
                request.Content = new StringContent(jsonBody, Encoding.UTF8, GlobalHelper.APPLICATION_JSON);
            }
            if (!string.IsNullOrWhiteSpace(Auth.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);
            }


            using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            string? responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                logger.LogError($"responseContent IsEmpty, StatusCode={response.StatusCode}, url={url}", L.Error.Server.InvalidResponse);
                return null;
            }

            return responseContent;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.LogException(ex, L.Error.Server.Timeout);
            return null;
        }
        catch (HttpRequestException ex) when (ex.InnerException is WebException)
        {
            bool haveInternet = await InternetChecker.CheckInternetConnectionAsync(cancellationToken).ConfigureAwait(false);
            string key = haveInternet ? L.Error.Server.Unavailable : L.Error.Server.NoInternetConnection;
            logger.LogException(ex, key);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogException(ex, L.Error.Server.InvalidResponse);
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
