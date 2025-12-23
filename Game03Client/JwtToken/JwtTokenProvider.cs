using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.JwtToken;

internal class JwtTokenProvider(JwtTokenCache jwtTokenCache, IHttpRequester httpRequesterProvider, ILogger<JwtTokenProvider> logger) : IJwtToken
{
    public async Task<string?> GetTokenAsync(string jsonBody, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        // Если есть токен — возвращаем сразу
        if (!jwtTokenCache.Token.IsEmpty())
        {
            return jwtTokenCache.Token;
        }

        string? response = await httpRequesterProvider.GetResponseAsync(Url.Authentication, cancellationToken, jsonBody, false);
        if (response == null) {
            logger.Log("response is null", L.Error.Server.InvalidResponse);
            return null;
        }
        JObject? jObject;
        try
        {
            jObject = JObject.Parse(response);
        }
        catch
        {
            logger.Log("jObject can't be parced", L.Error.Server.InvalidResponse);
            return null;
        }
        if (jObject is null)
        {
            logger.Log("jObject is null", L.Error.Server.InvalidResponse);
            return null;
        }

        string? token = jObject["token"]?.ToString();
        if (token.IsEmpty())
        {
            logger.Log("token IsEmpty");
            return null;
        }

        jwtTokenCache.Token = token;
        return token;
    }

    public string? GetTokenIfExists()
    {
        return jwtTokenCache.Token.IsEmpty() ? null : jwtTokenCache.Token;
    }

    public void DeleteToken()
    {
        jwtTokenCache.Token = null;
    }
}
