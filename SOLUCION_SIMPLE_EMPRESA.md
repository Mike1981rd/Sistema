# Solución Simple para el Problema de Edit Items

## El Problema
Los dropdowns (Select2) en la pantalla de Edit Items no cargan los datos guardados. El problema es que el UserService devuelve un empresaId fijo (1) que podría no coincidir con el empresaId real de los datos.

## Solución Simple (Sin cambios invasivos)

### 1. Modificar UserService para usar la sesión

Ya hicimos este cambio en `UserService.cs`:

```csharp
public int GetEmpresaId()
{
    // Primero intentar obtener de la sesión
    if (_httpContextAccessor.HttpContext?.Session != null)
    {
        if (_httpContextAccessor.HttpContext.Session.TryGetValue("EmpresaId", out byte[] empresaIdBytes) && 
            empresaIdBytes != null)
        {
            return BitConverter.ToInt32(empresaIdBytes, 0);
        }
    }
    
    // Si no hay sesión, devolver valor por defecto
    return 1; // Valor por defecto para desarrollo
}
```

### 2. Establecer la empresa en la sesión

Crear un endpoint simple para establecer la empresaId correcta:

```csharp
// En EmpresasController
[HttpPost]
public IActionResult EstablecerEmpresaActual(int id)
{
    HttpContext.Session.SetInt32("EmpresaId", id);
    return Json(new { success = true });
}
```

### 3. Usar la vista de diagnóstico simple

Navegar a `/Item/SimpleDiagnostic` para ver:
- Qué empresaId está usando el sistema
- Qué empresa existe en la BD
- Si hay diferencia entre ambos

### 4. Si hay diferencia, establecer el ID correcto

La vista SimpleDiagnostic mostrará un botón para establecer automáticamente el empresaId correcto en la sesión.

## Resultado esperado

Con estos cambios mínimos:
1. El UserService devolverá el empresaId correcto de la sesión
2. Los SelectLists en Edit cargarán los datos correctos
3. No se necesitan cambios invasivos en todo el sistema

## Para probar

1. Navegar a `/Item/SimpleDiagnostic`
2. Verificar si el empresaId del UserService coincide con el de la BD
3. Si no coincide, usar el botón para establecer el correcto
4. Probar Edit Items nuevamente

Esta solución es mucho más simple y menos invasiva que intentar cambiar todo el sistema a async.