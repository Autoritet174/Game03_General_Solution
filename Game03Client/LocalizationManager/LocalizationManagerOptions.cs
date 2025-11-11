using General;

namespace Game03Client.LocalizationManager;

internal class LocalizationManagerOptions(StringCapsule jsonFileData, GameLanguage language)
{
    public StringCapsule jsonFileData = jsonFileData;
    public GameLanguage Language = language;
}
