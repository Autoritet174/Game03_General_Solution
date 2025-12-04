using Microsoft.EntityFrameworkCore;

namespace Server_Common;

public static class ModelBuilderExt
{
    public static void ModelToSnakeCase(this ModelBuilder modelBuilder)
    {
        // ПРИВЕДЕНИЕ ВСЕХ ИМЕН К СТАНДАРТУ snake_case
        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            // Таблицы в snake_case
            if (entity.GetTableName() is string tableName)
            {
                entity.SetTableName(tableName.ToSnakeCase());
            }

            // Колонки в snake_case
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            // Первичный ключ:
            Microsoft.EntityFrameworkCore.Metadata.IMutableKey? pk = entity.FindPrimaryKey();
            pk?.SetName($"{entity.GetTableName()}_pkey");

            // Индексы:
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableIndex index in entity.GetIndexes())
            {
                index.SetDatabaseName($"{entity.GetTableName()}_{string.Join("_", index.Properties.Select(p => p.GetColumnName()))}_idx");
            }

            // Внешние ключи:
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey fk in entity.GetForeignKeys())
            {
                string principalTable = fk.PrincipalEntityType.GetTableName()!.ToSnakeCase();
                string columnName = fk.Properties[0].GetColumnName().ToSnakeCase();
                fk.SetConstraintName($"{entity.GetTableName()}_{columnName}_{principalTable}_fkey");
            }
        }
    }
}
