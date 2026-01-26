using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Users;
using System.Security.Authentication;

namespace Server.Users.Authentication;

/// <summary>
/// Сервис для управления ключами доступа (Passkeys).
/// </summary>
public sealed class PasskeyService(
    IFido2 fido2,
    DbContextGame dbContext,
    ILogger<PasskeyService> logger)
{
    /// <summary>
    /// Генерирует параметры регистрации нового ключа.
    /// </summary>
    public async Task<CredentialCreateOptions> GetRegistrationOptionsAsync(Guid userId, string userEmail)
    {
        ArgumentNullException.ThrowIfNull(userEmail);
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("USER_ID_EMPTY", nameof(userId));
        }

        List<PublicKeyCredentialDescriptor> existingCredentials = await dbContext.Set<UserAccesskey>()
            .Where(p => p.UserId == userId)
            .Select(p => new PublicKeyCredentialDescriptor(p.DescriptorId))
            .ToListAsync();

        var user = new Fido2User
        {
            DisplayName = userEmail,
            Name = userEmail,
            Id = userId.ToByteArray()
        };

        var parameters = new RequestNewCredentialParams
        {
            User = user,
            ExcludeCredentials = existingCredentials,
            AuthenticatorSelection = new AuthenticatorSelection
            {
                ResidentKey = ResidentKeyRequirement.Required,
                UserVerification = UserVerificationRequirement.Preferred,
                AuthenticatorAttachment = AuthenticatorAttachment.Platform
            }
        };

        return fido2.RequestNewCredential(parameters);
    }

    /// <summary>
    /// Подтверждает регистрацию и сохраняет ключ.
    /// </summary>
    public async Task ConfirmRegistrationAsync(
        AuthenticatorAttestationRawResponse clientResponse,
        CredentialCreateOptions originalOptions,
        Guid userId)
    {
        ArgumentNullException.ThrowIfNull(clientResponse);
        ArgumentNullException.ThrowIfNull(originalOptions);

        var parameters = new MakeNewCredentialParams
        {
            AttestationResponse = clientResponse,
            OriginalOptions = originalOptions,
            IsCredentialIdUniqueToUserCallback = async (args, ct) =>
            {
                return !await dbContext.Set<UserAccesskey>()
                    .AnyAsync(p => p.DescriptorId == args.CredentialId, ct);
            }
        };

        RegisteredPublicKeyCredential credential = await fido2.MakeNewCredentialAsync(parameters);

        var passkey = new UserAccesskey
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DescriptorId = credential.Id,
            PublicKey = credential.PublicKey,
            SignatureCounter = credential.SignCount,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _ = dbContext.UserAccesskeys.Add(passkey);
        _ = await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Генерирует параметры для входа. Исправлено на GetAssertionOptionsParams.
    /// </summary>
    public async Task<AssertionOptions> GetLoginOptionsAsync(Guid? userId)
    {
        var allowedCredentials = new List<PublicKeyCredentialDescriptor>();

        if (userId.HasValue && userId.Value != Guid.Empty)
        {
            allowedCredentials = await dbContext.Set<UserAccesskey>()
                .Where(p => p.UserId == userId.Value)
                .Select(p => new PublicKeyCredentialDescriptor(p.DescriptorId))
                .ToListAsync();
        }

        // ИСПРАВЛЕНИЕ: Использование GetAssertionOptionsParams
        var assertionParams = new GetAssertionOptionsParams
        {
            AllowedCredentials = allowedCredentials,
            UserVerification = UserVerificationRequirement.Preferred
        };

        return fido2.GetAssertionOptions(assertionParams);
    }

    /// <summary>
    /// Проверяет подпись при входе.
    /// </summary>
    public async Task<Guid> ConfirmLoginAsync(
        AuthenticatorAssertionRawResponse clientResponse,
        AssertionOptions originalOptions)
    {
        ArgumentNullException.ThrowIfNull(clientResponse);
        ArgumentNullException.ThrowIfNull(originalOptions);

        UserAccesskey dbKey = await dbContext.Set<UserAccesskey>()
            .FirstOrDefaultAsync(p => p.DescriptorId == clientResponse.RawId)
            ?? throw new AuthenticationException("PASSKEY_NOT_FOUND");

        var parameters = new MakeAssertionParams
        {
            AssertionResponse = clientResponse,
            OriginalOptions = originalOptions,
            StoredPublicKey = dbKey.PublicKey,
            StoredSignatureCounter = dbKey.SignatureCounter,
            IsUserHandleOwnerOfCredentialIdCallback = async (args, ct) =>
            {
                return await dbContext.Set<UserAccesskey>()
                    .AnyAsync(p => p.DescriptorId == args.CredentialId, ct);
            }
        };

        VerifyAssertionResult result = await fido2.MakeAssertionAsync(parameters);

        dbKey.SignatureCounter = result.SignCount;
        _ = await dbContext.SaveChangesAsync();

        return dbKey.UserId;
    }
}
