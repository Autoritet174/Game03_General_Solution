using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Server.Http_NS.Controllers_NS;
using Server.Users.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Server.Users;
/// <summary>
/// Контроллер для управления процессами регистрации и аутентификации через Passkeys.
/// </summary>
public sealed class PasskeyController(
    PasskeyService passkeyService,
    IMemoryCache cache,
    ILogger<PasskeyController> logger) : ControllerBaseApi
{
    private static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(5);

    private const string REGISTRATION_CACHE_PREFIX = "fido2_reg_";
    private const string ASSERTION_CACHE_PREFIX = "fido2_auth_";

    /// <summary>
    /// Флаг доступности функционала Passkey.
    /// </summary>
    private static readonly bool IsEnabled = false;

    /// <summary>
    /// Получает параметры для создания нового ключа доступа.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    [HttpGet("register/options")]
    public async Task<IActionResult> GetRegisterOptions([FromQuery] Guid userId, [FromQuery] string email)
    {
        if (IsEnabled)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Passkey registration is currently disabled.");
        }

        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Invalid user data provided.");
        }

        CredentialCreateOptions options = await passkeyService.GetRegistrationOptionsAsync(userId, email);

        // Сохраняем опции в кэш для последующей верификации
        _ = cache.Set($"{REGISTRATION_CACHE_PREFIX}{userId}", options, SessionTimeout);

        return Ok(options);
    }

    /// <summary>
    /// Подтверждает регистрацию ключа после взаимодействия пользователя с устройством.
    /// </summary>
    [HttpPost("register/confirm")]
    public async Task<IActionResult> ConfirmRegister([FromBody] DtoRegisterConfirm request)
    {
        if (IsEnabled)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Passkey registration is currently disabled.");
        }

        if (request?.Response == null)
        {
            return BadRequest("Request body or response is missing.");
        }

        if (!cache.TryGetValue($"{REGISTRATION_CACHE_PREFIX}{request.UserId}", out CredentialCreateOptions? originalOptions) || originalOptions == null)
        {
            return Conflict("Registration session expired or not found.");
        }

        await passkeyService.ConfirmRegistrationAsync(request.Response, originalOptions, request.UserId);

        cache.Remove($"{REGISTRATION_CACHE_PREFIX}{request.UserId}");

        return Ok(new { status = "Success" });
    }

    /// <summary>
    /// Получает параметры для выполнения входа (assertion).
    /// </summary>
    /// <param name="userId">Необязательный ID пользователя для фильтрации ключей.</param>
    [HttpGet("login/options")]
    public async Task<IActionResult> GetLoginOptions([FromQuery] Guid? userId)
    {
        if (IsEnabled)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Passkey registration is currently disabled.");
        }

        AssertionOptions options = await passkeyService.GetLoginOptionsAsync(userId);

        // Используем Challenge как ключ сессии
        string challengeKey = Convert.ToBase64String(options.Challenge);
        _ = cache.Set($"{ASSERTION_CACHE_PREFIX}{challengeKey}", options, SessionTimeout);

        return Ok(options);
    }

    /// <summary>
    /// Проверяет подпись ключа и завершает процесс входа.
    /// </summary>
    [HttpPost("login/confirm")]
    public async Task<IActionResult> ConfirmLogin([FromBody] DtoLoginConfirm request)
    {
        if (IsEnabled)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Passkey registration is currently disabled.");
        }

        if (request?.Response == null || string.IsNullOrEmpty(request.Challenge))
        {
            return BadRequest("Invalid login confirmation request.");
        }

        if (!cache.TryGetValue($"{ASSERTION_CACHE_PREFIX}{request.Challenge}", out AssertionOptions? originalOptions) || originalOptions == null)
        {
            return Conflict("Authentication session expired.");
        }

        // Выполняем проверку через сервис
        Guid userId = await passkeyService.ConfirmLoginAsync(request.Response, originalOptions);

        cache.Remove($"{ASSERTION_CACHE_PREFIX}{request.Challenge}");

        return Ok(new { UserId = userId });
    }
}

#region DTO Classes

/// <summary>
/// Данные для завершения регистрации ключа.
/// </summary>
public sealed record DtoRegisterConfirm(
    [Required] Guid UserId,
    [Required] AuthenticatorAttestationRawResponse Response);

/// <summary>
/// Данные для завершения входа по ключу.
/// </summary>
public sealed record DtoLoginConfirm(
    [Required] string Challenge,
    [Required] AuthenticatorAssertionRawResponse Response);

#endregion
