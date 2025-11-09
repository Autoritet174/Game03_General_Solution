using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client.JwtToken;

internal class JwtTokenProvider(JwtTokenCache cache) : IJwtTokenProvider
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<string> GetTokenAsync()
    {
        // Если есть неистекший токен — возвращаем сразу
        if (cache.Token != null && cache.ExpiresAt > DateTimeOffset.UtcNow)
        {
            return cache.Token;
        }

        await _lock.WaitAsync();     // защищаем от одновременных запросов
        try
        {
            // Если другой поток уже обновил токен — просто возвращаем
            if (cache.Token != null && cache.ExpiresAt > DateTimeOffset.UtcNow)
            {
                return cache.Token;
            }

            // Иначе — выполняем получение
            (string token, DateTimeOffset expiresAt) = await RequestNewTokenAsync();
            cache.Token = token;
            cache.ExpiresAt = expiresAt;
            return token;
        }
        finally
        {
            _ = _lock.Release();
        }
    }

    public void Reset()
    {
        cache.Token = null;
        cache.ExpiresAt = null;
    }

    /// <summary>
    /// Здесь выполняется запрос в API, авторизация, refresh-flow и т.п.
    /// </summary>
    private async Task<(string token, DateTimeOffset expiresAt)> RequestNewTokenAsync()
    {
        await Task.Delay(1);
        return ("123456789", DateTimeOffset.UtcNow);
    }

    //private class AuthResponse
    //{
    //    public string AccessToken { get; set; } = null!;
    //    public int ExpiresIn { get; set; }
    //}
}
