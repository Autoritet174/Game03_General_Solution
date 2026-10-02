
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using System;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client;

public class Auth
{
    private static readonly Logger<Auth> logger = new();
    public enum AuthType
    {
        Login,
        RefreshTokens
    }

    public static async Task<bool> AuthentificationAsync(DtoRequestAuthReg dto, AuthType authType, CancellationToken cancellationToken)
    {
        accessToken = null;
        refreshToken = null;
        refreshTokenExpirationAt = null;

        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogError("IsCancellationRequested");
            return false;
        }
        string url = authType == AuthType.Login ? Url.authLogin : Url.authRefreshTokens;

        string? response = await HttpRequester.GetResponseAsync(url, JSON.Serialize(dto), cancellationToken).ConfigureAwait(false);
        if (response == null)
        {
            logger.LogError("response is null", L.Error.Server.invalidResponse);
            return false;
        }

        DtoResponseAuthReg? dtoResponse = JSON.Deserialize<DtoResponseAuthReg>(response);
        if (dtoResponse == null)
        {
            logger.LogError("dtoResponse is null", L.Error.Server.invalidResponse);
            return false;
        }

        if (!string.IsNullOrEmpty(dtoResponse.errorKey))
        {
            //logger.LogError($"ErrorKey: {dtoResponse.ErrorKey}", dtoResponse.ErrorKey);
            return false;
        }

        accessToken = dtoResponse.accessToken;
        refreshToken = dtoResponse.refreshToken;
        refreshTokenExpirationAt = dtoResponse.extraDateTimeOffset;
        return true;
    }

    public static string? accessToken { get; set; }
    public static string? refreshToken { get; private set; }
    public static DateTimeOffset? refreshTokenExpirationAt { get; private set; }
}
