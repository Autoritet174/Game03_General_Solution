using Game03Client.HttpRequester;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Game03Client.JwtToken;

internal class JwtTokenProvider(JwtTokenCache jwtTokenCache, IHttpRequesterProvider httpRequesterProvider) : IJwtTokenProvider
{
    private static void Error(string error)
    {
        Console.WriteLine($"[{nameof(JwtTokenProvider)}] {error}");
    }
    public async Task<JwtTokenResult> GetTokenAsync(string jsonBody)
    {
        // Если есть токен — возвращаем сразу
        if (!string.IsNullOrWhiteSpace(jwtTokenCache.Token))
        {
            return new JwtTokenResult(jwtTokenCache.Token);
        }

        HttpRequesterResult? httpRequesterProviderResult = await httpRequesterProvider.GetResponceAsync(General.URLs.Uri_login, jsonBody);
        if (httpRequesterProviderResult == null)
        {
            Error("httpRequesterProviderResult is null");
            return new JwtTokenResult(null, L.Error.Server.InvalidResponse);
        }

        if (!httpRequesterProviderResult.Success)
        {
            Error("httpRequesterProviderResult.Success is false");
            return new JwtTokenResult(null, httpRequesterProviderResult.KeyError ?? L.Error.Server.InvalidResponse);
        }

        if (httpRequesterProviderResult.JObject == null)
        {
            Error("httpRequesterProviderResult.JObject is null");
            return new JwtTokenResult(null, httpRequesterProviderResult.KeyError ?? L.Error.Server.InvalidResponse);
        }

        JObject jObject = httpRequesterProviderResult.JObject;
        string? token = jObject["token"]?.ToString();
        if (string.IsNullOrWhiteSpace(token))
        {
            Error("httpRequesterProviderResult.JObject.token is null");
            return new JwtTokenResult(null, httpRequesterProviderResult.KeyError ?? L.Error.Server.InvalidResponse);
        }

        return new JwtTokenResult(token);
    }

    public string? GetTokenIfExists()
    {
        return string.IsNullOrWhiteSpace(jwtTokenCache.Token) ? null : jwtTokenCache.Token;
    }
}
