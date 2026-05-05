@echo off
chcp 65001 >nul
title Перезапуск PostgreSQL

echo ========================================
echo     Перезапуск сервера PostgreSQL
echo ========================================
echo.

REM Проверка прав администратора
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [ОШИБКА] Запустите скрипт от имени администратора!
    pause
    exit /b 1
)

REM Проверяем, существует ли служба (используем sc query без проверки кода ошибки)
echo Проверка наличия службы...
sc query postgresql-x64-18 >nul 2>&1
set SERVICE_EXISTS=%errorLevel%

if %SERVICE_EXISTS% neq 0 (
    echo [ОШИБКА] Служба postgresql-x64-18 не найдена!
    echo Проверьте название службы и версию PostgreSQL
    pause
    exit /b 1
)
echo [OK] Служба postgresql-x64-18 найдена

REM Проверяем текущий статус службы
echo Проверка текущего состояния службы...
sc query postgresql-x64-18 | findstr /C:"STATE" | findstr /C:"RUNNING" >nul
set SERVICE_RUNNING=%errorLevel%

if %SERVICE_RUNNING% equ 0 (
    echo [OK] Служба PostgreSQL запущена
    
    echo.
    echo [1/3] Останавливаю PostgreSQL...
    net stop postgresql-x64-18
    if %errorLevel% neq 0 (
        echo [ОШИБКА] Не удалось остановить службу
        pause
        exit /b 1
    )
    echo [OK] Служба остановлена
    timeout /t 3 /nobreak >nul
) else (
    echo [OK] Служба PostgreSQL не запущена, пропускаем остановку
)

echo.
echo [2/3] Запускаю PostgreSQL...
net start postgresql-x64-18
if %errorLevel% neq 0 (
    echo [ОШИБКА] Не удалось запустить службу
    echo Проверьте логи PostgreSQL или состояние порта 5432
    pause
    exit /b 1
)
echo [OK] Служба запущена
timeout /t 2 /nobreak >nul

echo.
echo [3/3] Проверяю статус...
sc query postgresql-x64-18 | findstr STATE

echo.
echo ========================================
echo     PostgreSQL успешно перезапущен!
echo ========================================
pause