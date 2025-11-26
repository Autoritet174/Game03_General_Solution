using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.InternetChecker;

internal interface IInternetChecker
{
    Task<bool> CheckInternetConnectionAsync(CancellationToken cancellationToken);
}
