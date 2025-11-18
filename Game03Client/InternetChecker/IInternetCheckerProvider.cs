using System.Threading.Tasks;

namespace Game03Client.InternetChecker;

internal interface IInternetCheckerProvider
{
    Task<bool> CheckInternetConnectionAsync();
}
