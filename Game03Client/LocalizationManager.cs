
using System;
using System.Collections.Generic;
using L = General.LocalizationKeys;
using JsonDocument = System.Text.Json.JsonDocument;
using JsonElement = System.Text.Json.JsonElement;
using JsonValueKind = System.Text.Json.JsonValueKind;
using JsonProperty = System.Text.Json.JsonProperty;

namespace Game03Client;

/// <summary>
/// Реализация провайдера для управления локализацией, загружающая данные из JSON.
/// </summary>
public static class LocalizationManager
{
    private static readonly Dictionary<string, string> localization = [];

    public static void Init(StringCapsule jsonFileData)
    {
        localization.Clear();

        using var doc = JsonDocument.Parse(jsonFileData.value);
        ProcessElement(doc.RootElement, "");
    }

    private static void ProcessElement(JsonElement element, string currentPath)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty prop in element.EnumerateObject())
                {
                    ProcessElement(prop.Value, $"{currentPath}{prop.Name}.");
                }
                break;

            case JsonValueKind.String:
                localization[currentPath.TrimEnd('.')] = element.GetString() ?? string.Empty;
                break;
        }
    }

    /// <summary>
    /// Получает локализованное строковое значение по ключу.
    /// </summary>
    /// <returns>Локализованная строка, или сам ключ, если значение не найдено.</returns>
    public static string GetValue(string key) =>
        localization.TryGetValue(key, out string value) ? value : key;

    /// <summary>
    /// Получить локализованную строку текста ошибки на основе данных из JSON-строки.
    /// </summary>
    public static string GetTextByJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }

        try
        {
            using var doc = JsonDocument.Parse(json);
            return GetTextByElement(doc.RootElement);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Получить локализованную строку текста ошибки на основе данных из <see cref="JsonElement"/>.
    /// </summary>
    public static string GetTextByElement(JsonElement root)
    {
        if (root.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return string.Empty;
        }

        string keyError = root.TryGetProperty(L.keyLocalization, out JsonElement keyEl)
            ? keyEl.GetString() ?? L.Error.unknownError
            : L.Error.unknownError;

        string textError = GetValue(keyError);

        // --- Специальная обработка для ошибки "Аккаунт забанен" ---
        if (keyError == L.Error.Server.accountBannedUntil)
        {
            string dateTimeExpiresAtString = root.TryGetProperty(L.datetimeExpiration, out JsonElement dtEl)
                ? (dtEl.GetString() ?? string.Empty).Trim()
                : string.Empty;

            if (dateTimeExpiresAtString != string.Empty)
            {
                textError = textError.Replace(L.datetimeExpiration, dateTimeExpiresAtString);

                string[] dtA = dateTimeExpiresAtString.Split([" ", ".", ":"], StringSplitOptions.None);
                try
                {
                    DateTime dtUnbanUtc = new(
                        int.Parse(dtA[0]), int.Parse(dtA[1]), int.Parse(dtA[2]),
                        int.Parse(dtA[3]), int.Parse(dtA[4]), int.Parse(dtA[5]));

                    long secondsRemaining = (long)(dtUnbanUtc - DateTime.UtcNow).TotalSeconds;
                    textError = textError.Replace(L.timeRemaining, GlobalHelper.SecondsToTimeStr(secondsRemaining));
                }
                catch { }
            }
        }

        // --- Специальная обработка для ошибки "Слишком много попыток входа" ---
        if (keyError == L.Error.Server.tooManyRequests)
        {
            string secondsRemainingString = root.TryGetProperty(L.secondsRemaining, out JsonElement srEl)
                ? (srEl.GetString() ?? string.Empty).Trim()
                : string.Empty;

            if (secondsRemainingString != string.Empty
                && long.TryParse(secondsRemainingString, out long secondsRemaining))
            {
                textError = textError.Replace(L.timeRemaining, GlobalHelper.SecondsToTimeStr(secondsRemaining));
            }
        }

        return textError;
    }
}
