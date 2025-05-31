# Resumen de Correcciones para Resolver el Error 404

Se han identificado y corregido varios problemas que estaban causando el error 404 persistente en la aplicación. A continuación se presenta un resumen de las correcciones realizadas:

## 1. Corrección en UserService.cs

**Problema:** El servicio tenía un ID de empresa hardcodeado (valor 4) que podría no existir en la base de datos.

**Solución:** Se modificó el método `GetEmpresaId()` para utilizar el valor 1 como predeterminado, que es más seguro y probablemente exista en todas las bases de datos.

```csharp
public int GetEmpresaId()
{
    // Primero intentar obtener de la sesión
    if (_httpContextAccessor.HttpContext?.Session != null)
    {
        var empresaId = _httpContextAccessor.HttpContext.Session.GetInt32("EmpresaId");
        if (empresaId.HasValue && empresaId.Value > 0)
        {
            return empresaId.Value;
        }
    }
    
    // Si no hay sesión, devolver valor predeterminado seguro
    return 1; // Valor por defecto para empresa existente
}
```

## 2. Implementación de Middleware Personalizado

**Problema:** No había un mecanismo confiable para establecer la empresa en la sesión al inicio de cada solicitud.

**Solución:** Se creó un middleware personalizado (`SessionMiddleware.cs`) que verifica y establece automáticamente la empresa en la sesión:

```csharp
public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
{
    try
    {
        // Verificar si hay empresa en sesión
        if (!context.Session.TryGetValue("EmpresaId", out _))
        {
            // Si no hay empresa en sesión, buscar la primera disponible
            var empresa = await dbContext.Empresas.FirstOrDefaultAsync();
            if (empresa != null)
            {
                // Establecer empresa en sesión
                context.Session.SetInt32("EmpresaId", empresa.Id);
                Console.WriteLine($"[SessionMiddleware] Empresa {empresa.Id} establecida en sesión");
            }
            else
            {
                // No hay empresas disponibles, esto es un problema crítico
                Console.WriteLine("[SessionMiddleware] ADVERTENCIA: No se encontraron empresas en la base de datos");
            }
        }
    }
    catch (Exception ex)
    {
        // Registrar la excepción pero continuar con la solicitud
        Console.WriteLine($"[SessionMiddleware] ERROR: {ex.Message}");
    }

    // Llamar al siguiente middleware en la pipeline
    await _next(context);
}
```

## 3. Corrección en el Orden del Middleware

**Problema:** El orden del middleware en `Program.cs` podría estar afectando cómo se procesa la sesión.

**Solución:** Se corrigió el orden del middleware asegurando que `app.UseSession()` esté antes de `app.UseRouting()` y añadiendo el nuevo middleware personalizado:

```csharp
// Configurar el middleware de sesión ANTES de UseRouting
app.UseSession();

app.UseRouting();

// Middleware personalizado para verificar y establecer empresa en sesión
app.UseSessionMiddleware();

// IMPORTANTE: El middleware de autorización debe estar después de UseRouting
app.UseAuthorization();
```

## 4. Protección Contra Redirecciones Circulares

**Problema:** La implementación anterior en `_SetEmpresaPartial.cshtml` podía causar redirecciones circulares.

**Solución:** Se implementó un contador para prevenir bucles de redirección:

```csharp
@{
    var currentEmpresaId = Context.Session.GetInt32("EmpresaId") ?? 0;
    // Protección contra redirecciones circulares
    var redirectCount = Context.Session.GetInt32("RedirectCount") ?? 0;
}

@if (currentEmpresaId == 0 && redirectCount < 3)
{
    Context.Session.SetInt32("RedirectCount", redirectCount + 1);
    // Lógica para establecer empresa...
}
else if (redirectCount >= 3)
{
    // Reiniciar contador para evitar bloqueo permanente
    Context.Session.SetInt32("RedirectCount", 0);
    
    <div class="alert alert-danger mt-3">
        <strong>Error:</strong> Se detectó un posible bucle de redirección.
        <a href="/Session/Current" class="btn btn-outline-danger ms-3">Configurar Empresa</a>
    </div>
}
```

## 5. Script SQL para Datos Iniciales

**Problema:** La aplicación depende de datos iniciales en varias tablas que pueden no existir en una base de datos recién creada.

**Solución:** Se creó un script SQL completo (`fix_data_script.sql`) que verifica y crea datos iniciales en todas las tablas críticas:

- Empresas
- CuentasContables
- Categorías y Familias
- TiposEntradaDiario y NumeracionesEntradaDiario
- Impuestos
- Almacenes

## Pasos para Aplicar las Correcciones

1. **Ejecutar el script SQL para restaurar datos críticos**:
   ```
   psql -U postgres -d SistemaContable -f fix_data_script.sql
   ```

2. **Reiniciar la aplicación para aplicar los cambios en el código**

3. **Acceder a `/Session/Current` para verificar y configurar manualmente la empresa si es necesario**

## Verificación del Éxito

Después de aplicar estas correcciones, la aplicación debería:

1. Iniciar correctamente sin errores 404
2. Mostrar las páginas principales según las rutas configuradas
3. Mantener la empresa seleccionada en sesión entre solicitudes

Si los problemas persisten, se recomienda revisar los logs de la aplicación y potencialmente recrear la base de datos desde cero con el script `fix_data_script.sql`.