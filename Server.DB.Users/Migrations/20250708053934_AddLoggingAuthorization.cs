using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class AddLoggingAuthorization : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {

        _ = migrationBuilder.Sql("""
            CREATE TABLE users_authorization_logs (
            	id UUID NOT NULL PRIMARY KEY,
            	user_id UUID NOT NULL, -- Идентификатор пользователя
            	success BOOLEAN NOT NULL, -- Успешна ли авторизация
            	created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(), -- Временная метка
            	email CHARACTER VARYING(255) NOT NULL, -- Электронная почта пользователя
            	password_hash CHARACTER VARYING(255) NOT NULL, -- Пароль пользователя

            	ip_address INET, -- IP адрес

            	device_model CHARACTER VARYING(255), -- Модель устройства
            	device_type CHARACTER VARYING(255), -- Тип устройства (например, Desktop, Handheld)
            	operating_system CHARACTER VARYING(255), -- Операционная система
            	processor_type CHARACTER VARYING(255), -- Тип процессора
            	processor_count INTEGER, -- Кол-во ядер процессора
            	system_memory_size INTEGER, -- ОЗУ в МБ
            	graphics_device_name CHARACTER VARYING(255), -- Название видеокарты
            	graphics_memory_size INTEGER -- Видеопамять в МБ
            );
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

        _ = migrationBuilder.Sql("""
            DROP TABLE IF EXISTS users_authorization_logs;
            """);
    }
}
