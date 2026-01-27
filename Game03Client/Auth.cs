using General;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

using LOGGER = Game03Client.LOGGER<Game03Client.Auth>;

namespace Game03Client;

public class Auth
{
    public enum AuthType
    {
        Login,
        RefreshTokens
    }

    public static async Task<bool> AuthentificationAsync(DtoRequestAuthReg dto, AuthType authType , CancellationToken cancellationToken = default)
    {
        AccessToken = null;
        RefreshToken = null;
        RefreshTokenExpirationAt = null;

        if (cancellationToken.IsCancellationRequested)
        {
            LOGGER.LogError("IsCancellationRequested");
            return false;
        }
        string url = authType == AuthType.Login ? Url.AUTH_LOGIN : Url.AUTH_REFRESH_TOKENS;

        string? response = await HttpRequester.GetResponseAsync(url, JsonConvert.SerializeObject(dto), cancellationToken);
        if (response == null)
        {
            LOGGER.LogError("response is null", L.Error.Server.InvalidResponse);
            return false;
        }

        DtoResponseAuthReg? dtoResponse = JsonConvert.DeserializeObject<DtoResponseAuthReg>(response);
        if (dtoResponse == null)
        {
            LOGGER.LogError("dtoResponse is null", L.Error.Server.InvalidResponse);
            return false;
        }

        if (!string.IsNullOrEmpty(dtoResponse.ErrorKey))
        {
            //LOGGER.LogError($"ErrorKey: {dtoResponse.ErrorKey}", dtoResponse.ErrorKey);
            return false;
        }

        AccessToken = dtoResponse.AccessToken;
        RefreshToken = dtoResponse.RefreshToken;
        RefreshTokenExpirationAt = dtoResponse.ExtraDateTimeOffset;
        return true;
    }

    public static string? AccessToken { get; set; }
    public static string? RefreshToken { get; private set; }
    public static DateTimeOffset? RefreshTokenExpirationAt { get; private set; }
}
