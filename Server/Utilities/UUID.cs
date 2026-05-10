using System.Runtime.CompilerServices;

namespace Server.Utilities;

public static class UUID
{
    /// <summary>
    /// Возвращает новый Guid v7.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.Guid CreateV7() => UUIDNext.Uuid.NewSequential();
}
