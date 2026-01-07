using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.JwtToken;

public class JwtTokenProvider(HttpRequesterProvider httpRequester, LoggerProvider<JwtTokenProvider> logger)
{
    public string? AccessToken { get; private set; } = null;
    public string? RefreshToken { get; private set; } = null;
    public async Task RefreshTokensAsync(string jsonBody, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        // Если есть токен — возвращаем сразу
        if (!string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(RefreshToken))
        {
            return;
        }

        string? response = await httpRequester.GetResponseAsync(Url.Auth, cancellationToken, jsonBody);
        if (response == null)
        {
            logger.LogAndThrow("response is null", L.Error.Server.InvalidResponse);
            return;
        }
        JObject? jObject;
        try
        {
            jObject = JObject.Parse(response);
        }
        catch
        {
            logger.LogAndThrow("jObject can't be parced", L.Error.Server.InvalidResponse);
            return;
        }
        if (jObject is null)
        {
            logger.LogAndThrow("jObject is null", L.Error.Server.InvalidResponse);
            return;
        }

        string? accessToken = jObject["accessToken"]?.ToString();
        string? refresh_token = jObject["refreshToken"]?.ToString();
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            logger.LogAndThrow("accessToken IsEmpty");
            //return null;
        }

        this.AccessToken = accessToken;
        return token;
    }

}
