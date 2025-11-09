using System.Threading.Tasks;

namespace Client.JwtToken;

internal interface IJwtTokenProvider
{
    Task<string> GetTokenAsync();

    void Reset();
}
