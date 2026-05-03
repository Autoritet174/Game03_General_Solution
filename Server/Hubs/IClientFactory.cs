using Microsoft.EntityFrameworkCore;
using Server.Cache;
using Server_DB_Postgres;

namespace Server.Hubs;

public interface IClientFactory
{
    Client Create(Guid userId);
}

public class ClientFactory(IServiceProvider serviceProvider) : IClientFactory
{
    public Client Create(Guid userId)
    {
        // Получаем зависимости вручную, так как userId не из DI
        ILogger<Client> logger = serviceProvider.GetRequiredService<ILogger<Client>>();
        IDbContextFactory<DbContextGame> dbFactory = serviceProvider.GetRequiredService<IDbContextFactory<DbContextGame>>();
        CacheService cache = serviceProvider.GetRequiredService<CacheService>();

        return new Client(userId, logger, dbFactory, cache);
    }
}
