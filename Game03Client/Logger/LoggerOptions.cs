namespace Game03Client.Logger;

public delegate void LoggerCallback(object message);

internal class LoggerOptions(LoggerCallback loggerCallback)
{
    internal LoggerCallback _loggerCallback = loggerCallback;
}
