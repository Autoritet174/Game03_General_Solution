using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Server.GameDataCache;
using Server.Http_NS.Controllers_NS.Users;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using Server.WebSocket_NS;
using Server_DB_Data;
using Server_DB_Data.Repositories;
using Server_DB_UserData;
using Server_DB_Users;
using Server_DB_Users.Repositories;
using System.Text;
using System.Threading.RateLimiting;

namespace Server;

/// <summary>
/// Класс содержит точку входа приложения и настройки сервисов, аутентификации,
/// middleware, маршрутов и баз данных.
/// </summary>
internal class Program
{

    /// <summary>
    /// Точка входа в приложение. Выполняет настройку DI, БД, аутентификации,
    /// регистрацию сервисов и запускает сервер.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    private static void Main(string[] args)
    {
        //Utilities.ConsoleWindow.Restore();
        if (!General.ServerErrors.CheckEnumServerResponse())
        {
            Console.WriteLine("Bad enum ServerResponse");
            _ = Console.ReadLine();
            return;
        }

        string serilogDir = Path.Combine(AppContext.BaseDirectory, "logs-errors");
        Directory.CreateDirectory(serilogDir);

        Log.Logger = new LoggerConfiguration()
            // Все ошибки и критические события — в файл ошибок
            .WriteTo.File(
                Path.Combine(serilogDir, "errors-.txt"),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10 МБ
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 365,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error) // Только Error

            // В консоль — всё, что угодно (можно ограничить)
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)

            .CreateLogger();
        
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        _ = builder.Host.UseSerilog(); // Это заменит встроенный провайдер на Serilog

        // Инициализация параметров для AuthOptions при старте приложения
        //Jwt.Initialize(builder.Configuration);

        IServiceCollection services = builder.Services;

        // Добавление контроллеров
        _ = services.AddControllers();
        _ = services.AddHttpLogging();

        // Регистрация ClientManager как singleton
        _ = services.AddSingleton<ClientManager2>();




        // Добавление аутентификации с использованием JWT
        _ = builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt")); // Jwt.Issuer, Jwt.Audience, Jwt.Lifetime из конфигурации

        _ = builder.Services.AddSingleton<JwtService>();

        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            IConfigurationSection jwtConfig = builder.Configuration.GetSection("Jwt");
            string jwtConfig_key = JwtService.GetJwtSecret();
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
                _ = policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });


        // База данных пользователей
        _ = services.AddDbContext<DbContext_Game03Users>(options => options.UseNpgsql(DbContext_Game03Users.GetConnectionString()));
        _ = services.AddScoped<UserRepository>();


        // База данных с игровыми данными
        _ = services.AddDbContext<DbContext_Game03Data>(options => options.UseNpgsql(DbContext_Game03Data.GetConnectionString()));
        _ = services.AddScoped<HeroRepository>();

        // Конфигурация MongoDB
        _ = services.Configure<MongoSettings>(options =>
        {
            options.ConnectionString = "mongodb://localhost:27017";
            options.DatabaseName = "userData";
            options.CollectionName = "items";
        });

        // Регистрация репозитория
        _ = builder.Services.AddSingleton<MongoRepository>();

        _ = builder.Services.AddSingleton<WebSocketConnectionHandler>();
        _ = builder.Services.AddHostedService(provider => provider.GetRequiredService<WebSocketConnectionHandler>());


        // Ограничение размера тела
        _ = builder.Services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = 1_048_576;
            options.MultipartBodyLengthLimit = 1_048_576;
        });


        // --- Добавляем Rate Limiting с учётом IP ---
        _ = services.AddRateLimiter(options =>
        {
            _ = options.AddPolicy("login", context =>
            {
                // Получаем IP-адрес клиента
                string? ipAddress = context.Connection.RemoteIpAddress?.ToString();

                // Если не удалось определить (например, в тестах) — используем "unknown"
                string clientKey = ipAddress ?? "unknown";

                // Создаём "токен бакет" на основе IP
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientKey,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        PermitLimit = 5,
                        QueueLimit = 0
                    });
            });

            // Опционально: глобальный лимит, если хочешь
            // options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(...);
        });

        _ = services.AddHostedService<BackgroundLoggerAuthentificationService>();

        WebApplication app = builder.Build();

        //Миддлвар 1 - Обработка ошибок
        _ = app.UseMiddleware<ExceptionLoggingMiddleware>();
        //_ = app.UseExceptionHandler("/Home/Error");// этот мидлвар не нужен так как сервер обслуживает только API, без сайта и вебстраниц

        //Миддлвар 2 - Логирование
        //_ = app.UseHttpLogging();

        //Миддлвар 3 - Статические файлы
        //_ = app.UseStaticFiles();

        _ = app.UseRateLimiter();

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


        _ = app.UseForwardedHeaders();
        ListAllHeroes.Init();
        try
        {
            Log.Information("✅ Приложение стартует. Serilog работает.");
            Log.Error("🧪 Это тестовая ошибка — должна попасть в файл.");
        }
        catch
        {
            // На всякий случай — если Log нерабочий
            Console.WriteLine("❌ Log.Error не сработал");
        }
        app.Run();

    }

    /// <summary>
    /// Тестовая функция для проверки взаимодействия с базой данных.
    /// Может быть временной или отладочной.
    /// </summary>
    /// <param name="app">Экземпляр <see cref="WebApplication"/>.</param>
    /// <returns>Асинхронная задача без значения.</returns>
    private static async Task Test(WebApplication app)
    {
        await Task.Delay(0);
        //using DbData db = new();
        //var hero = db.Heroes.First(a=>a.Name == "Warrior");
        //var ct = db.CreatureTypes.First(a => a.Name == "Humanoid");
        //hero.CreatureTypes.Add(ct);
        //db.SaveChanges();
    }
}
