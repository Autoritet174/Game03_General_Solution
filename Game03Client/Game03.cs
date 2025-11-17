using Game03Client.GlobalFunctions;
using Game03Client.HttpRequester;
using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Game03Client.JwtToken;
using Game03Client.LocalizationManager;
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
    public IJwtTokenProvider JwtTokenProvider { get; private set; }

    /// <summary>
    /// Провайдер IniFile.
    /// </summary>
    public IIniFileProvider IniFileProvider { get; private set; }

    /// <summary>
    /// Провайдер HttpRequester.
    /// </summary>
    public IHttpRequesterProvider HttpRequesterProvider { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public ILocalizationManagerProvider LocalizationManagerProvider { get; private set; }

    /// <summary>
    /// Провайдер LocalizationManager.
    /// </summary>
    public IWebSocketClientProvider WebSocketClientProvider { get; private set; }

    /// <summary>
    /// Провайдер GlobalFunctions.
    /// </summary>
    public IGlobalFunctionsProvider GlobalFunctionsProvider { get; private set; }

    private Game03(ServiceProvider provider)
    {
        _provider = provider;
        JwtTokenProvider = provider.GetRequiredService<IJwtTokenProvider>();
        IniFileProvider = provider.GetRequiredService<IIniFileProvider>();
        HttpRequesterProvider = provider.GetRequiredService<IHttpRequesterProvider>();
        LocalizationManagerProvider = provider.GetRequiredService<ILocalizationManagerProvider>();
        WebSocketClientProvider = provider.GetRequiredService<IWebSocketClientProvider>();
        GlobalFunctionsProvider = provider.GetRequiredService<IGlobalFunctionsProvider>();
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
    /// <param name="configure"></param>
    /// <returns></returns>
    public static Game03 Create(string iniFileFullPath, StringCapsule stringCapsuleJsonFileData, GameLanguage languageGame, Action<IServiceCollection>? configure = null)
    {
        ServiceCollection services = new();

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

        configure?.Invoke(services); // опциональные переопределения

        ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        return new Game03(provider);
    }
}
