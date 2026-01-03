namespace Game03Client.Logger;

public delegate void LoggerCallbackError(object message);
public delegate void LoggerCallbackInfo(object message);

public class LoggerOptions(LoggerCallbackError loggerCallbackError, LoggerCallbackInfo loggerCallbackInfo)
{
    public LoggerCallbackError _loggerCallbackError = loggerCallbackError;
    public LoggerCallbackInfo _loggerCallbackInfo = loggerCallbackInfo;
}
