using Game03Client.Logger;
using IniParser;
using IniParser.Model;
using System.Globalization;
using System.IO;

namespace Game03Client.IniFile;

internal class IniFileProvider(IniFileOptions options
    , ILogger<IniFileProvider> logger
    ) : IIniFile
{
    private readonly FileIniDataParser _fileIniDataParser = new();

    public string? Read(string section, string key)
    {
        if (File.Exists(options.FileName))
        {
            try
            {
                IniData data = _fileIniDataParser.ReadFile(options.FileName);
                try
                {
                    return data[section][key];
                }
                catch
                {
                    logger.Log($"error read section=[{section}] key=[{key}] in file <{options.FileName}>");
                }
            }
            catch
            {
                logger.Log($"error read ini file <{options.FileName}>");
            }
        }
        return null;
    }

    public double ReadDouble(string section, string key, double defaultValue = 0)
    {
        string? valueString = Read(section, key)?.Trim();
        if (!string.IsNullOrEmpty(valueString))
        {
            if (double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
            {
                return parsedValue;
            }
        }

        logger.Log($"not found value in [{section}][{key}]");
        return defaultValue;
    }

    public void Write(string section, string key, string value)
    {
        IniData data = _fileIniDataParser.ReadFile(options.FileName);
        data[section][key] = value;
        _fileIniDataParser.WriteFile(options.FileName, data);
    }
}
