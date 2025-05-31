# Guía para Solucionar Error 404 en la Aplicación

Si estás experimentando un error 404 persistente en todas las rutas de la aplicación, sigue estos pasos para diagnosticar y resolver el problema:

## 1. Verificar la Configuración del Proyecto

### Asegúrate de que estás ejecutando el proyecto correcto:
- En Visual Studio, verifica que el proyecto SistemaContable está configurado como proyecto de inicio
- Si tienes varios proyectos en la solución, asegúrate de que estás ejecutando el correcto

### Verifica la URL en el navegador:
- Asegúrate de que estás accediendo a la URL correcta
- Los puertos correctos según launchSettings.json son:
  - HTTP: http://localhost:5089
  - HTTPS: https://localhost:7168
  - IIS Express: https://localhost:44351

## 2. Reinstalar la Base de Datos

Si el problema persiste, posiblemente haya un problema con la base de datos:

1. Detener la aplicación
2. Abrir pgAdmin o una herramienta de PostgreSQL
3. Ejecutar los siguientes comandos SQL:

```sql
-- Terminar todas las conexiones activas a la base de datos
SELECT pg_terminate_backend(pg_stat_activity.pid)
FROM pg_stat_activity
WHERE pg_stat_activity.datname = 'SistemaContable'
AND pid <> pg_backend_pid();

-- Eliminar y recrear la base de datos
DROP DATABASE IF EXISTS "SistemaContable";
CREATE DATABASE "SistemaContable";
```

4. Reiniciar la aplicación (se aplicarán las migraciones automáticamente)

## 3. Modificar el archivo Program.cs

Modifica tu archivo Program.cs para usar la configuración mínima que garantice el funcionamiento:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Servicios esenciales
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Pipeline mínima
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
```

## 4. Verificar la Empresa por Consola

Si después de reiniciar la aplicación aún tienes problemas, verifica la existencia de empresas directamente:

1. Abre una consola o terminal
2. Conéctate a PostgreSQL: `psql -U postgres -d SistemaContable`
3. Ejecuta: `SELECT * FROM "Empresas";`
4. Si no hay resultados, inserta una empresa:

```sql
INSERT INTO "Empresas" (
    "Nombre", "NumeroIdentificacion", "TipoIdentificacion", 
    "Direccion", "Ciudad", "Provincia", "CodigoPostal", 
    "Pais", "Telefono", "Email", "SitioWeb", 
    "NombreComercial", "MonedaPrincipal", "NumeroEmpleados", 
    "PrecisionDecimal", "SeparadorDecimal", "LogoUrl", 
    "ResponsabilidadTributaria", "FechaCreacion", "Activo"
) VALUES (
    'Empresa Predeterminada', '000-0000000-0', 'RNC',
    'Dirección Predeterminada', 'Ciudad', 'Provincia', '00000',
    'República Dominicana', '000-000-0000', 'info@empresa.com', 'www.empresa.com',
    'Empresa Demo', 'DOP', 5,
    2, '.', '/images/logo.png',
    'Persona Jurídica', NOW(), TRUE
);
```

## 5. Corregir EmpresaService.cs

Asegúrate de que EmpresaService.cs no tenga valores hardcoded:

```csharp
public async Task<int> ObtenerEmpresaActualId()
{
    try
    {
        // Intenta obtener el ID de empresa de la sesión
        if (_httpContextAccessor.HttpContext?.Session != null)
        {
            var empresaId = _httpContextAccessor.HttpContext.Session.GetInt32("EmpresaId");
            if (empresaId.HasValue && empresaId.Value > 0)
            {
                return empresaId.Value;
            }
        }
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Error al obtener empresa de sesión");
    }
    
    // Si no se pudo obtener de la sesión, busca en la base de datos
    try
    {
        var empresa = await _context.Empresas.FirstOrDefaultAsync();
        if (empresa != null)
        {
            return empresa.Id;
        }
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Error al obtener empresa de base de datos");
    }
    
    return 1; // valor predeterminado
}
```

## 6. Configurar para Depuración

En Visual Studio:
1. Establece el proyecto en modo Debug
2. En el menú Debug > Windows > Exception Settings, desmarca "Break when this exception type is user-unhandled" para .NET Framework y .NET Core Exceptions
3. Ejecuta en modo depuración para ver exactamente dónde falla

## Verificación de Puertos

Si crees que el puerto puede ser el problema, puedes intentar forzar un puerto específico:

1. Abre una terminal en la carpeta del proyecto
2. Ejecuta: `dotnet run --urls="http://localhost:5000"`
3. Accede a: `http://localhost:5000`

## Reinicio de Visual Studio

Como último recurso, intenta:
1. Cerrar Visual Studio
2. Reiniciar tu computadora
3. Volver a abrir Visual Studio
4. Limpiar la solución (Build > Clean Solution)
5. Reconstruir la solución (Build > Rebuild Solution)
6. Ejecutar nuevamente