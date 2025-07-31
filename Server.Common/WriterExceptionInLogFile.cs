using System.Text;

namespace Server.Common;

public static class WriterExceptionInLogFile
{
    private static readonly string logDir;

    static WriterExceptionInLogFile()
    {
        logDir = Path.Combine(AppContext.BaseDirectory, "logs");
        _ = Directory.CreateDirectory(logDir);
    }

    /// <summary>
    /// Логирует исключение в файл.
    /// </summary>
    public static async Task LogToFileAsync(Exception ex)
    {
        try
        {
            DateTime nowUtc = DateTime.UtcNow;
            string logPath = Path.Combine(logDir, $"ServerExceptions_[{nowUtc:yyyy-MM-dd}].log");

            StringBuilder sb = new();
            _ = sb.AppendLine($"[{nowUtc:yyyy-MM-dd HH:mm:ss.fff} (UTC)] Exception logged:");

            _ = sb.AppendLine();
            _ = sb.AppendLine(ex.ToString());
            _ = sb.AppendLine(new string('*', 100));
            _ = sb.AppendLine();

            await File.AppendAllTextAsync(logPath, sb.ToString());
        }
        catch { }
    }
}
