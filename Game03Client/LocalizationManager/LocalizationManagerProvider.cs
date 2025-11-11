using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using L = General.LocalizationKeys;

namespace Game03Client.LocalizationManager;

internal class LocalizationManagerProvider : ILocalizationManagerProvider
{

    private readonly LocalizationManagerOptions _localizationManagerOptions;

    public LocalizationManagerProvider(LocalizationManagerOptions localizationManagerOptions)
    {
        _localizationManagerOptions = localizationManagerOptions;

        localization.Clear();
        var obj = JObject.Parse(_localizationManagerOptions.jsonFileData.Value);

        void ProcessToken(JToken token, string currentPath)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        ProcessToken(prop.Value, $"{currentPath}{prop.Name}.");
                    }
                    break;
                case JTokenType.String:
                    localization[currentPath.TrimEnd('.')] = token.ToString();
                    break;
            }
        }
        ProcessToken(obj, "");

    }


    ///// <summary>
    ///// Получение строки из Description конкретного значения Languages.
    ///// </summary>
    ///// <param name="language">Значение перечисления Languages.</param>
    ///// <returns>Строка из атрибута Description.</returns>
    //private string GetLanguageCode(Game03.LanguageGame language)
    //{
    //    FieldInfo field = typeof(Game03.LanguageGame).GetField(language.ToString());
    //    DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
    //    return attr?.Description ?? throw new InvalidOperationException($"LocalizationManager не найден Description языка {language}.");
    //}


    private readonly Dictionary<string, string> localization = [];

    public string GetValue(string key)
    {
        return localization.TryGetValue(key, out string value) ? value : key;
    }


    /// <summary>
    /// Получить строку которая является ключом к тексту ошибки.
    /// </summary>
    /// <param name="jObject"></param>
    /// <returns></returns>
    public string GetTextByJObject(JObject jObject)
    {
        if (jObject == null)
        {
            return string.Empty;
        }

        string keyError = jObject["keyError"]?.ToString() ?? L.Error.UnknownError;
        string textError = GetValue(keyError) ?? string.Empty;


        // Аккаунт забанен
        if (keyError == L.Error.Server.AccountBannedUntil)
        {
            string dateTimeExpiresAtString = (jObject["dateTimeExpiresAt"]?.ToString() ?? string.Empty).Trim();
            if (dateTimeExpiresAtString != string.Empty)
            {
                textError = textError.Replace("{datetimeExpiration}", dateTimeExpiresAtString);

                string[] dtA = dateTimeExpiresAtString.Split(new string[] { " ", ".", ":" }, StringSplitOptions.None);
                try
                {
                    DateTime dtUnbanUtc = new(int.Parse(dtA[0]), int.Parse(dtA[1]), int.Parse(dtA[2]), int.Parse(dtA[3]), int.Parse(dtA[4]), int.Parse(dtA[5]));
                    long secondsRemaining = (long)(dtUnbanUtc - DateTime.UtcNow).TotalSeconds;
                    textError = textError.Replace("{timeRemaining}", secondsRemaining > 0 ? General.StringExtensions.SecondsToTimeStr(secondsRemaining) : "0");
                }
                catch { }
            }
        }


        // Много попыток входа
        if (keyError == L.Error.Server.TooManyRequests)
        {
            string secondsRemainingString = (jObject["secondsRemaining"]?.ToString() ?? string.Empty).Trim();
            if (secondsRemainingString != string.Empty && long.TryParse(secondsRemainingString, out long secondsRemaining) && secondsRemaining > 0)
            {
                textError = textError.Replace("{timeRemaining}", General.StringExtensions.SecondsToTimeStr(secondsRemaining));
            }
        }


        return textError;
    }



}
