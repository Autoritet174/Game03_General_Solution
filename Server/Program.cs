using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.DB.Users;
using Server.DB.Users.Repositories;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using System.Text;

namespace Server;
internal class Program
{

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

    private static void Main(string[] args)
    {
        Utilities.ConsoleWindow.Restore();
        if (!General.ServerErrors.CheckEnumServerResponse())
        {
            Console.WriteLine("Bad enum ServerResponse");
            _ = Console.ReadLine();
            return;
        }

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Инициализация параметров для AuthOptions при старте приложения
        //Jwt.Initialize(builder.Configuration);

        IServiceCollection services = builder.Services;

        // Добавление контроллеров
        _ = services.AddControllers();
        _ = services.AddHttpLogging();

        // Регистрация ClientManager как singleton
        _ = services.AddSingleton<ClientManager>();





        // Добавление аутентификации с использованием JWT
        _ = builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt")); // Jwt.Issuer, Jwt.Audience, Jwt.Lifetime из конфигурации

        _ = builder.Services.AddSingleton<JwtService>();

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
                _ = policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });

        string connectionString_DbUsers = builder.Configuration.GetConnectionString("DB.Users") ?? throw new Exception("connectionString_DataBase_Game03Users is empty");
        _ = services.AddDbContext<DbContext_Game03Users>(options => options.UseNpgsql(connectionString_DbUsers));

        _ = services.AddScoped<UserRepository>();


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
        _ = Test(app);

        app.Run();

    }


    private static async Task Test(WebApplication app)
    {
        await Task.Delay(1);
        //using IServiceScope scope = app.Services.CreateScope();
        ////TestUsers userService = scope.ServiceProvider.GetRequiredService<TestUsers>();

        ////for (int i = 0; i < 1; i++)
        ////{
        ////    await userService.CreateUserAsync();
        ////}
        //UserRepository userService = scope.ServiceProvider.GetRequiredService<UserRepository>();
        //DB.Users.Entities.User? all = userService.GetUserByEmailAsync("sUpEraDmiN@maIl.RU").Result;

        DbContextOptionsBuilder<DbContext_Game03Users> options = new();

        _ = options.UseNpgsql("Host=127.127.126.5;Port=5432;Database=Game03_Users;Username=postgres;Password=");

        using DbContext_Game03Users db = new(options.Options);
        DbSet<DB.Users.Entities.User> allq = db.Users;
        //Console.WriteLine(db.Users.First().Email);
    }
}