namespace Game03Client.Logger;

public delegate void LoggerCallback(object message);

public class LoggerOptions(LoggerCallback loggerCallback)
{
    public LoggerCallback _loggerCallback = loggerCallback;
}
