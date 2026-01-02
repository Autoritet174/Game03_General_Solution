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
    public string? token = null;
    public async Task<string?> GetTokenAsync(string jsonBody, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        // Если есть токен — возвращаем сразу
        if (!this.token.IsEmpty())
        {
            return this.token;
        }

        string? response = await httpRequester.GetResponseAsync(Url.Auth, cancellationToken, jsonBody);
        if (response == null)
        {
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

        this.token = token;
        return token;
    }

}
