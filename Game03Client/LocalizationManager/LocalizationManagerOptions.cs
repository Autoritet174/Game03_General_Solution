using General;

namespace Game03Client.LocalizationManager;

/// <summary>
/// Предоставляет опции для инициализации провайдера локализации.
/// </summary>
/// <param name="jsonFileData">Капсула, содержащая строку с данными локализации в формате JSON.</param>
/// <param name="language">Язык, для которого предназначены данные локализации.</param>
public class LocalizationManagerOptions(StringCapsule jsonFileData, GameLanguage language)
{
    /// <summary>
    /// Данные локализации в формате JSON, обернутые в <see cref="StringCapsule"/>.
    /// </summary>
    public StringCapsule jsonFileData = jsonFileData;

    /// <summary>
    /// Указывает язык, соответствующий загруженным данным.
    /// </summary>
    public GameLanguage Language = language;
}
