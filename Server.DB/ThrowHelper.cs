namespace Server.DB;

public static class ThrowHelper
{
    /// <summary>
    /// Выбрасывает InvalidOperationException если exists равен false.
    /// </summary>
    public static void ThrowIfRecordNotExists(bool exists)
    {
        if (!exists)
        {
            throw new InvalidOperationException($"Запись не найдена или удалена (поле DeletedAt не null).");
        }
    }
    /// <summary>
    /// Выбрасывает ArgumentException если guid равен Guid.Empty.
    /// </summary>
    public static void ThrowIfGuidEmpty(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Идентификатор UUID не может быть нулевым.");
        }
    }
}