using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Npgsql;
using Serilog;
using Serilog.Events;
using Server.Cache;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using Server.Users;
using Server.Users.Authentication;
using Server.Users.Registration;
using Server.WebSocket_NS;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Repositories;
using System.IO.Compression;
using System.Net;
using System.Threading.RateLimiting;

namespace Server;

/// <summary> Класс содержит точку входа приложения и настройки сервисов, аутентификации, middleware, маршрутов и баз данных. </summary>
internal partial class Program
{

    /// <summary> Точка входа в приложение. Выполняет настройку DI, БД, аутентификации, регистрацию сервисов и запускает сервер. </summary>
    private static async Task Main(string[] args)
    {
        _ = ThreadPool.SetMinThreads(200, 200);
        //MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
        //MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.NullableSerializer<Guid>(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)));

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        InitSerilog(builder);

        // Инициализация параметров для AuthOptions при старте приложения
        //Jwt.Initialize(builder.Configuration);

        IMvcBuilder iMvcBuilder = services.AddControllers();// Добавление контроллеров
        _ = services.AddHttpLogging();

        string connectionString = CreateConnectionString(builder);

        // База данных с игровыми данными
        //DbContext_Game.Init(connectionString);// Инициализация NpgsqlDataSource с поддержкой Newtonsoft.Json

        //источник данных Npgsql с поддержкой JSON
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        JsonSerializerSettings jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore // Это исключит null поля из JSON
        };
        _ = dataSourceBuilder.UseJsonNet(jsonSettings);
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        _ = services.AddDbContext<DbContextGame>(options => options.UseNpgsql(dataSource));

        //_ = services.AddScoped(sp => new DbContext_Game(DbContext_Game.DbContextOptions));


        // РЕПОЗИТОРИИ
        _ = services.AddScoped<CollectionHeroRepository>();

        _ = services.AddSingleton<CacheService>();
        _ = services.AddSingleton<ITestService, TestService>();


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

        InitRateLimite(services);


        // Регистрируем сервис как синглтон
        _ = services.AddSingleton<AuthRegLoggerBackgroundService>();

        // Регистрируем его как IHostedService, используя тот же экземпляр
        _ = services.AddHostedService(provider => provider.GetRequiredService<AuthRegLoggerBackgroundService>());

        // Добавляем HeroCacheService
        //_ = services.AddScoped<IHeroCacheService, HeroesCacheService>();

        _ = services.AddMemoryCache();

        _ = services.AddScoped<AuthService>();
        _ = services.AddScoped<RegService>();
        _ = services.AddScoped<SessionService>();




        // установить Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
        // если надо. сейчас нет кубернетес или сервисов проверки работоспособности
        //_ = services.AddHealthChecks()
        //    .AddDbContextCheck<DbContext_Game>(
        //    name: "database-check",
        //    tags: new[] { "ready" });

        InitNewtonsoftJson(iMvcBuilder);
        InitCompressionResponse(services);

        //_ = services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        //{
        //    options.User.RequireUniqueEmail = true;

        //    options.Lockout.MaxFailedAccessAttempts = 5;
        //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

        //    options.SignIn.RequireConfirmedAccount = false;
        //})
        //.AddEntityFrameworkStores<DbContext_Game>()
        //.AddDefaultTokenProviders();

        _ = services.AddIdentityCore<User>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddSignInManager<SignInManager<User>>()  // Добавьте эту строку для регистрации SignInManager
        .AddRoles<IdentityRole<Guid>>()  // Если используете роли; иначе удалите
        .AddEntityFrameworkStores<DbContextGame>()
        .AddDefaultTokenProviders();

        _ = services.AddFido2(options =>
        {
            options.ServerDomain = "your-game-domain.com";
            options.ServerName = "Your Game Name";
            options.Origins = new HashSet<string>
            {
                "https://your-game-domain.com"
            };
        });



        _ = services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });
        InitJwt(builder);

        _ = builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxConcurrentConnections = null; // unlimited
            options.Limits.MaxConcurrentUpgradedConnections = null; // unlimited для WebSockets
        });

        WebApplication app = builder.Build();

        _ = app.UseForwardedHeaders();//Forwarded headers (первым!)
        _ = app.UseHsts();//HSTS
        _ = app.UseMiddleware<ExceptionLoggingMiddleware>();//Обработка исключений
        _ = app.UseResponseCompression();//Сжатие ответов (ДО маршрутизации)

        //_ = app.UseExceptionHandler("/Home/Error");// этот мидлвар не нужен так как сервер обслуживает только API, без сайта и вебстраниц

        //Миддлвар 2 - Логирование
        //_ = app.UseHttpLogging();

        //Миддлвар 3 - Статические файлы
        //_ = app.UseStaticFiles();

        //_ = app.UseHttpsRedirection();

        // Разрешение WebSocket соединений
        _ = app.UseWebSockets();//WebSockets (ДО маршрутизации)

        InitWebSocket(app);

        // Подключение кастомного WebSocket middleware
        //_ = app.UseMiddleware<WebSocketMiddleware>();

        _ = app.UseRouting();//Маршрутизация

        _ = app.UseRateLimiter();//Rate limiting (ПОСЛЕ маршрутизации)

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.UseMiddleware<SecurityHeadersMiddleware>();// заголовки безопасности (можно после аутентификации)

        //_ = app.UseCors("AllowAll");//это нужно только для браузеров, то есть на этом сервере это не нужно

        _ = app.UseResponseCaching();// Ответы с кешированием (почти в конце)

        _ = app.MapControllers();// Маршрутизация контроллеров (после всех Use())

        await ConnectionsIsCorrect(app);

        // на этом момент есть гарантия что соединения со всеми СУБД корректно.
        // иначе где то сработает один из throw.

        await LoadServerCache(app);

        IHostApplicationLifetime lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

        _ = lifetime.ApplicationStopping.Register(() =>
        {
            Log.Information("Application is stopping...");
            // Очистка ресурсов
        });

        _ = lifetime.ApplicationStopped.Register(() =>
        {
            Log.Information("Application stopped.");
            Log.CloseAndFlush();
        });

        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }


        /*
         * позже сделать, сейчас не надо
         // пакет: Prometheus.AspNetCore
        _ = services.AddMetrics();
        _ = app.UseMetricServer(); // endpoint /metrics для Prometheus
         */
    }

    /// <summary> Использование Serilog вместо обычного microsoft лога. </summary>
    private static void InitSerilog(WebApplicationBuilder builder)
    {
        string serilogDir = Path.Combine(AppContext.BaseDirectory, "logs-errors");
        _ = Directory.CreateDirectory(serilogDir);

        // Настраиваем Serilog через UseSerilog
        _ = builder.Host.UseSerilog((context, services, configuration) => configuration
            .Enrich.FromLogContext()

            // Файл для ошибок
            .WriteTo.File(
                Path.Combine(serilogDir, "errors-.txt"),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 365,
                restrictedToMinimumLevel: LogEventLevel.Error,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")

            // Файл для всех логов
            .WriteTo.File(
                Path.Combine(serilogDir, "all-.txt"),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 20 * 1024 * 1024, // 20 МБ
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 30, // Храним 30 дней
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")

            // Консоль
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .ReadFrom.Configuration(context.Configuration));
    }

    /// <summary> Добавление аутентификации с использованием JWT. </summary>
    private static void InitJwt(WebApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;
        _ = services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        _ = services.AddSingleton<JwtService>();
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { }); // Пустая конфигурация, чтобы зарегистрировать схему

        // Добавляем пост-конфигурацию с использованием DI
        _ = services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(sp =>
            new PostConfigureOptions<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    JwtOptions jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;
                    JwtService jwtService = sp.GetRequiredService<JwtService>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = JwtService.ClockSkew,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtService.IssuerSigningKey
                    };

                    //// Логируем ошибки валидации для отладки
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        Console.WriteLine("JWT валидация провалилась");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnTokenValidated = context => {
                    //        Console.WriteLine("OnTokenValidated");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnMessageReceived = context => {
                    //        Console.WriteLine("OnMessageReceived");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnChallenge = context => {
                    //        Console.WriteLine("OnChallenge");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnForbidden = context => {
                    //        Console.WriteLine("OnForbidden");
                    //        return Task.CompletedTask;
                    //    }
                    //};
                }));
    }

    /// <summary> Получение строки подключения из настройек и дополнительные корректировки. </summary>
    private static string CreateConnectionString(WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Строка подключения 'Postgres' не найдена.");

        // параметры пулинга, если их нет в строке подключения
        if (!connectionString.Contains("Pooling="))
        {
            connectionString += ";"
                + "Pooling=true;" // Пуллинг для оптимизации работы с postgres
                + "Minimum Pool Size=5;"      // Минимум 5 соединений
                + "Maximum Pool Size=100;"    // Максимум 100 соединений  
                + "Connection Idle Lifetime=300;" // Через 300 сек неиспользуемое соединение закрывается
                + "Connection Pruning Interval=10;" // Проверка каждые 10 секунд
                + "Timeout=15"; // Таймаут ожидания свободного соединения (сек)
        }
        return connectionString;
    }

    /// <summary> Добавляем Rate Limiting с учётом IP. </summary>
    private static void InitRateLimite(IServiceCollection services)
    {
        _ = 0;
        _ = services.AddRateLimiter(static options =>
        {
            _ = options.AddPolicy(Consts.RATE_LIMITER_POLICY_AUTH, static context =>
            {
                string? ipAddress = context.Connection.RemoteIpAddress?.ToString();

                // Если не удалось определить (например, в тестах) — используем "unknown"
                string clientKey = string.IsNullOrWhiteSpace(ipAddress) ? "unknown" : ipAddress;

                // Создаём "токен бакет" на основе IP
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientKey,
                    factory: static _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        PermitLimit = 5,// Максимум 5 попыток в минуту на IP
                        QueueLimit = 0
                    });
            });

            _ = options.AddPolicy(Consts.RATE_LIMITER_POLICY_COLLECTION, static context =>
            {
                string? ipAddress = context.Connection.RemoteIpAddress?.ToString();
                string clientKey = string.IsNullOrWhiteSpace(ipAddress) ? "unknown" : ipAddress;
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientKey,
                    factory: static _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(10),
                        PermitLimit = 1,
                        QueueLimit = 0
                    });
            });
        });
    }

    private static void InitNewtonsoftJson(IMvcBuilder iMvcBuilder)
    {
        _ = 0;
        _ = iMvcBuilder.AddNewtonsoftJson(static options =>
        {
            // 1. Обработка ссылок
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Обработка циклических ссылок
            options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None; // Сохранение ссылок на объекты

            //// 2. Null значения
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;

            //// 3. Именование (camelCase для JSON)
            //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //// 4. Даты в UTC ISO формате
            //options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";

            //// 5. Конвертеры
            //options.SerializerSettings.Converters.Add(new StringEnumConverter());

            //// 6. Форматирование
            //options.SerializerSettings.Formatting = Formatting.Indented;

            //// 8. Дополнительные настройки
            //options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            //options.SerializerSettings.MaxDepth = 32;
        });
    }

    /// <summary> Подключение WebSocket-обработчика через маршрутизацию ASP.NET Core. Запросы по адресу /ws будут направляться в ProcessKestrelWebSocketRequest. </summary>
    private static void InitWebSocket(WebApplication app)
    {
        _ = 0;
        // Маршрут для WebSocket соединений
        _ = app.Map("/ws", static appBuilder =>
        {
            appBuilder.Run(static async context =>
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
                    try
                    {
                        // Получаем Singleton-экземпляр обработчика из DI
                        WebSocketConnectionHandler handler = context.RequestServices.GetRequiredService<WebSocketConnectionHandler>();

                        // Запускаем обработку WebSocket
                        // Используем CancellationToken из контекста приложения
                        await handler.ProcessKestrelWebSocketRequest(context, context.RequestAborted);
                    }
                    catch (OperationCanceledException)
                    {
                        // Корректное завершение при отмене
                        Log.Information("WebSocket соединение было отменено");
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Запрос должен быть WebSocket-запросом.");
                }
            });
        });
    }

    /// <summary>
    /// Выполняет проверку соединения с базой данных.
    /// </summary>
    /// <param name="app">Экземпляр работающего WebApplication.</param>
    /// <exception cref="InvalidOperationException">Бросается, если соединение не установлено.</exception>
    private static async Task ConnectionsIsCorrect(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        // Использование целевого типа для логгера
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();
            using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

            // Простая проверка активности соединения
            _ = await db.Database.CanConnectAsync(cts.Token)
                ? true
                : throw new InvalidOperationException("Database is unreachable");

            logger.LogInformation("SERVER=postgres, DB=Data, connection is correct");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Failed to connect to database during startup");
            throw;
        }
    }

    /// <summary> Загружаем в оперативную память константные серверные данные которые не меняются во время работы сервера. </summary>
    private static async Task LoadServerCache(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        CacheService service = scope.ServiceProvider.GetRequiredService<CacheService>();
        using DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await service.LoadServerDataAsync(db, cts.Token);

            logger.LogInformation("Server cache loaded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Server cache load failed");
            throw;
        }
    }

    private static readonly string[] AdditionalMimeTypesForCompression = ["application/json", "application/xml", "text/plain", "text/html", "text/css", "text/javascript", "application/javascript", "image/svg+xml"];
    private static void InitCompressionResponse(IServiceCollection services)
    {
        // Добавление сервисов сжатия ответов
        _ = services.AddResponseCompression(static options =>
        {
            options.EnableForHttps = true; // Включить сжатие для HTTPS
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();

            // Настройка MIME-типов для сжатия
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(AdditionalMimeTypesForCompression);
        });

        // настройка провайдеров сжатия
        _ = services.Configure<BrotliCompressionProviderOptions>(static options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        _ = services.Configure<GzipCompressionProviderOptions>(static options =>
        {
            options.Level = CompressionLevel.Fastest;
        });
    }
}
