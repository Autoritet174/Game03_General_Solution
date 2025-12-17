using System;

namespace General;

/// <summary>
/// Оборотка исключений для ошибок в процессе взаимодействия с базой данных.
/// </summary>
public static class ThrowHelper
{
    /// <summary>
    /// Выбрасывает InvalidOperationException если exists равен false.
    /// </summary>
    public static void ThrowIfRecordNotExists(bool exists)
    {
        if (!exists)
        {
            throw new InvalidOperationException($"exists is false.");
        }
    }
    /// <summary>
    /// Выбрасывает ArgumentException если guid равен Guid.Empty.
    /// </summary>
    public static void ThrowIfGuidEmpty(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("UUID is empty.");
        }
    }
}
