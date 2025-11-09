using Client.IniFile;
using Client.JwtToken;
using IniParser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Client;

public sealed class GameClient : IAsyncDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IJwtTokenProvider jwtTokenProvider;

    internal GameClient(ServiceProvider provider)
    {
        _provider = provider;
        jwtTokenProvider = provider.GetRequiredService<JwtTokenProvider>();
    }

    public async Task<string> JwtTokenGetAsync()
    {
        return await jwtTokenProvider.GetTokenAsync();
    }

    public ValueTask DisposeAsync()
    {
        _provider.Dispose();
        return default;
    }

    public static GameClient Create(string iniFileFullPath, Action<IServiceCollection>? configure = null)
    {
        ServiceCollection services = new();

        // JwtToken
        _ = services.AddSingleton<JwtTokenCache>();
        _ = services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

        // IniFile
        _ = services.AddSingleton(new IniFileOptions(iniFileFullPath));
        _ = services.AddSingleton<IIniFileProvider, IniFileProvider>();
        _ = services.AddSingleton<FileIniDataParser>();


        configure?.Invoke(services); // опциональные переопределения

        ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        return new GameClient(provider);
    }
}
