@echo off
echo ============
echo POSTGRES BACKUP
echo ============
echo
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

:: Распаковка архива
"C:\Program Files\7-Zip\7z.exe" x -y "%archive_name%" -o"%backup_folder%" >nul || (
    echo ERROR: Failed to extract archive
    pause
    exit /b 1
)
echo ------------Archive extracted

:: Восстановление базы данных
"C:\OSPanel\addons\MDBTools-100.12\mongorestore.exe" --host 127.127.126.5 --port 27017 --drop --db userData "%backup_folder%\userData" >nul || (
    echo ERROR: Database restore failed
    pause
    exit /b 1
)
echo ------------Database restored

:: Очистка временных файлов
rmdir /s /q "%backup_folder%" || >nul (
    echo ERROR: Failed to delete folder
    pause
    exit /b 1
)
echo ------------Folder deleted

echo COMPLETED!
pause