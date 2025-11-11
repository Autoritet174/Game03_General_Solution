using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game03Client.LocalizationManager;
public interface ILocalizationManagerProvider
{
    string GetTextByJObject(JObject jObject);
    string GetValue(string key);
}
