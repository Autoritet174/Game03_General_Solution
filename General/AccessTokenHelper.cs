using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
                return false; // Некорректный JWT
            }

            string payload = parts[1];

            // JWT Base64Url может не иметь padding, добавляем если нужно
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            // Декодируем payload в JSON
            byte[] jsonBytes = Convert.FromBase64String(payload);
            string json = System.Text.Encoding.UTF8.GetString(jsonBytes);

            var payloadObj = JObject.Parse(json);

            long currentUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Проверяем exp (expiration) — обязательно
            if (payloadObj["exp"] is JToken expToken && long.TryParse(expToken.ToString(), out long exp))
            {
                if (currentUnix >= exp)
                {
                    return false; // Истёк
                }
            }
            else
            {
                return false; // Нет exp — считаем недействительным
            }

            // Проверяем nbf (not before) — опционально, но рекомендуется
            if (payloadObj["nbf"] is JToken nbfToken && long.TryParse(nbfToken.ToString(), out long nbf))
            {
                if (currentUnix < nbf)
                {
                    return false; // Ещё не начал действовать
                }
            }

            return true; // Валиден по времени
        }
        catch
        {
            // Любая ошибка парсинга — токен считаем недействительным
            return false;
        }
    }

}
