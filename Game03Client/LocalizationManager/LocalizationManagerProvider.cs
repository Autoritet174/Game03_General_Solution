using Game03Client.JwtToken;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using L = General.LocalizationKeys;

namespace Game03Client.LocalizationManager;

/// <summary>
/// Реализация провайдера для управления локализацией, загружающая данные из JSON.
/// </summary>
internal class LocalizationManagerProvider : ILocalizationManager
{
    #region Logger
    private readonly ILogger _logger;
    private const string NAME_THIS_CLASS = nameof(LocalizationManagerProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger


    /// <summary>
    /// Опции, содержащие данные локализации в формате JSON.
    /// </summary>
    private readonly LocalizationManagerOptions _localizationManagerOptions;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="LocalizationManagerProvider"/>.
    /// </summary>
    /// <param name="localizationManagerOptions">Опции локализации, содержащие JSON-строку с данными.</param>
    /// <param name="logger">Логер для вызова коллбеков в игре.</param>
    /// <exception cref="Newtonsoft.Json.JsonReaderException">Вызывается, если JSON-строка в опциях недействительна.</exception>
    public LocalizationManagerProvider(LocalizationManagerOptions localizationManagerOptions, ILogger logger)
    {
        _localizationManagerOptions = localizationManagerOptions;
        _logger = logger;

        // Очистка предыдущих данных локализации (если конструктор вызывается повторно, что маловероятно для провайдера).
        localization.Clear();
        // Разбор JSON-строки, содержащей данные локализации.
        var obj = JObject.Parse(_localizationManagerOptions.jsonFileData.Value);

        void ProcessToken(JToken token, string currentPath)
        {
            //Рекурсивный обход токенов JToken для построения плоского словаря локализации.
            switch (token.Type)
            {
                case JTokenType.Object:
                    // Если токен - объект, обходим его свойства.
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        // Рекурсивный вызов с добавлением имени свойства к текущему пути.
                        ProcessToken(prop.Value, $"{currentPath}{prop.Name}.");
                    }
                    break;
                case JTokenType.String:
                    // Если токен - строка (значение локализации), добавляем его в словарь.
                    localization[currentPath.TrimEnd('.')] = token.ToString();
                    break;
            }
        }

        // Запуск процесса обхода с корневого объекта и пустым путем.
        ProcessToken(obj, "");
        //foreach (KeyValuePair<string, string> item in localization)
        //{
        //    Log($"key={item.Key}; value={item.Value}");
        //}
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


    /// <summary>
    /// Словарь, хранящий ключи локализации (путь в JSON) и соответствующие им строковые значения.
    /// </summary>
    private readonly Dictionary<string, string> localization = [];

    /// <summary>
    /// Получает локализованное строковое значение по ключу.
    /// </summary>
    /// <param name="key">Ключ локализации (например, "UI.Menu.StartGame").</param>
    /// <returns>Локализованная строка, или сам ключ, если значение не найдено.</returns>
    public string GetValue(string key)
    {
        // Попытка получить значение по ключу. Если ключ не найден, возвращается сам ключ.
        return localization.TryGetValue(key, out string value) ? value : key;
    }


    /// <summary>
    /// Получить локализованную строку текста ошибки на основе данных из <see cref="JObject"/>.
    /// </summary>
    /// <param name="jObject">Объект JSON, содержащий ключ ошибки ("keyError") и дополнительные данные.</param>
    /// <returns>Локализованная строка ошибки с подставленными данными, или пустая строка, если <paramref name="jObject"/> равен null.</returns>
    public string GetTextByJObject(JObject jObject)
    {
        // Проверка на null
        if (jObject == null)
        {
            return string.Empty;
        }

        // Извлечение ключа ошибки. Если ключ отсутствует, используется ключ для "Неизвестная ошибка".
        string keyError = jObject[L.KEY_LOCALIZATION]?.ToString() ?? L.Error.UnknownError;
        // Получение локализованного текста по ключу ошибки.
        string textError = GetValue(keyError) ?? string.Empty;


        // --- Специальная обработка для ошибки "Аккаунт забанен" ---
        if (keyError == L.Error.Server.AccountBannedUntil)
        {
            // Извлечение строки с датой и временем окончания бана.
            string dateTimeExpiresAtString = (jObject[L.DATE_TIME_EXPIRES_AT]?.ToString() ?? string.Empty).Trim();

            if (dateTimeExpiresAtString != string.Empty)
            {
                // Замена плейсхолдера {datetimeExpiration} на дату/время окончания бана.
                textError = textError.Replace(L.DATETIME_EXPIRATION, dateTimeExpiresAtString);

                // Попытка вычислить оставшееся время до разблокировки.
                string[] dtA = dateTimeExpiresAtString.Split([" ", ".", ":"], StringSplitOptions.None);
                try
                {
                    // Создание объекта DateTime (предполагая, что дата/время передается в UTC).
                    DateTime dtUnbanUtc = new(int.Parse(dtA[0]), int.Parse(dtA[1]), int.Parse(dtA[2]), int.Parse(dtA[3]), int.Parse(dtA[4]), int.Parse(dtA[5]));
                    // Вычисление оставшихся секунд.
                    long secondsRemaining = (long)(dtUnbanUtc - DateTime.UtcNow).TotalSeconds;
                    // Замена плейсхолдера {timeRemaining} на строку оставшегося времени или "0".
                    textError = textError.Replace(L.TIME_REMAINING, secondsRemaining > 0 ? General.G.SecondsToTimeStr(secondsRemaining) : "0");
                }
                catch
                {
                    // Игнорируем исключения при парсинге или вычислении, если они произошли.
                }
            }
        }


        // --- Специальная обработка для ошибки "Слишком много попыток входа" ---
        if (keyError == L.Error.Server.TooManyRequests)
        {
            // Извлечение строки с количеством оставшихся секунд до сброса лимита.
            string secondsRemainingString = (jObject[L.SECONDS_REMAINING]?.ToString() ?? string.Empty).Trim();

            // Если строка не пуста, является числом, и число больше 0.
            if (secondsRemainingString != string.Empty && long.TryParse(secondsRemainingString, out long secondsRemaining) && secondsRemaining > 0)
            {
                // Замена плейсхолдера {timeRemaining} на строку оставшегося времени.
                textError = textError.Replace(L.TIME_REMAINING, General.G.SecondsToTimeStr(secondsRemaining));
            }
        }


        return textError;
    }



}
