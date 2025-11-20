using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public interface IPlayerCollectionProvider
{
    Task LoadAllCollectionFromServer(CancellationToken cancellationToken);
}
