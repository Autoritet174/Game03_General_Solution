using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Server.GameDataCache;
using Server.Http_NS.Controllers_NS.Users_NS;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using Server.WebSocket_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Repositories;
using System.Net; // Добавлено для HttpStatusCode
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
    /// Точка входа в приложение. Выполняет настройку DI, БД, аутентификации,
    /// регистрацию сервисов и запускает сервер.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    private static async Task Main(string[] args)
    {
        //MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
        //MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.NullableSerializer<Guid>(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)));


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

        //_ = services.AddCors(options =>
        //{
        //    options.AddPolicy("AllowAll", policy =>
        //    {
        //        _ = policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        //    });
        //});



        // -- БАЗЫ ДАННЫХ --

        // База данных пользователей
        //string сonnectionStringUsers = builder.Configuration.GetConnectionString("Postgres_Users")
        //    ?? throw new InvalidOperationException("Строка подключения 'Postgres_Users' не найдена.");
        //_ = services.AddDbContext<DbContext_Game03Users>(options => options.UseNpgsql(сonnectionStringUsers));
        //_ = services.AddScoped<UserRepository>();


        // База данных с игровыми данными
        string сonnectionString = builder.Configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Строка подключения 'Postgres' не найдена.");
        _ = services.AddDbContext<DbContext_Game>(options => options.UseNpgsql(сonnectionString));
        _ = services.AddScoped<HeroRepository>();


        //// Настройки "MongoDb"
        //IConfigurationSection mongoSection = builder.Configuration.GetSection("MongoDb");
        //_ = builder.Services.Configure<MongoDbSettings>(mongoSection);
        //// Регистрация репозитория
        //_ = services.AddSingleton<MongoRepository>();


        _ = services.AddSingleton<WebSocketConnectionHandler>();


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

        _ = services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.PropertyNamingPolicy = null;
               options.JsonSerializerOptions.DictionaryKeyPolicy = null;
           });


        WebApplication app = builder.Build();


        //Миддлвар 1 - Обработка ошибок
        _ = app.UseMiddleware<ExceptionLoggingMiddleware>();
        //_ = app.UseExceptionHandler("/Home/Error");// этот мидлвар не нужен так как сервер обслуживает только API, без сайта и вебстраниц

        //Миддлвар 2 - Логирование
        //_ = app.UseHttpLogging();

        //Миддлвар 3 - Статические файлы
        //_ = app.UseStaticFiles();

        _ = app.UseRateLimiter();

        //_ = app.UseHttpsRedirection();
        _ = app.UseHsts();

        // Добавляем заголовки безопасности
        _ = app.UseMiddleware<SecurityHeadersMiddleware>();

        // Разрешение WebSocket соединений
        _ = app.UseWebSockets();

        // НОВОЕ: Подключение WebSocket-обработчика через маршрутизацию ASP.NET Core.
        // Запросы по адресу /ws будут направляться в ProcessKestrelWebSocketRequest
        _ = app.Map("/ws", appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                // НОВОЕ: Проверка, что это GET-запрос. 
                // WebSocket-хендшейк всегда использует метод GET.
                if (context.Request.Method != "GET")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed; // 405 Method Not Allowed
                    await context.Response.WriteAsync("Для WebSocket-хендшейка разрешен только метод GET.");
                    return;
                }
                if (context.WebSockets.IsWebSocketRequest)
                {
                    // Получаем Singleton-экземпляр обработчика из DI
                    WebSocketConnectionHandler handler = context.RequestServices.GetRequiredService<WebSocketConnectionHandler>();

                    // Запускаем обработку WebSocket
                    // Используем CancellationToken из контекста приложения
                    await handler.ProcessKestrelWebSocketRequest(context, context.RequestAborted);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Запрос должен быть WebSocket-запросом.");
                }
            });
        });


        // Подключение кастомного WebSocket middleware
        //_ = app.UseMiddleware<WebSocketMiddleware>();

        // Маршрутизация
        _ = app.UseRouting();

        //_ = app.UseCors("AllowAll");//это нужно только для браузеров, то есть на этом сервере это не нужно

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

        using (IServiceScope scope = app.Services.CreateScope())
        {
            Serilog.ILogger logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

            //await DbContext_Game03Users.ThrowIfFailureConnection(сonnectionStringUsers);
            //logger.Information("SERVER=postgres, DB=Users, connection is correct");

            await DbContext_Game.ThrowIfFailureConnection(сonnectionString);
            logger.Information("SERVER=postgres, DB=Data, connection is correct");

            //IOptions<MongoDbSettings> mongoSettings = scope.ServiceProvider.GetRequiredService<IOptions<MongoDbSettings>>();
            //await MongoRepository.ThrowIfFailureConnectionAsync(mongoSettings);
            //logger.Information("SERVER=MongoDb, DB=UserData, connection is correct");
        }

        // на этом момент есть гарантия что соединения со всеми СУБД корректно.

        // Инициализация кэша до старта
        using (IServiceScope scope = app.Services.CreateScope())
        {
            IHeroCacheService heroCache = scope.ServiceProvider.GetRequiredService<IHeroCacheService>();
            heroCache.InitializeAsync(scope.ServiceProvider).GetAwaiter().GetResult();
        }
       

        // СТАРТ
        app.Run();
    }
}
