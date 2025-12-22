using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using static General.StringExt;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres;

/// <summary>
/// Класс расширяющих методов для Entity Framework Core ModelBuilder
/// </summary>
public static class ModelBuilderExt
{
    public static void ApplyDefaultValues(this ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            EntityTypeBuilder entity = modelBuilder.Entity(entityType.ClrType);

            foreach (PropertyInfo property in entityType.ClrType.GetProperties())
            {
                HasDefaultValueAttribute? attr = property.GetCustomAttribute<HasDefaultValueAttribute>();
                if (attr != null)
                {
                    _ = entity.Property(property.Name).HasDefaultValue(attr.Value);
                }
            }
        }
    }

    /// <summary>
    /// Добавить тег [ConcurrencyToken] к свойствам "Version"
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddConcurrencyTokenToVersion(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            IMutableProperty? versionProperty = entityType.FindProperty("Version");
            if (versionProperty != null && versionProperty.ClrType == typeof(long))
            {
                // Делаем Version ConcurrencyToken для всех сущностей
                versionProperty.IsConcurrencyToken = true;

                // Опционально: устанавливаем значение по умолчанию
                versionProperty.SetDefaultValue(1L);
            }
        }
    }


    /// <summary>
    /// Преобразует имена таблиц, столбцов, ключей и индексов модели.
    /// </summary>
    /// <param name="modelBuilder">Экземпляр ModelBuilder.</param>
    public static void CorrectNames(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName() is string tableName)
            {
                entity.SetTableName(tableName.ToSnakeCase());
            }

            // Обработка имен столбцов: изменение только если имя не задано явно
            foreach (IMutableProperty property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            // Обработка первичных ключей: изменение только если имя не задано явно
            IMutableKey? pk = entity.FindPrimaryKey();
            pk?.SetName($"{entity.GetTableName().ToSnakeCase()}__pkey");

            // Обработка индексов: изменение только если имя не задано явно
            foreach (IMutableIndex index in entity.GetIndexes())
            {
                index.SetDatabaseName($"{entity.GetTableName().ToSnakeCase()}__{string.Join("__", index.Properties.Select(static p => p.GetColumnName().ToSnakeCase()))}__idx");
            }

            // Обработка внешних ключей: изменение только если имя не задано явно
            foreach (IMutableForeignKey fk in entity.GetForeignKeys())
            {
                string principalTable = fk.PrincipalEntityType.GetTableName().ToSnakeCase() ?? string.Empty;
                string columnName = fk.Properties[0].GetColumnName().ToSnakeCase();
                string newName = $"{entity.GetTableName().ToSnakeCase()}__{columnName}__{principalTable}__fkey";
                fk.SetConstraintName(newName);
            }

            // Обработка схем (пространств имен)
            if (entity.GetSchema() is string schemaName)
            {
                // Преобразуем первую букву в нижний регистр
                string newSchemaName = schemaName.ToSnakeCase();

                // Применяем только если изменилось
                if (newSchemaName != schemaName)
                {
                    entity.SetSchema(newSchemaName);
                }
            }
            
        }
    }

    /// <summary>
    /// Преобразует первую букву имени схемы (пространства имен) в нижний регистр.
    /// Примеры: _List -> _list, List -> list, XCross -> xCross, _XCross -> _xCross
    /// </summary>
    public static void FirstLetterToLowerInScheme(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetSchema() is string schemaName)
            {
                // Преобразуем первую букву в нижний регистр
                string newSchemaName = FirstLetterToLower(schemaName);

                // Применяем только если изменилось
                if (newSchemaName != schemaName)
                {
                    entity.SetSchema(newSchemaName);
                }
            }
        }
    }

    /// <summary>
    /// Преобразует первую букву строки в нижний регистр.
    /// Оставляет без изменений, если первый символ не буква.
    /// </summary>
    private static string FirstLetterToLower(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        char[] chars = input.ToCharArray();

        // Ищем первую букву
        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsLetter(chars[i]))
            {
                // Нашли первую букву
                if (char.IsUpper(chars[i]))
                {
                    // Меняем на нижний регистр только если это верхний регистр
                    chars[i] = char.ToLowerInvariant(chars[i]);
                }
                // Если уже нижний регистр - не меняем

                return new string(chars);
            }
        }

        // Если не нашли букв - возвращаем без изменений
        return input;
    }

    /// <summary>
    /// Проверяет, было ли имя таблицы явно указано через атрибут или Fluent API.
    /// </summary>
    /// <param name="entity">Тип сущности EF Core.</param>
    /// <returns>true, если имя таблицы явно указано; иначе false.</returns>
    private static bool IsExplicitlyNamedTable(this IMutableEntityType entity)
    {
        // 1. Проверяем наличие атрибута [Table]
        if (entity.ClrType.GetCustomAttribute<TableAttribute>() != null)
        {
            return true;
        }

        // 2. Проверяем, отличается ли текущее имя таблицы от имени по умолчанию
        string? currentTableName = entity.GetTableName();
        string defaultTableName = entity.ClrType.Name;

        // 3. Проверяем наличие явной схемы
        string? currentSchema = entity.GetSchema();

        // Для явного определения имени таблицы через Fluent API:
        // - Либо имя таблицы отличается от имени класса
        // - Либо указана схема
        // - Либо есть аннотация TableName (что означает, что ToTable() был вызван)

        bool hasTableNameAnnotation = entity.FindAnnotation("Relational:TableName") != null;
        bool hasSchemaAnnotation = entity.FindAnnotation("Relational:Schema") != null;

        // Если ToTable() был вызван с тем же именем, что и у класса,
        // все равно считаем это явным указанием
        if (hasTableNameAnnotation && hasSchemaAnnotation)
        {
            return true;
        }

        // Если есть аннотация имени таблицы и она отличается от имени класса
        if (hasTableNameAnnotation &&
            currentTableName != null &&
            !string.Equals(currentTableName, defaultTableName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Если есть явная схема
        return hasSchemaAnnotation && !string.IsNullOrEmpty(currentSchema);
    }

    /// <summary>
    /// Проверяет, было ли имя колонки явно указано через атрибут или Fluent API.
    /// </summary>
    /// <param name="property">Свойство EF Core.</param>
    /// <returns>true, если имя колонки явно указано; иначе false.</returns>
    private static bool IsExplicitlyNamedColumn(this IMutableProperty property)
    {
        // Проверяем атрибут [Column]
        if (property.PropertyInfo?.GetCustomAttribute<ColumnAttribute>() != null)
        {
            return true;
        }

        // Проверяем Fluent API через аннотацию
        IAnnotation? annotation = property.FindAnnotation("Relational:ColumnName");
        if (annotation == null || annotation.Value == null)
        {
            return false;
        }

        // Если есть аннотация ColumnName и она отличается от имени свойства
        string columnName = annotation.Value.ToString()!;
        return !string.Equals(columnName, property.Name, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Проверяет, было ли имя ключа явно указано через атрибут или Fluent API.
    /// </summary>
    /// <param name="key">Первичный ключ EF Core.</param>
    /// <returns>true, если имя ключа явно указано; иначе false.</returns>
    private static bool IsExplicitlyNamedConstraint(this IMutableKey key)
    {
        IAnnotation? annotation = key.FindAnnotation("Relational:Name");
        return annotation != null && annotation.Value != null;
    }

    /// <summary>
    /// Проверяет, было ли имя индекса явно указано через атрибут или Fluent API.
    /// </summary>
    /// <param name="index">Индекс EF Core.</param>
    /// <returns>true, если имя индекса явно указано; иначе false.</returns>
    private static bool IsExplicitlyNamedIndex(this IMutableIndex index)
    {
        IAnnotation? annotation = index.FindAnnotation("Relational:Name");
        return annotation != null && annotation.Value != null;
    }

    /// <summary>
    /// Проверяет, было ли имя внешнего ключа явно указано через атрибут или Fluent API.
    /// </summary>
    /// <param name="fk">Внешний ключ EF Core.</param>
    /// <returns>true, если имя внешнего ключа явно указано; иначе false.</returns>
    private static bool IsExplicitlyNamedConstraint(this IMutableForeignKey fk)
    {
        IAnnotation? annotation = fk.FindAnnotation("Relational:Name");
        return annotation != null && annotation.Value != null;
    }

}
