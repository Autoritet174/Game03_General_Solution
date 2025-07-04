using System.Runtime.InteropServices;

namespace Server.Utilities;

public static class ConsoleWindow
{
    private const int SW_RESTORE = 9;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// Разворачивает окно консоли, если оно было свернуто.
    /// </summary>
    public static void Restore()
    {
        try
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                _ = ShowWindow(handle, SW_RESTORE);
            }
        }
        catch { }
    }
}
