using System.Diagnostics.CodeAnalysis;

namespace Game03Client.Logger;

public interface ILogger<T>
{
    void Log(object message, string? keyLocal = null);

    [DoesNotReturn]
    void LogAndThrow(string message, string? keyLocal = null);
}
