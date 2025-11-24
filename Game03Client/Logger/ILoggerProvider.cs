namespace Game03Client.Logger;

public interface ILoggerProvider
{
    void Log(object message);
    void LogEx(string nameClass, string message);
}
