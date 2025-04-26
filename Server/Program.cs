using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.UserAuth_NS;
using Server.WebSocket_NS;
using System.Text;

namespace Server;
internal class Program {
    private static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Инициализация параметров для AuthOptions при старте приложения
        AuthOptions.Initialize(builder.Configuration);

        // Добавление контроллеров
        _ = builder.Services.AddControllers();

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

        WebApplication app = builder.Build();

        // Разрешение WebSocket соединений
        _ = app.UseWebSockets();

        // Подключение кастомного WebSocket middleware
        _ = app.UseMiddleware<WebSocketMiddleware>();

        // Подключение аутентификации и авторизации
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        // Маршрутизация контроллеров
        _ = app.MapControllers();

        app.Run();
    }
}