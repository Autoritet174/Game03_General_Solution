@echo off
REM ========================================
REM Восстановление базы PostgreSQL
REM ========================================

REM Укажите параметры
set PGPATH="C:\Program Files\PostgreSQL\16\bin"
set DBNAME=mydatabase
set USER=postgres
set PASSWORD=MyStrongPassword
set BACKUPFILE=C:\PostgresBackups\mydatabase_2025-09-28.backup

REM Установим пароль
set PGPASSWORD=%PASSWORD%

REM Удаляем старую базу (если нужно) и создаём новую
%PGPATH%\dropdb -U %USER% %DBNAME%
%PGPATH%\createdb -U %USER% %DBNAME%

REM Восстанавливаем из файла
%PGPATH%\pg_restore -U %USER% -d %DBNAME% -v "%BACKUPFILE%"

echo Восстановление завершено из %BACKUPFILE%
pause
