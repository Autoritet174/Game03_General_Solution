@echo off

:: Получение параметра из командной строки
set archive_name_only=%1
set command=%2

echo ============
echo %archive_name_only% %command%
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

set archive_name="c:\UnityProjects\Game03_General_Solution\DataBaseBackupArchives\%archive_name_only%.7z"

set backup_folder=C:\Game03_DataBasesRestore

:: Создание папки для файлов бэкапа (игнорируем ошибки)
mkdir "%backup_folder%" >nul 2>&1

if command=="backup" (

	:: Создание бэкапа базы данных
	set backup_failed="0"
	if "%archive_name_only%"=="Users.postgres" (
		pg_dump -U postgres --host=127.127.126.5 --format=custom --verbose --no-owner --no-privileges --dbname=Game03_Users > "%backup_folder%\Users.postgres" >nul 2>nul || set backup_failed="1"
	)

	if "%archive_name_only%"=="GameData.postgres" (
		pg_dump -U postgres --host=127.127.126.5 --format=custom --verbose --no-owner --no-privileges --dbname=Game03_GameData > "%backup_folder%\GameData.postgres" >nul 2>nul || set backup_failed="1"
	)

	if "%archive_name_only%"=="UsersData.mongodb" (
		"C:\OSPanel\addons\MDBTools-100.12\mongodump.exe" --host 127.127.126.5 --port 27017 --db userData --out "%backup_folder%" >nul 2>nul || set backup_failed="1"
	)

	if "%backup_failed%"=="1" (
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
	
)
if command=="restore" (

	:: Распаковка архива
	"C:\Program Files\7-Zip\7z.exe" x -y "%archive_name%" -o"%backup_folder%" >nul || (
		echo ERROR: Failed to extract archive
		pause
		exit /b 1
	)
	echo ------------Archive extracted
	
	
	:: Восстановление базы данных
	set backup_failed="0"
	
	
	
	if "%archive_name_only%"=="Users.postgres" (
		
	)
	if "%archive_name_only%"=="GameData.postgres" (
		
	)
	if "%archive_name_only%"=="UsersData.mongodb" (
		"C:\OSPanel\addons\MDBTools-100.12\mongorestore.exe" --host 127.127.126.5 --port 27017 --drop --db userData "%backup_folder%\userData" >nul || set backup_failed="1"
	)
	if "%backup_failed%"=="1" (
		echo ERROR: Restore failed
		pause
		exit /b 1
	)
	echo ------------Database restored
	
)

:: Очистка временных файлов
rmdir /s /q "%backup_folder%" || >nul (
    echo ERROR: Failed to delete folder
    pause
    exit /b 1
)

echo COMPLETED!
pause