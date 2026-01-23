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
    public static DtoResponseAuthReg? Dto { get; private set; }
    public static async Task RefreshTokensAsync(DtoRequestAuthReg dto, CancellationToken cancellationToken)
    {
        Dto = null;
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        string? response = await HttpRequester.GetResponseAsync(Url.Auth, JsonConvert.SerializeObject(dto), cancellationToken);
        if (response == null)
        {
            LOGGER.LogError("response is null", L.Error.Server.InvalidResponse);
            throw new Exception();
        }

        Dto = JsonConvert.DeserializeObject<DtoResponseAuthReg>(response) ?? throw new Exception();
    }

    public static string? AccessToken => Dto?.AccessToken;
}
