using Game03Client.IniFile;
using Game03Client.InternetChecker;
using Game03Client.JwtToken;
using Microsoft.Extensions.DependencyInjection;
using System;
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

    private Game03(ServiceProvider provider)
    {
        _provider = provider;
        JwtToken = provider.GetRequiredService<JwtTokenProvider>();
        IniFile = provider.GetRequiredService<IniFileProvider>();
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

    public static Game03 Create(string iniFileFullPath, Action<IServiceCollection>? configure = null)
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

        configure?.Invoke(services); // опциональные переопределения

        ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        return new Game03(provider);
    }
}
