using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.InternetChecker;

internal class InternetCheckerProvider : IInternetCheckerProvider
{
    /// <summary>
    /// Асинхронно проверяет наличие интернет-соединения, пингуя известные DNS-сервера.
    /// </summary>
    /// <remarks>
    /// "8.8.8.8" - IPv4 адрес Google DNS.
    /// "2606:4700::64" - IPv6 адрес Cloudflare DNS.
    /// </remarks>
    /// <returns>true, если соединение успешно установлено, иначе false.</returns>
    public async Task<bool> CheckInternetConnectionAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }
        List<Task<PingReply?>> tasks =
        [
            SendPingAsync("8.8.8.8"),   // IPv4 адрес Google DNS
            SendPingAsync("2606:4700::64") // IPv6 адрес Cloudflare DNS
        ];

        PingReply?[] results = await Task.WhenAll(tasks); // ждем завершения всех запросов

        foreach (PingReply? result in results)
        {
            if (result?.Status == IPStatus.Success)
            {
                return true; // если хотя бы одна проверка прошла успешно
            }
        }

        return false; // если обе проверки завершились неудачно
    }

    private async Task<PingReply?> SendPingAsync(string host)
    {
        using Ping ping = new();
        try
        {
            return await ping.SendPingAsync(host, 1000); // ожидание ответа до 1 секунды
        }
        catch //(Exception ex)
        {
            //Console.WriteLine($"Ошибка при попытке пинга {host}: {ex.Message}");
        }
        return null;
    }
}
