using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using Server.WebSocket_NS;
using System;
using System.Text;

namespace Server;
internal class Program {
    private static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Инициализация параметров для AuthOptions при старте приложения
        JwtCash.Initialize(builder.Configuration);

        // Добавление контроллеров
        _ = builder.Services.AddControllers();
        _ = builder.Services.AddHttpLogging();

        // Регистрация ClientManager как singleton
        _ = builder.Services.AddSingleton<ClientManager>();

        // Добавление аутентификации с использованием JWT
        _ = builder.Services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => {
            IConfigurationSection jwtConfig = builder.Configuration.GetSection("Jwt");
            string jwtConfig_key = jwtConfig["Key"] ?? throw new ArgumentNullException(jwtConfig["Key"]);
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig_key))
            };
        });


        // Добавляем контекст БД (SQL Server)
        //builder.Services.AddDbContext<DbContextEf>(options =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Добавляем контекст БД (MySql)
        //string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //if (connectionString == null) {
        //    Console.WriteLine("connectionString = null");
        //    return;
        //}
        //Microsoft.EntityFrameworkCore.ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
        //builder.Services.AddDbContext<DbContextEf>(options =>
        //    options.UseMySql(connectionString, serverVersion));
      
       


        WebApplication app = builder.Build();

        Configure(app);

        // Маршрутизация контроллеров
        _ = app.MapControllers();

        app.Run();
    }


    public static void Configure(WebApplication app) {

        //Миддлвар 1 - Обработка ошибок
        _ = app.UseExceptionHandler("/Home/Error");

        //Миддлвар 2 - Логирование
        _ = app.UseHttpLogging();

        //Миддлвар 3 - Статические файлы
        //_ = app.UseStaticFiles();


        _ = app.UseHttpsRedirection();
        _ = app.UseHsts();

        // Добавляем заголовки безопасности
        _ = app.UseMiddleware<SecurityHeadersMiddleware>();


        // Разрешение WebSocket соединений
        _ = app.UseWebSockets();

        // Подключение кастомного WebSocket middleware
        _ = app.UseMiddleware<WebSocketMiddleware>();

        // Подключение аутентификации и авторизации
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();


        // Ответы с кешированием (если требуется)
        //app.UseResponseCaching();

        // Маршрутизация
        //app.UseRouting();

        // CORS
        //app.UseCors("AllowSpecificOrigins");



    }
}