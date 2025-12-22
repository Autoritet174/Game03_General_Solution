using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.HttpRequester;

public interface IHttpRequester
{
    Task<string?> GetResponseAsync(string url, CancellationToken cancellationToken, string? jsonBody = null, bool useJwtToken = true);
}
