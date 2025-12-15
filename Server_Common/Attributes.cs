namespace Server_Common;

/// <summary>
/// Кастомные атрибуты.
/// </summary>
public class Attributes
{
    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HasDefaultValueAttribute(object value) : Attribute
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public object Value { get; } = value;
    }
}
