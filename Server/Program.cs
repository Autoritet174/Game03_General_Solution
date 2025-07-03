using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.DB.Users;
using Server.DB.Users.Entities;
using Server.DB.Users.Repositories;
using Server.Http_NS.Controllers_NS.Game;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using System.Text;
using System.Threading.Tasks;

namespace Server;
internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Инициализация параметров для AuthOptions при старте приложения
        Jwt.Initialize(builder.Configuration);

        IServiceCollection services = builder.Services;

        // Добавление контроллеров
        _ = services.AddControllers();
        _ = services.AddHttpLogging();

        // Регистрация ClientManager как singleton
        _ = services.AddSingleton<ClientManager>();

        // Добавление аутентификации с использованием JWT
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            IConfigurationSection jwtConfig = builder.Configuration.GetSection("Jwt");
            string jwtConfig_key = jwtConfig["Key"] ?? throw new ArgumentNullException(jwtConfig["Key"]);
            options.TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuer = true,// Проверять издателя
                ValidIssuer = jwtConfig["Issuer"],

                ValidateAudience = true,// Проверять аудиторию
                ValidAudience = jwtConfig["Audience"],

                ValidateLifetime = true,// Проверять срок действия

                ValidateIssuerSigningKey = true,// Проверять подпись

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig_key))
            };

            // Добавляем обработчик событий
            //options.Events = new JwtBearerEvents {
            //    OnAuthenticationFailed = ctx =>
            //    {
            //        var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ctx.Exception, "JWT validation failed");
            //        File.WriteAllText("__log.txt", ctx.Exception.ToString());
            //        return Task.CompletedTask;
            //    },
            //    // Другие события можно добавить по необходимости
            //    OnTokenValidated = ctx =>
            //    {
            //        Console.WriteLine("Токен успешно валидирован!");
            //        return Task.CompletedTask;
            //    }
            //};
        });

        _ = services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                _ = policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
            });
        });

        string connectionString_DbUsers = builder.Configuration.GetConnectionString("DB.Users") ?? throw new Exception("connectionString_DbUsers is empty");
        _ = services.AddDbContext<DB_Users>(options => options.UseNpgsql(connectionString_DbUsers));

        services.AddScoped<UserRepository>();

        services.AddScoped<DB.Users.Tests.TestUsers>();

        // Добавляем контекст БД (SQL Server)
        //services.AddDbContext<DbContextEf>(options =>
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Добавляем контекст БД (MySql)
        //string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //if (connectionString == null) {
        //    Console.WriteLine("connectionString = null");
        //    return;
        //}
        //Microsoft.EntityFrameworkCore.ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
        //services.AddDbContext<DbContextEf>(options =>
        //    options.UseMySql(connectionString, serverVersion));

        //services.AddIdentity<IdentityUser, IdentityRole>();


        WebApplication app = builder.Build();

        Configure(app);



        //HeroesController.Init();
        //Test(app);

        app.Run();

    }


    public static void Configure(WebApplication app)
    {

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
        //_ = app.UseMiddleware<WebSocketMiddleware>();

        // Маршрутизация
        _ = app.UseRouting();

        _ = app.UseCors("AllowAll");

        // Подключение аутентификации и авторизации
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();


        // Ответы с кешированием (если требуется)
        //app.UseResponseCaching();


        // CORS
        //app.UseCors("AllowSpecificOrigins");

        // Лог запросов в консоль
        //app.Use(async (ctx, next) =>
        //{
        //    Console.WriteLine($"Запрос: {ctx.Request.Path}");
        //    await next();
        //});


        // Маршрутизация контроллеров
        _ = app.MapControllers();

    }

    private static async Task Test(WebApplication app)
    {
        // 👉 Ваш вызов вручную после запуска контейнера
        using (var scope = app.Services.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<UserRepository>();

            for (int i = 0; i < 100; i++)
            {
                await userService.AddAsync(new User
                {
                    Email = "john.doe@example.com_" + Guid.NewGuid().ToString(),
                    PasswordHash = "secret"
                });
            }
        }
    }
}