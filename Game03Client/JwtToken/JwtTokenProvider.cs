using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.JwtToken;

internal class JwtTokenProvider(JwtTokenCache jwtTokenCache, IHttpRequesterProvider httpRequesterProvider, ILoggerProvider logger) : IJwtTokenProvider
{
    #region Logger
    private readonly ILoggerProvider _logger = logger;
    private const string NAME_THIS_CLASS = nameof(JwtTokenProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger

    public async Task<string?> GetTokenAsync(string jsonBody, CancellationToken cancellationToken)
    {
        // Если есть токен — возвращаем сразу
        if (!jwtTokenCache.Token.IsEmpty())
        {
            return jwtTokenCache.Token;
        }

        JObject? jObject = await httpRequesterProvider.GetJObjectAsync(Url.Authentication, cancellationToken, jsonBody, false);
        if (jObject == null)
        {
            return null;
        }

        string? token = jObject["token"]?.ToString();
        if (token.IsEmpty())
        {
            Log("token IsEmpty");
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
