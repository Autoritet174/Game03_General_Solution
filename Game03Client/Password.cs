using System;
using System.Security.Cryptography;
using System.Text;

namespace Game03Client;

public static class Password
{
    /// <summary>Хэширует пароль SHA512.</summary>
    /// <param name="password">Пароль пользователя в plaintext.</param>
    /// <returns>Хэш пароля в формате Base64.</returns>
    public static string HashSha512(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        using var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
