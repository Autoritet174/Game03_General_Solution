using Game03Client.HttpRequester;
using Game03Client.IniFile;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionProvider(PlayerCollectionCache _collectionCache, IIniFileProvider _iniFile, IHttpRequesterProvider _httpRequester, ILoggerProvider _logger) : IPlayerCollectionProvider
{
    #region Logger
    private const string NAME_THIS_CLASS = nameof(PlayerCollectionProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger

    /// <summary>
    /// 
    /// </summary>
    public async Task LoadAllCollectionFromServer(CancellationToken cancellationToken) {

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        // Получить коллекцию героев игрока
        JObject? jObject = await _httpRequester.GetJObjectAsync(General.Url.Inventory.Heroes, cancellationToken);
        if (jObject == null)
        {
            Log("jObject == null");
            return;
        }


    }
}
