using General;
using General.DTO.RestRequest;
using General.DTO.RestResponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Jwt_NS;
using Server_DB_Postgres.Entities.Users;
using System.Net;

namespace Server.Users.Registration;

/// <summary>
/// Сервис для обработки регистрации пользователей.
/// </summary>
public sealed class RegService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    JwtService jwt,
    IMemoryCache cache,
    AuthRegLoggerBackgroundService backgroundLoggerAuthentificationService,
    ILogger<RegService> logger)
{
    private static readonly Random _Random = new(); // Генератор случайных задержек для защиты от timing-атак
    private static readonly TimeSpan _LockoutPeriod = TimeSpan.FromMinutes(2); // Период блокировки при неудачных попытках

    public async Task<DtoResponseAuthReg> RegisterAsync(DtoRequestAuthReg dto, IPAddress? ip)
    {
        // Инициализация времени для задержек
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset dtEndCheck = now.AddMilliseconds(_Random.Next(600, 800));

        Guid? userId = null; // ID пользователя для логирования
        bool success = false;
        string? email = dto.Email;
        string? password = dto.Password;
        try
        {
            // Проверка входных данных
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
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
                return AuthRegResponse.RequiresTwoFactor();
            }

            success = true;
            ResetAttempts(email); // Сброс попыток при успехе


            string accessToken = jwt.GenerateToken(user.Id);
            
            // 
            return AuthRegResponse.Success(accessToken, "временная заглушка для рефреш токена", DateTimeOffset.Now);
        }
        catch
        {
            return AuthRegResponse.InvalidResponse();
        }
        finally
        {
            try
            {
                backgroundLoggerAuthentificationService.EnqueueLog(success, dto, userId, ip, false);
                if (!success)
                {
                    RegisterFail(email);
                }
                await Delay(dtEndCheck); // Задержка перед возвратом
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, "Error in finally block during registration for email {Email}", email);
                }
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
    private void RegisterFail(string email) => IncrementFailedRegisterAttempt(email);

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
        if (string.IsNullOrWhiteSpace(email))
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
        if (!string.IsNullOrWhiteSpace(email))
        {
            cache.Remove($"register-attempts:{email.NormalizedValueGame03()}");
        }
    }

    /// <summary>
    /// Получение оставшегося времени блокировки.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Оставшееся время в секундах.</returns>
    private long GetRemainingLockoutTime(string email) => cache.TryGetValue(
            $"register-attempts:{email.NormalizedValueGame03()}",
            out Attempt? attempt)
            && attempt!.ExpiresAt > DateTimeOffset.UtcNow
            ? (long)Math.Ceiling((attempt.ExpiresAt - DateTimeOffset.UtcNow).TotalSeconds)
            : 0;
}
