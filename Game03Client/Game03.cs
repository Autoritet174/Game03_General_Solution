using Game03Client.PlayerCollection;
using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Game03Client.JwtToken;
using Game03Client.LocalizationManager;
using Game03Client.Logger;
using Game03Client.WebSocketClient;
using General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game03Client;

/// <summary>
/// Класс содержащий логику и веб взаимодействие игрового клиента.
/// </summary>
public sealed class Game03 : IAsyncDisposable
{
    private readonly ServiceProvider _provider;

    /// <summary>
    /// Провайдер JwtToken.
    /// </summary>
    public IJwtToken JwtToken { get; private set; }

    /// <summary>
    /// Провайдер IniFile.
    /// </summary>
    public IIniFile IniFile { get; private set; }

    /// <summary>
    /// Провайдер HttpRequester.
    /// </summary>
    public IHttpRequester HttpRequester { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public ILocalizationManager LocalizationManager { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public IWebSocketClient WebSocketClient { get; private set; }

    /// <summary>
    /// Провайдер GameData.
    /// </summary>
    public IGameData GameData { get; private set; }

    /// <summary>
    /// Провайдер Collection.
    /// </summary>
    public IPlayerCollection Collection { get; private set; }

    /// <summary>
    /// Провайдер Logger.
    /// </summary>
    //public ILogger<Game03> Logger { get; private set; }

    private Game03(ServiceProvider provider)
    {
        _provider = provider;
        //Logger = provider.GetRequiredService<ILogger<Game03>>();
        JwtToken = provider.GetRequiredService<IJwtToken>();
        IniFile = provider.GetRequiredService<IIniFile>();
        HttpRequester = provider.GetRequiredService<IHttpRequester>();
        LocalizationManager = provider.GetRequiredService<ILocalizationManager>();
        WebSocketClient = provider.GetRequiredService<IWebSocketClient>();
        GameData = provider.GetRequiredService<IGameData>();
        Collection = provider.GetRequiredService<IPlayerCollection>();
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    /// <returns></returns>
    public ValueTask DisposeAsync()
    {
        _provider.Dispose();
        return default;
    }

    /// <summary>
    /// Фабрика.
    /// </summary>
    /// <param name="iniFileFullPath"></param>
    /// <param name="stringCapsuleJsonFileData"></param>
    /// <param name="languageGame"></param>
    /// <param name="loggerCallback"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static Game03 Create(string iniFileFullPath, StringCapsule stringCapsuleJsonFileData, GameLanguage languageGame, LoggerCallback loggerCallback, Action<IServiceCollection>? configure = null)
    {
        ServiceCollection services = new();

        // Logger
        _ = services.AddSingleton(new LoggerOptions(loggerCallback));
        // Logger - регистрируем открытый generic тип
        _ = services.AddSingleton(typeof(ILogger<>), typeof(LoggerProvider<>));

        // JwtToken
        _ = services.AddSingleton<JwtTokenCache>();
        _ = services.AddSingleton<IJwtToken, JwtTokenProvider>();

        // IniFile
        _ = services.AddSingleton(new IniFileOptions(iniFileFullPath));
        _ = services.AddSingleton<IIniFile, IniFileProvider>();

        // InternetCheckerProvider
        _ = services.AddSingleton<IInternetChecker, InternetCheckerProvider>();

        // HttpRequesterProvider
        _ = services.AddSingleton<IHttpRequester, HttpRequesterProvider>();

        // LocalizationManagerProvider
        _ = services.AddSingleton(new LocalizationManagerOptions(stringCapsuleJsonFileData, languageGame));
        _ = services.AddSingleton<ILocalizationManager, LocalizationManagerProvider>();

        // WebSocketClientProvider
        _ = services.AddSingleton<IWebSocketClient, WebSocketClientProvider>();

        // GlobalFunctions
        _ = services.AddSingleton<GameDataCache>();
        _ = services.AddSingleton<IGameData, GameDataProvider>();

        // Collection
        _ = services.AddSingleton<PlayerCollectionCache>();
        _ = services.AddSingleton<IPlayerCollection, PlayerCollectionProvider>();


        configure?.Invoke(services); // опциональные переопределения

        ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        return new Game03(provider);
    }

}
