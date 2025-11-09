namespace Client.IniFile;

internal sealed class IniFileOptions(string fileName)
{
    public string FileName { get; } = fileName;
}
