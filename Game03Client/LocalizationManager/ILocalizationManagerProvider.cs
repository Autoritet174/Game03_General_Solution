using Newtonsoft.Json.Linq;

namespace Game03Client.LocalizationManager;

/// <summary>
/// Определяет контракт для провайдеров, управляющих получением локализованных строк.
/// </summary>
public interface ILocalizationManagerProvider
{
    /// <summary>
    /// Получает локализованную строку текста ошибки на основе данных из JSON-объекта.
    /// </summary>
    /// <param name="jObject">Объект JSON, содержащий данные для локализации ошибки (например, "keyError").</param>
    /// <returns>Локализованная строка ошибки с подставленными данными, если применимо.</returns>
    string GetTextByJObject(JObject jObject);

    /// <summary>
    /// Получает локализованное строковое значение по заданному ключу.
    /// </summary>
    /// <param name="key">Ключ локализации.</param>
    /// <returns>Локализованная строка. Если ключ не найден, возвращается сам ключ.</returns>
    string GetValue(string key);
}
