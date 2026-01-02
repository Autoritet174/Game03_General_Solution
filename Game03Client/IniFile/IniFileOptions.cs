namespace Game03Client.IniFile;

public sealed class IniFileOptions(string fileName)
{
    public string FileName { get; } = fileName;
}
