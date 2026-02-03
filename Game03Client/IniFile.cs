using IniParser;
using IniParser.Model;
using System.Globalization;
using System.IO;

namespace Game03Client;

public class IniFile
{
    private static readonly Logger<IniFile> logger = new();
    private static readonly FileIniDataParser _fileIniDataParser = new();
    public static string FileName { get; internal set; } = null!;
    public static string? Read(string section, string key)
    {
        if (File.Exists(FileName))
        {
            try
            {
                IniData data = _fileIniDataParser.ReadFile(FileName);
                try
                {
                    return data[section][key];
                }
                catch
                {
                    logger.LogError($"error read section=[{section}] key=[{key}] in file <{FileName}>");
                }
            }
            catch
            {
                logger.LogError($"error read ini file <{FileName}>");
            }
        }
        return null;
    }

    public static double ReadDouble(string section, string key, double defaultValue = 0)
    {
        string? valueString = Read(section, key)?.Trim();
        if (!string.IsNullOrEmpty(valueString))
        {
            if (double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
            {
                return parsedValue;
            }
        }

        logger.LogError($"not found value in [{section}][{key}]");
        return defaultValue;
    }

    public static void Write(string section, string key, string value)
    {
        IniData data = _fileIniDataParser.ReadFile(FileName);
        data[section][key] = value;
        _fileIniDataParser.WriteFile(FileName, data);
    }
}
