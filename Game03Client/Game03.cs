using General;

namespace Game03Client;

/// <summary>
/// Класс содержащий логику и веб взаимодействие игрового клиента.
/// </summary>
public static class Game03
{
    public static void Init(string iniFileFullPath, StringCapsule stringCapsuleJsonFileData, LoggerCallbackError loggerCallbackError, LoggerCallbackInfo loggerCallbackInfo)
    {
        HttpRequester.Init();
        LoggerProvider.LoggerCallbackInfo = loggerCallbackInfo;
        LoggerProvider.LoggerCallbackError = loggerCallbackError;
        IniFile.FileName = iniFileFullPath;
        LocalizationManager.Init(stringCapsuleJsonFileData);
    }

}
