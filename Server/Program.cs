using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Http.Middleware;
using Server.Jwt_NS;
using Server.WebSocket_NS;
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