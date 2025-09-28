@echo off
echo ============
echo MONGODB TOOL
echo ============
echo BACKUP
echo.
set /p confirm="Write 'yes' to continue?: "
if /i not "%confirm%"=="yes" (
    if /i not "%confirm%"=="yes" (
        echo Operation cancelled
        pause
        exit /b 0
    )
)

:: Получение параметра из командной строки
if not "%1"=="" set archive_folder=%1

:: Если параметр не передан, используем значение по умолчанию
if "%archive_folder%"=="" (
	set archive_name=e:\_ExchangeFolder\Game03_DataBasesBackups\mongoDb.7z
) else (
	set archive_name="%archive_folder%"\mongoDb.7z
)

set backup_folder=C:\Game03_DataBasesRestore

:: Создание папки для восстановления (игнорируем ошибки)
mkdir "%backup_folder%" >nul 2>&1

:: Создание бэкапа базы данных
"C:\OSPanel\addons\MDBTools-100.12\mongodump.exe" --host 127.127.126.5 --port 27017 --db userData --out "%backup_folder%" >nul 2>nul || (
    echo ERROR: Backup failed
    pause
    exit /b 1
)
echo ------------Backup created

:: Создание архива
"C:\Program Files\7-Zip\7z.exe" a -t7z "%archive_name%" "%backup_folder%\*" >nul || (
    echo ERROR: Archive creation failed
    pause
    exit /b 1
)
echo ------------Archive created

:: Очистка временных файлов
rmdir /s /q "%backup_folder%" || >nul (
    echo ERROR: Failed to delete folder
    pause
    exit /b 1
)
echo ------------Folder deleted

echo COMPLETED!
pause