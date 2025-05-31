@echo off
echo ===== DIAGNÓSTICO Y REPARACIÓN DE APLICACIÓN ASP.NET CORE =====
echo.

REM Detener cualquier proceso de dotnet
echo Deteniendo procesos existentes...
taskkill /f /im dotnet.exe 2>nul
taskkill /f /im iisexpress.exe 2>nul
timeout /t 2 /nobreak >nul

REM Limpiar la carpeta bin y obj
echo Limpiando carpetas temporales...
rd /s /q obj 2>nul
rd /s /q bin 2>nul

REM Restaurar los paquetes NuGet
echo Restaurando paquetes NuGet...
dotnet restore

REM Limpiar la solución
echo Limpiando la solución...
dotnet clean

REM Reconstruir la solución
echo Reconstruyendo la solución...
dotnet build --configuration Debug

echo.
echo ===== EJECUTANDO LA APLICACIÓN =====
echo.
echo Para acceder a la aplicación, abre un navegador web y ve a:
echo http://localhost:5089
echo.
echo Presiona Ctrl+C para detener la aplicación.
echo.

REM Ejecutar la aplicación en el puerto correcto según launchSettings.json
dotnet run --urls=http://localhost:5089

pause