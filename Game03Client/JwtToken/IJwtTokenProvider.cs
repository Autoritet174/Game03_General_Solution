using System.Threading.Tasks;

namespace Game03Client.JwtToken;

public interface IJwtTokenProvider
{
    Task<JwtTokenResult?> GetTokenAsync(string jsonBody);

    string? GetTokenIfExists();
}
