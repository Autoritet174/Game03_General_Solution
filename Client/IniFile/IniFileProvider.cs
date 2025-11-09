using IniParser;
using IniParser.Model;

namespace Client.IniFile;
internal class IniFileProvider(FileIniDataParser fileIniDataParser, IniFileOptions options) : IIniFileProvider
{
    private readonly IniFileOptions _options = options;
    private readonly FileIniDataParser _fileIniDataParser = fileIniDataParser;

    public string Read(string section, string key)
    {
        IniData data = _fileIniDataParser.ReadFile(_options.FileName);
        return data[section][key];
    }

    public void Write(string section, string key, string value)
    {
        IniData data = _fileIniDataParser.ReadFile(_options.FileName);
        data[section][key] = value;
        _fileIniDataParser.WriteFile(_options.FileName, data);
    }
}
