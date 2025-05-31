#!/bin/bash

echo "===== DIAGNÓSTICO Y REPARACIÓN DE APLICACIÓN ASP.NET CORE ====="
echo ""

# Cambiar al directorio del proyecto
cd "/mnt/c/Users/hp/Documents/Visual Studio 2022/Projects/AuroraContabilidad/SistemaContable"

# Detener cualquier proceso de dotnet existente
echo "Deteniendo procesos existentes..."
pkill -f dotnet || true
sleep 2

# Limpiar la carpeta bin y obj
echo "Limpiando carpetas temporales..."
rm -rf obj bin

# Restaurar los paquetes NuGet
echo "Restaurando paquetes NuGet..."
dotnet restore

# Limpiar la solución
echo "Limpiando la solución..."
dotnet clean

# Reconstruir la solución
echo "Reconstruyendo la solución..."
dotnet build --configuration Debug

echo ""
echo "===== EJECUTANDO LA APLICACIÓN ====="
echo ""
echo "Para acceder a la aplicación, abre un navegador web y ve a:"
echo "http://localhost:5089"
echo ""
echo "Presiona Ctrl+C para detener la aplicación."
echo ""

# Ejecutar la aplicación en el puerto correcto según launchSettings.json
dotnet run --urls=http://localhost:5089