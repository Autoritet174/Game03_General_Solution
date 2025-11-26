using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.JwtToken;

public interface IJwtToken
{
    Task<string?> GetTokenAsync(string jsonBody, CancellationToken cancellationToken);

    string? GetTokenIfExists();

    void DeleteToken();
}
