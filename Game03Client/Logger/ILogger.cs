namespace Game03Client.Logger;

public interface ILogger
{
    void Log(object message);
    void LogEx(string nameClass, string message);
}
