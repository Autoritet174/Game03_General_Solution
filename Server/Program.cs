using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.DB.Data;
using Server.DB.Data.Repositories;
using Server.DB.Users;
using Server.DB.Users.Repositories;
using Server.GameDataCache;
using Server.Http_NS.Middleware_NS;
using Server.Jwt_NS;
using System.Text;

namespace Server;

internal class Program
{

    public static void Configure(WebApplication app)
    {

        //Миддлвар 1 - Обработка ошибок
        _ = app.UseMiddleware<ExceptionLoggingMiddleware>();
        //_ = app.UseExceptionHandler("/Home/Error");// этот мидлвар не нужен так как сервер обслуживает только API, без сайта и вебстраниц

        //Миддлвар 2 - Логирование
        //_ = app.UseHttpLogging();

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
        _ = services.AddDbContext<DbContext_Game03Users>(options => options.UseNpgsql(DbUsers.GetConnectionString()));
        _ = services.AddScoped<UserRepository>();


        // База данных с игровыми данными
        _ = services.AddDbContext<DbContext_Game03Data>(options => options.UseNpgsql(DbData.GetConnectionString()));
        _ = services.AddScoped<HeroRepository>();


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


        DbUsers.Init();
        DbData.Init();

        Console.WriteLine("TestConnectionWithDataBase Users - " + DbUsers.GetStateConnection());
        Console.WriteLine("TestConnectionWithDataBase Data  - " + DbData.GetStateConnection());
        // Зеленый текст (как "info" в ASP.NET Core)

        ListAllHeroes.Init();

        //HeroesController.Init();
        _ = Test(app);



        app.Run();

    }

    private static async Task Test(WebApplication app)
    {
        await Task.Delay(1);
        //using DbData db = new();
        //var hero = db.Heroes.First(a=>a.Name == "Warrior");
        //var ct = db.CreatureTypes.First(a => a.Name == "Humanoid");
        //hero.CreatureTypes.Add(ct);
        //db.SaveChanges();
    }
}