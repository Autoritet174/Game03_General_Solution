namespace Game03Client.IniFile;

public interface IIniFile
{
    void Write(string section, string key, string value);
    string? Read(string section, string key);
    double ReadDouble(string section, string key, double defaultValue);
}
