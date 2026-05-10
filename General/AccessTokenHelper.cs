using System;
using System.Text;
using JsonElement = System.Text.Json.JsonElement;

namespace General;

public static class AccessTokenHelper
{
    /// <summary>
    /// Проверяет, действителен ли JWT access-токен по времени (exp и nbf), без валидации подписи.
    /// </summary>
    /// <param name="accessToken">Строка JWT токена</param>
    /// <returns>true — токен ещё действителен по времени, false — истёк или ещё не начал действовать</returns>
    public static bool IsStillValid(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        try
        {
            string[] parts = accessToken.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            string payload = parts[1];

            // JWT Base64Url может не иметь padding, добавляем если нужно
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            byte[] jsonBytes = Convert.FromBase64String(payload);
            string json = Encoding.UTF8.GetString(jsonBytes);

            using var doc = JSON.Parse(json);
            JsonElement root = doc.RootElement;

            long currentUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Проверяем exp (expiration) — обязательно
            if (!root.TryGetProperty("exp", out JsonElement expElement) ||
                !expElement.TryGetInt64(out long exp))
            {
                return false; // Нет exp — считаем недействительным
            }

            if (currentUnix - exp > -30) // истечёт через 30 секунд — считаем истекшим
            {
                return false;
            }

            // Проверяем nbf (not before) — опционально
            if (root.TryGetProperty("nbf", out JsonElement nbfElement) &&
                nbfElement.TryGetInt64(out long nbf))
            {
                if (currentUnix < nbf)
                {
                    return false;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
