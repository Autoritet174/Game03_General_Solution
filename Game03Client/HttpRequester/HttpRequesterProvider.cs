using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Game03Client.JwtToken;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.HttpRequester;

internal class HttpRequesterProvider : IHttpRequester
{
    #region Logger
    private readonly ILogger _logger;
    private const string NAME_THIS_CLASS = nameof(HttpRequesterProvider);

    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger

    private readonly HttpClient _httpClient;
    private readonly IIniFile _iniFileProvider;
    private readonly IInternetChecker _internetCheckerProvider;
    private readonly JwtTokenCache _tokenCache;


    public HttpRequesterProvider(IIniFile iniFileProvider, IInternetChecker internetCheckerProvider, JwtTokenCache tokenCache, ILogger logger)
    {
        _iniFileProvider = iniFileProvider;
        _internetCheckerProvider = internetCheckerProvider;
        _tokenCache = tokenCache;
        _logger = logger;
        _httpClient = new();
        double timeout = _iniFileProvider.ReadDouble("Http", "Timeout", 30d);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeout);
    }


    public async Task<string?> GetResponseAsync(string url, CancellationToken cancellationToken, string? jsonBody = null, bool useJwtToken = true)
    {
        if (url.IsEmpty())
        {
            string e = "url IsEmpty";
            Log(e);
            throw new Exception(e);
        }

        Uri uri = new(url);
        if (uri == null)
        {
            string e = "uri is null";
            Log(e);
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

            if (useJwtToken)
            {
                string? jwtToken = _tokenCache.Token;
                if (!jwtToken.IsEmpty())
                {
                    // Если был передан токен то подставляем его в заголовок как авторизацию
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                }
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.IsEmpty())
            {
                Log("responseContent IsEmpty", L.Error.Server.InvalidResponse);
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
            Log(ex.ToString(), L.Error.Server.Timeout);
            return null;
        }
        catch (HttpRequestException ex) when (ex.InnerException is WebException)
        {
            bool haveInternet = await _internetCheckerProvider.CheckInternetConnectionAsync(cancellationToken);
            string key = haveInternet ? L.Error.Server.Unavailable : L.Error.Server.NoInternetConnection;
            Log(ex.ToString(), key);
            return null;
        }
        catch (Exception ex)
        {
            Log(ex.ToString(), L.Error.Server.InvalidResponse);
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
