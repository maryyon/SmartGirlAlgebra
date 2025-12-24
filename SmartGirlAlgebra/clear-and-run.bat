@echo off
echo ========================================
echo Smart Girl Algebra - Clean Build Script
echo ========================================
echo.

REM Kill any running dotnet processes
echo Stopping any running app...
taskkill /F /IM dotnet.exe 2>nul
timeout /t 2 /nobreak >nul

REM Clean the build output
echo Cleaning build output...
dotnet clean
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

REM Clear browser cache directories (Chrome, Edge, Firefox)
echo Clearing browser cache...
taskkill /F /IM chrome.exe 2>nul
taskkill /F /IM msedge.exe 2>nul
taskkill /F /IM firefox.exe 2>nul
timeout /t 2 /nobreak >nul

REM Delete Chrome cache
rd /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Cache" 2>nul
rd /s /q "%LOCALAPPDATA%\Google\Chrome\User Data\Default\Code Cache" 2>nul

REM Delete Edge cache
rd /s /q "%LOCALAPPDATA%\Microsoft\Edge\User Data\Default\Cache" 2>nul
rd /s /q "%LOCALAPPDATA%\Microsoft\Edge\User Data\Default\Code Cache" 2>nul

echo.
echo Building fresh...
dotnet build

echo.
echo Starting app on http://localhost:5000
echo.
echo ========================================
echo IMPORTANT: Open a NEW browser window!
echo Go to: http://localhost:5000
echo ========================================
echo.

dotnet run

