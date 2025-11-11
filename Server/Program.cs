using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using Serilog.Events;
using Server.GameDataCache;
using Server.Http_NS.Controllers_NS.Users_NS;
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
internal partial class Program
{
    /// <summary>
    /// РЕЖИМ РАЗРАБОТКИ
    /// </summary>
    public static bool DEV_MODE { get; } = true;

    private static void AlertIfDevMode()
    {
        if (DEV_MODE)
        {
            Console.WriteLine("""
                DEV_MODE DEV_MODE DEV_MODE DEV_MODE DEV_MODE
                DEV_MODE DEV_MODE DEV_MODE DEV_MODE DEV_MODE
                DEV_MODE DEV_MODE DEV_MODE DEV_MODE DEV_MODE
                DEV_MODE DEV_MODE DEV_MODE DEV_MODE DEV_MODE
                """);
        }
    }

    /// <summary>
    /// Точка входа в приложение. Выполняет настройку DI, БД, аутентификации,
    /// регистрацию сервисов и запускает сервер.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    private static void Main(string[] args)
    {
        //Utilities.ConsoleWindow.Restore();
        //if (!General.ServerErrors.CheckEnumServerResponse())
        //{
        //    Console.WriteLine("Bad enum ServerResponse");
        //    _ = Console.ReadLine();
        //    return;
        //}

        AlertIfDevMode();

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new NullableSerializer<Guid>(new GuidSerializer(GuidRepresentation.Standard)));




        string serilogDir = Path.Combine(AppContext.BaseDirectory, "logs-errors");
        _ = Directory.CreateDirectory(serilogDir);

        Log.Logger = new LoggerConfiguration()
            // Все ошибки и критические события — в файл ошибок
            .WriteTo.File(
                Path.Combine(serilogDir, "errors-.txt"),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024, // 10 МБ
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 365,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error) // Только Error

            // В консоль — всё
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)

            .CreateLogger();

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        //_ = builder.Host.UseSerilog(); // Это заменит встроенный провайдер на Serilog
        _ = builder.Host.UseSerilog((context, services, configuration) => configuration
            .WriteTo.File(
                Path.Combine(serilogDir, "errors-.txt"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .ReadFrom.Configuration(context.Configuration));



        // Инициализация параметров для AuthOptions при старте приложения
        //Jwt.Initialize(builder.Configuration);

        IServiceCollection services = builder.Services;

        // Добавление контроллеров
        _ = services.AddControllers();
        _ = services.AddHttpLogging();

        // Регистрация ClientManager как singleton
        //_ = services.AddSingleton<ClientManager2>();




        // Добавление аутентификации с использованием JWT
        _ = services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt")); // Jwt.Issuer, Jwt.Audience, Jwt.Lifetime из конфигурации

        _ = services.AddSingleton<JwtService>();
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        _ = services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IConfiguration, JwtService>((opts, cfg, jwtSvc) =>
            {
                IConfigurationSection jwtSection = cfg.GetSection("Jwt");
                string? issuer = jwtSection["Issuer"];
                string? audience = jwtSection["Audience"];
                string secret = jwtSvc.GetJwtSecret(); // <-- экземплярный метод

                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
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
        _ = services.Configure<MongoHeroesSettings>(options =>
        {
            options.ConnectionString = "mongodb://localhost:27017";
            options.DatabaseName = "userData";
            options.CollectionName = "heroes";
        });


        // Регистрация репозитория
        _ = services.AddSingleton<MongoHeroesRepository>();

        _ = services.AddSingleton<WebSocketConnectionHandler>();
        _ = services.AddHostedService(provider => provider.GetRequiredService<WebSocketConnectionHandler>());


        // Ограничение размера тела
        _ = services.Configure<FormOptions>(options =>
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
        });


        // Регистрируем сервис как синглтон
        _ = services.AddSingleton<BackgroundLoggerAuthentificationService>();

        // Регистрируем его как IHostedService, используя тот же экземпляр
        _ = services.AddHostedService(
            provider => provider.GetRequiredService<BackgroundLoggerAuthentificationService>());

        // Добавляем HeroCacheService
        //_ = services.AddScoped<IHeroCacheService, HeroesCacheService>();
        _ = services.AddSingleton<IHeroCacheService, HeroesCacheService>();

        _ = services.AddMemoryCache();




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


        // Инициализация кэша до старта
        using (IServiceScope scope = app.Services.CreateScope())
        {
            IHeroCacheService heroCache = scope.ServiceProvider.GetRequiredService<IHeroCacheService>();
            heroCache.InitializeAsync(scope.ServiceProvider).GetAwaiter().GetResult();
        }


        // СТАРТ
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
