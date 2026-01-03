using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Game03Client.JwtToken;
using Game03Client.Logger;
using General;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.HttpRequester;

public class HttpRequesterProvider
{
    private static readonly HttpClient httpClient = new();
    private readonly IniFileProvider iniFileProvider;
    private readonly InternetCheckerProvider _internetCheckerProvider;
    private readonly LoggerProvider<HttpRequesterProvider> logger;

    public HttpRequesterProvider(IniFileProvider iniFileProvider, InternetCheckerProvider internetCheckerProvider, LoggerProvider<HttpRequesterProvider> logger)
    {
        this.iniFileProvider = iniFileProvider;
        _internetCheckerProvider = internetCheckerProvider;
        this.logger = logger;
        double timeout = this.iniFileProvider.ReadDouble("Http", "Timeout", 30d);
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
    }


    public async Task<string?> GetResponseAsync(string url, CancellationToken cancellationToken, string? jsonBody = null, string? jwtToken = null)
    {
        if (url.IsEmpty())
        {
            string e = "url IsEmpty";
            logger.LogError(e);
            throw new Exception(e);
        }

        Uri uri = new(url);
        if (uri == null)
        {
            string e = "uri is null";
            logger.LogError(e);
            throw new Exception(e);
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
                request.Content = new StringContent(jsonBody, Encoding.UTF8, G.APPLICATION_JSON);
            }

            if (!jwtToken.IsEmpty())
            {
                // Если был передан токен то подставляем его в заголовок как авторизацию
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }
            

            using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.IsEmpty())
            {
                logger.LogError($"responseContent IsEmpty, StatusCode={response.StatusCode}", L.Error.Server.InvalidResponse);
                return null;
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
            logger.LogError(ex.ToString(), L.Error.Server.Timeout);
            return null;
        }
        catch (HttpRequestException ex) when (ex.InnerException is WebException)
        {
            bool haveInternet = await _internetCheckerProvider.CheckInternetConnectionAsync(cancellationToken);
            string key = haveInternet ? L.Error.Server.Unavailable : L.Error.Server.NoInternetConnection;
            logger.LogError(ex.ToString(), key);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString(), L.Error.Server.InvalidResponse);
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
