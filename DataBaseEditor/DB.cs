using General;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server_DB_Postgres;

namespace DataBaseEditor;

public static class DB
{
    private const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=Game;Username=postgres;Password=";

    public static DbContextGame Create()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(CONNECTION_STRING);
        _ = dataSourceBuilder.EnableDynamicJson();
        _ = dataSourceBuilder.ConfigureJsonOptions(JSON.Options);
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        DbContextOptionsBuilder<DbContextGame> optionsBuilder = new();
        return new DbContextGame(optionsBuilder.UseNpgsql(dataSource).Options);
    }
}
