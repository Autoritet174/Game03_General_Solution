using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Attributes;

/// <summary>
/// Значение по умолчанию.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DefaultAttribute(object value) : Attribute
{
    /// <summary> Значение. </summary>
    public object Value { get; } = value;
}

/// <summary>
/// Атрибут для указания, что свойство должно храниться в базе данных в виде jsonb.
/// </summary>
public class JsonbAttribute : ColumnAttribute
{
    public JsonbAttribute() : base()
    {
        TypeName = "jsonb";
    }
}

