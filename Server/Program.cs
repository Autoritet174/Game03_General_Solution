
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Serilog;
using Serilog.Events;
using Server.Cache;
using Server.Game;
using Server.Http_NS.Middleware_NS;
using Server.Hubs;
using Server.Jwt_NS;
using Server.Users;
using Server.Users.Authentication;
using Server.Users.Registration;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.IO.Compression;
using System.Threading.RateLimiting;

namespace Server;

/*
export DB_HOST=your-db-host
export DB_PORT=5432
export DB_NAME=Game
export DB_USER=postgres
export DB_PASSWORD=SecretPassword123
dotnet Server.dll
 */


/// <summary> Класс содержит точку входа приложения и настройки сервисов, аутентификации, middleware, маршрутов и баз данных. </summary>
internal partial class Program
{
    private static readonly CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token;

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

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        _ = dataSourceBuilder.EnableDynamicJson();
        _ = dataSourceBuilder.ConfigureJsonOptions(JSON.Options);



        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        _ = services.AddDbContextFactory<DbContextGame>(options => options.UseNpgsql(dataSource));
        //_ = services.AddDbContext<DbContextGame>(options => options.UseNpgsql(dataSource));
        _ = services.AddDbContext<DbContextGame>(options =>
            options.UseNpgsql(dataSource),
            ServiceLifetime.Scoped,  // DbContext как Scoped
            ServiceLifetime.Singleton); // DbContextOptions как Singleton

        _ = services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, GlobalJsonContext.Default);
            });
        //_ = services.AddScoped(sp => new DbContext_Game(DbContext_Game.DbContextOptions));



        _ = services.AddSingleton<CacheService>();
        _ = services.AddSingleton<TestService>();
        _ = services.AddSingleton<ClientManager>();
        _ = services.AddSingleton<IClientFactory, ClientFactory>();

        //// Настройки "MongoDb"
        //IConfigurationSection mongoSection = builder.Configuration.GetSection("MongoDb");
        //_ = builder.Services.Configure<MongoDbSettings>(mongoSection);
        //// Регистрация репозитория
        //_ = services.AddSingleton<MongoRepository>();

        //_ = services.AddSingleton<WebSocketConnectionHandler>();

        // Ограничение размера тела
        _ = services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = 1_048_576;
            options.MultipartBodyLengthLimit = 1_048_576;
        });

        InitRateLimite(services);


        // Регистрируем сервис как синглтон
        _ = services.AddSingleton<AuthRegLoggerBackgroundService>();
        _ = services.AddSingleton<LootGenerator>();

        // Регистрируем его как IHostedService, используя тот же экземпляр
        _ = services.AddHostedService(provider => provider.GetRequiredService<AuthRegLoggerBackgroundService>());

        // Добавляем HeroCacheService
        //_ = services.AddScoped<IHeroCacheService, HeroesCacheService>();

        _ = services.AddMemoryCache();

        _ = services.AddScoped<AuthService>();
        _ = services.AddScoped<RegService>();
        _ = services.AddScoped<SessionService>();
        _ = services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = !builder.Environment.IsProduction();
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = JSON.Options;
            });

        // установить Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
        // если надо. сейчас нет кубернетес или сервисов проверки работоспособности
        //_ = services.AddHealthChecks()
        //    .AddDbContextCheck<DbContext_Game>(
        //    name: "database-check",
        //    tags: new[] { "ready" });

        //InitNewtonsoftJson(iMvcBuilder);
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
        .AddSignInManager<SignInManager<User>>()
        //.AddRoles<IdentityRole<Guid>>()  // Если используете роли; иначе удалите
        .AddEntityFrameworkStores<DbContextGame>()
        .AddDefaultTokenProviders();

        //_ = services.AddFido2(options =>
        //{
        //    options.ServerDomain = "your-game-domain.com";
        //    options.ServerName = "Your Game Name";
        //    options.Origins = new HashSet<string>
        //    {
        //        "https://your-game-domain.com"
        //    };
        //});

        string domain = builder.Configuration.GetValue<string>("Kestrel:Endpoints:Https:Url") ?? throw new Exception("Kestrel HTTPS URL is not configured");
        Url.Init(domain.TrimEnd('/'));


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
        _ = app.MapHub<GameHub>(Parametrs.SignalR_Address);
        //InitWebSocket(app);

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

        await ConnectionsIsCorrectAsync(app, cancellationToken).ConfigureAwait(false);

        // На этом момент есть 100% гарантия что соединения со всеми СУБД корректно, иначе где то сработает один из throw.

        LoadServerCache(app);
        await LoadTestServiceAsync(app, cancellationToken).ConfigureAwait(false);

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
            await app.RunAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            await Log.CloseAndFlushAsync().ConfigureAwait(false);
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
    //private static void InitJwt(WebApplicationBuilder builder)
    //{
    //    IServiceCollection services = builder.Services;
    //    _ = services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
    //    _ = services.AddSingleton<JwtService>();
    //    _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { }); // Пустая конфигурация, чтобы зарегистрировать схему

    //    // Добавляем пост-конфигурацию с использованием DI
    //    _ = services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(sp =>
    //        new PostConfigureOptions<JwtBearerOptions>(
    //            JwtBearerDefaults.AuthenticationScheme,
    //            options =>
    //            {
    //                JwtOptions jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;
    //                JwtService jwtService = sp.GetRequiredService<JwtService>();

    //                options.TokenValidationParameters = new TokenValidationParameters
    //                {
    //                    ValidateIssuer = true,
    //                    ValidIssuer = jwtOptions.Issuer,
    //                    ValidateAudience = true,
    //                    ValidAudience = jwtOptions.Audience,
    //                    ValidateLifetime = true,
    //                    ClockSkew = JwtService.ClockSkew,
    //                    ValidateIssuerSigningKey = true,
    //                    IssuerSigningKey = jwtService.IssuerSigningKey
    //                };

    //                // Добавляем обработку событий
    //                options.Events = new JwtBearerEvents
    //                {
    //                    OnMessageReceived = context =>
    //                    {
    //                        // SignalR передаёт access_token в query string
    //                        StringValues accessToken = context.Request.Query["access_token"];
    //                        PathString path = context.HttpContext.Request.Path;

    //                        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments(Url.SignalRAddress))
    //                        {
    //                            context.Token = accessToken;
    //                        }
    //                        return Task.CompletedTask;
    //                    }
    //                };

    //                //// Логируем ошибки валидации для отладки
    //                //options.Events = new JwtBearerEvents
    //                //{
    //                //    OnAuthenticationFailed = context =>
    //                //    {
    //                //        Console.WriteLine("JWT валидация провалилась");
    //                //        return Task.CompletedTask;
    //                //    },
    //                //    OnTokenValidated = context => {
    //                //        Console.WriteLine("OnTokenValidated");
    //                //        return Task.CompletedTask;
    //                //    },
    //                //    OnMessageReceived = context => {
    //                //        Console.WriteLine("OnMessageReceived");
    //                //        return Task.CompletedTask;
    //                //    },
    //                //    OnChallenge = context => {
    //                //        Console.WriteLine("OnChallenge");
    //                //        return Task.CompletedTask;
    //                //    },
    //                //    OnForbidden = context => {
    //                //        Console.WriteLine("OnForbidden");
    //                //        return Task.CompletedTask;
    //                //    }
    //                //};
    //            }));
    //}

    private static void InitJwt(WebApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        // Сохраняем для использования в JWT
        string issuer = Url.UrlDomain;
        string audience = Url.UrlDomain;

        _ = services.Configure<JwtOptions>(options =>
        {
            // Переопределяем Issuer и Audience из конфига значениями из URL
            options.Issuer = issuer;
            options.Audience = audience;
            // Lifetime оставляем из appsettings.json
        });

        _ = services.AddSingleton<JwtService>();
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            StringValues accessToken = context.Request.Query["access_token"];
                            PathString path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrWhiteSpace(accessToken) &&
                                path.StartsWithSegments(Parametrs.SignalR_Address))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
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

    /// <summary>
    /// Выполняет проверку соединения с базой данных.
    /// </summary>
    private static async Task ConnectionsIsCorrectAsync(WebApplication app, CancellationToken cancellationToken)
    {
        using IServiceScope scope = app.Services.CreateScope();
        // Использование целевого типа для логгера
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();
            // Простая проверка активности соединения
            _ = await db.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false)
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
    private static void LoadServerCache(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        CacheService service = scope.ServiceProvider.GetRequiredService<CacheService>();
        using DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            service.LoadServerData(db);

            logger.LogInformation("Server cache loaded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Server cache load failed");
            throw;
        }
    }
    /// <summary> Загружаем в оперативную память константные серверные данные которые не меняются во время работы сервера. </summary>
    private static async Task LoadTestServiceAsync(WebApplication app, CancellationToken cancellationToken)
    {
        using IServiceScope scope = app.Services.CreateScope();
        TestService service = scope.ServiceProvider.GetRequiredService<TestService>();
        CacheService cacheService = scope.ServiceProvider.GetRequiredService<CacheService>();
        using DbContextGame db = scope.ServiceProvider.GetRequiredService<DbContextGame>();
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            await service.MainAsync(db, cacheService, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("TestService loaded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "TestService load failed");
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
