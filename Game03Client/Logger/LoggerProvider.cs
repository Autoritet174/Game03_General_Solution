namespace Game03Client.Logger;


internal class LoggerProvider(LoggerOptions loggerOptions) : ILogger
{
    public void Log(object message)
    {
        loggerOptions._loggerCallback?.Invoke(message);
    }

    public void LogEx(string nameClass, string message)
    {
        loggerOptions._loggerCallback?.Invoke($"[{nameClass}] {message}");
    }
}
