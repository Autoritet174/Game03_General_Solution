using General.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using System;  // Для ValueTuple
using Server_DB_Postgres;  // Убедитесь, что namespace совпадает с DbContext_Game

namespace Server_DB_Postgres;

public class DbContext_GameFactory : IDesignTimeDbContextFactory<DbContext_Game>
{
    public DbContext_Game CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext_Game>();

        // Ваша строка подключения
        var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost;Port=5432;Database=Game;Username=postgres;Password=yourpassword");
        dataSourceBuilder.MapComposite<ValueTuple<int, int, int?>>("server.dice");  // Маппинг для ValueTuple
        var dataSource = dataSourceBuilder.Build();

        optionsBuilder.UseNpgsql(dataSource);

        return new DbContext_Game(optionsBuilder.Options);
    }
}
