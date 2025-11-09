using System;
using System.Collections.Generic;
using System.Text;

namespace Client.IniFile;
internal interface IIniFileProvider
{
    void Write(string section, string key, string value);
    string Read(string section, string key);
}
