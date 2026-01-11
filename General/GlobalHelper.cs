using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace General;

/// <summary>
/// Глобальный статический класс, содержащий константы и вспомогательные функции,
/// которые должны быть легко доступны из любого места приложения.
/// </summary>
public static class GlobalHelper
{
    public static JsonSerializerSettings JsonSerializerSettings;
    static GlobalHelper()
    {
        JsonSerializerSettings = new JsonSerializerSettings
        {
            // Использование CamelCase для всех полей, если атрибуты не заданы явно
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            // Компактный вид (аналог WriteIndented = false)
            Formatting = Formatting.None,
            // Игнорирование null значений для уменьшения веса JSON
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    /// <summary>
    /// Константа для типа медиа "application/json", часто используемого в HTTP-запросах.
    /// </summary>
    public const string APPLICATION_JSON = "application/json";

    /// <summary>
    /// Преобразует общее количество секунд в удобочитаемую строку,
    /// форматируя их в дни, часы, минуты и секунды (d hh mm s).
    /// </summary>
    /// <param name="sec">Общее количество секунд (<see cref="long"/>), которое необходимо преобразовать.</param>
    /// <returns>
    /// Строка, представляющая время в формате "dd hh mm s".
    /// Пример: "05m 10s", "01h 05m 10s", "1d 01h 05m 10s".
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если <paramref name="sec"/> меньше нуля.</exception>
    public static string SecondsToTimeStr(long sec)
    {
        // Проверка входных данных: количество секунд не может быть отрицательным.
        if (sec < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sec), "Количество секунд не может быть отрицательным.");
        }
        long min = sec / 60L;
        sec -= min * 60L;

        long hours = min / 60L;
        min -= hours * 60L;

        long days = hours / 24L;
        hours -= days * 24L;

        string result = $"{min:00}m {sec:00}s";
        if (days > 0)
        {
            result = $"{days}d {hours:00}h {result}";
        }
        else if (hours > 0)
        {
            result = $"{hours:00}h {result}";
        }

        return result;
    }

    /// <summary>
    /// Проверяет, действителен ли JWT access-токен по времени (exp и nbf), без валидации подписи.
    /// </summary>
    /// <param name="accessToken">Строка JWT токена</param>
    /// <returns>true — токен ещё действителен по времени, false — истёк или ещё не начал действовать</returns>
    public static bool IsAccessTokenStillValid(string accessToken)
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
