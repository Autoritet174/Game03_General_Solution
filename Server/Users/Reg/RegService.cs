using General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Server.Jwt_NS;
using Server_DB_Postgres.Entities.Users;
using System.Net;
using System.Text.Json.Nodes;

namespace Server.Users.Reg;

/// <summary>
/// Сервис для обработки регистрации пользователей.
/// </summary>
public sealed class RegService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService)
{
    private static readonly Random _Random = new(); // Генератор случайных задержек для защиты от timing-атак
    private static readonly TimeSpan _LockoutPeriod = TimeSpan.FromMinutes(2); // Период блокировки при неудачных попытках

    /// <summary>
    /// Асинхронный метод регистрации пользователя.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="requestJson">JSON запроса для логирования.</param>
    /// <param name="ip">IP-адрес клиента.</param>
    /// <returns>Результат регистрации.</returns>
    public async Task<AuthRegResponse> RegisterAsync(
        string email,
        string password,
        JsonObject requestJson,
        IPAddress? ip)
    {
        // Инициализация времени для задержек
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

        Guid? userId = null; // ID пользователя для логирования
        bool success = false;

        try
        {
            // Проверка входных данных
            if (email.IsEmpty() || password.IsEmpty())
            {
                return AuthRegResponse.InvalidCredentials();
            }

            // Проверка существования пользователя
            User? existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return AuthRegResponse.UserAlreadyExists();
            }

            // Создание нового пользователя
            User user = new()
            {
                UserName = email,
                Email = email
            };

            // Попытка создания пользователя с паролем
            IdentityResult createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                return AuthRegResponse.InvalidCredentials();
            }
            userId = user.Id; // Установка ID после успешного создания

            // Автоматический вход после регистрации
            SignInResult signInResult = await signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);
            if (!signInResult.Succeeded)
            {
                return AuthRegResponse.InvalidCredentials();
            }

            // Проверка на блокировку
            if (signInResult.IsLockedOut)
            {
                return AuthRegResponse.TooManyRequests(GetRemainingLockoutTime(email));
            }

            // Проверка на необходимость 2FA
            if (signInResult.RequiresTwoFactor)
            {
                return AuthRegResponse.RequiresTwoFactor(user.Id);
            }

            success = true;
            ResetAttempts(email); // Сброс попыток при успехе
            return AuthRegResponse.SuccessResponse(jwt.GenerateToken(user.Id)); // Генерация JWT-токена
        }
        catch
        {
            return AuthRegResponse.InvalidResponse();
        }
        finally
        {
            try
            {
                backgroundLoggerAuthentificationService.EnqueueLog(success, requestJson, email, userId, ip, false);
                if (!success)
                {
                    RegisterFail(email);
                }
                await Delay(dtEndCheck); // Задержка перед возвратом
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in finally block during registration for email {Email}", email);
            }
        }
    }


    /// <summary>
    /// Метод для выполнения задержки.
    /// </summary>
    /// <param name="end">Время окончания задержки.</param>
    private static async Task Delay(DateTimeOffset end)
    {
        TimeSpan delay = end - DateTimeOffset.UtcNow;
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }
    }

    /// <summary>
    /// Регистрация неудачной попытки.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    private void RegisterFail(string email)
    {
        IncrementFailedRegisterAttempt(email);
    }

    /// <summary>
    /// Запись для попыток.
    /// </summary>
    private record Attempt(int Count, DateTimeOffset ExpiresAt);

    /// <summary>
    /// Увеличение счетчика неудачных попыток.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    private void IncrementFailedRegisterAttempt(string email)
    {
        if (email.IsEmpty())
        {
            return;
        }

        // Формирование ключа кэша
        string key = $"register-attempts:{email.NormalizedValueGame03()}";
        DateTimeOffset expires = DateTimeOffset.UtcNow + _LockoutPeriod;

        // Получение или создание попытки в кэше
        Attempt? attempt = cache.GetOrCreate(key, e =>
        {
            e.AbsoluteExpiration = expires;
            return new Attempt(1, expires);
        });

        // Обновление кэша с увеличенным счетчиком
        _ = cache.Set(key,
            new Attempt(attempt!.Count + 1, expires),
            _LockoutPeriod);
    }

    /// <summary>
    /// Сброс попыток для email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    private void ResetAttempts(string email)
    {
        if (!email.IsEmpty())
        {
            cache.Remove($"register-attempts:{email.NormalizedValueGame03()}");
        }
    }

    /// <summary>
    /// Получение оставшегося времени блокировки.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Оставшееся время в секундах.</returns>
    private long GetRemainingLockoutTime(string email)
    {
        // Проверка наличия в кэше
        return cache.TryGetValue(
            $"register-attempts:{email.NormalizedValueGame03()}",
            out Attempt? attempt)
            && attempt!.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
            : 0;
    }
}
