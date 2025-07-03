using Microsoft.AspNetCore.Identity;

namespace Server.DB.Users.Utilities;

public static class PasswordHasher
{
    private static readonly PasswordHasher<object> _passwordHasher = new();

    public static string Create(string user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public static bool Verify(string user, string hashedPassword, string providedPassword)
    {
        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }

}
