namespace Game03Client.Logger;

public delegate void LoggerCallback(object message);

internal class LoggerProvider : ILoggerProvider
{
    public event LoggerCallback? OnLog = null;

    public void Log(object message)
    {
        OnLog?.Invoke(message);
    }
    public void LogEx(string nameClass, string message) {
        OnLog?.Invoke($"[{nameClass}] {message}");
    }
}
