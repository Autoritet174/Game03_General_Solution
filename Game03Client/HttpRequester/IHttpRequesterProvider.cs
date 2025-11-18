using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.HttpRequester;

public interface IHttpRequesterProvider
{
    Task<JObject?> GetJObjectAsync(string url, CancellationToken cancellationToken, string? jsonBody = null, bool useJwtToken = true);
}
