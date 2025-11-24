using Game03Client.PlayerCollection;
using Game03Client.GlobalFunctions;
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
    public IJwtTokenProvider JwtToken { get; private set; }

    /// <summary>
    /// Провайдер IniFile.
    /// </summary>
    public IIniFileProvider IniFile { get; private set; }

    /// <summary>
    /// Провайдер HttpRequester.
    /// </summary>
    public IHttpRequesterProvider HttpRequester { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public ILocalizationManagerProvider LocalizationManager { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public IWebSocketClientProvider WebSocketClient { get; private set; }

    /// <summary>
    /// Провайдер GlobalFunctions.
    /// </summary>
    public IGlobalFunctionsProvider GlobalFunctions { get; private set; }

    /// <summary>
    /// Провайдер Collection.
    /// </summary>
    public IPlayerCollectionProvider Collection { get; private set; }

    /// <summary>
    /// Провайдер Logger.
    /// </summary>
    public ILoggerProvider Logger { get; private set; }

    private Game03(ServiceProvider provider)
    {
        _provider = provider;
        Logger = provider.GetRequiredService<ILoggerProvider>();
        JwtToken = provider.GetRequiredService<IJwtTokenProvider>();
        IniFile = provider.GetRequiredService<IIniFileProvider>();
        HttpRequester = provider.GetRequiredService<IHttpRequesterProvider>();
        LocalizationManager = provider.GetRequiredService<ILocalizationManagerProvider>();
        WebSocketClient = provider.GetRequiredService<IWebSocketClientProvider>();
        GlobalFunctions = provider.GetRequiredService<IGlobalFunctionsProvider>();
        Collection = provider.GetRequiredService<IPlayerCollectionProvider>();
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
        _ = services.AddSingleton<ILoggerProvider, LoggerProvider>();

        // JwtToken
        _ = services.AddSingleton<JwtTokenCache>();
        _ = services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

        // IniFile
        _ = services.AddSingleton(new IniFileOptions(iniFileFullPath));
        _ = services.AddSingleton<IIniFileProvider, IniFileProvider>();

        // InternetCheckerProvider
        _ = services.AddSingleton<IInternetCheckerProvider, InternetCheckerProvider>();

        // HttpRequesterProvider
        _ = services.AddSingleton<IHttpRequesterProvider, HttpRequesterProvider>();

        // LocalizationManagerProvider
        _ = services.AddSingleton(new LocalizationManagerOptions(stringCapsuleJsonFileData, languageGame));
        _ = services.AddSingleton<ILocalizationManagerProvider, LocalizationManagerProvider>();

        // WebSocketClientProvider
        _ = services.AddSingleton<IWebSocketClientProvider, WebSocketClientProvider>();

        // GlobalFunctions
        _ = services.AddSingleton<GlobalFunctionsProviderCache>();
        _ = services.AddSingleton<IGlobalFunctionsProvider, GlobalFunctionsProvider>();

        // Collection
        _ = services.AddSingleton<PlayerCollectionCache>();
        _ = services.AddSingleton<IPlayerCollectionProvider, PlayerCollectionProvider>();


        configure?.Invoke(services); // опциональные переопределения

        ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        return new Game03(provider);
    }
}
