using Microsoft.AspNetCore.Identity;

namespace Server.Utilities;

public static class PassHasher
{
    private static readonly PasswordHasher<object> passwordHasher = new();

    public static string Create(string user, string password)
    {
        return passwordHasher.HashPassword(user, password);
    }

    public static bool Verify(string user, string hashedPassword, string providedPassword)
    {
        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
