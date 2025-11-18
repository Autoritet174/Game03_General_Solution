using System;
using System.Collections.Generic;
using System.Text;

namespace Game03Client.Logger;

public interface ILoggerProvider
{
    event LoggerCallback OnLog;
    void Log(object message);
    void LogEx(string nameClass, string message);
}
