# Solución Completa y Agresiva para Error HTTP 431

## Descripción del Error

El error HTTP 431 "Request Header Fields Too Large" se produce cuando el tamaño total de las cabeceras HTTP en una solicitud excede el límite configurado en el servidor. Este error está afectando críticamente a la aplicación, impidiendo su correcto funcionamiento.

## Causas Identificadas

Después de análisis adicional, identificamos varias causas potenciales:

1. **Cookies de sesión sobredimensionadas**: El tamaño de las cookies está creciendo demasiado.
2. **Llamadas AJAX con cabeceras extensas**: Las solicitudes AJAX para gestionar la sesión están acumulando cabeceras.
3. **Ciclos de redirección con estado**: Los ciclos de redirección están incrementando datos en sesión y cabeceras.
4. **Parámetros anti-caché demasiado largos**: Los timestamps y otros parámetros están sobrecargando las URLs.

## Solución Implementada

Hemos aplicado un enfoque agresivo con múltiples capas para solucionar definitivamente el problema:

### 1. Configuración de Kestrel (Servidor Web)

```csharp
// Configuración de límites Kestrel agresivamente aumentados
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestHeadersTotalSize = 65536; // 64KB (4 veces el valor predeterminado)
    options.Limits.MaxRequestHeaderCount = 100;
    options.Limits.MaxRequestBodySize = 30 * 1024 * 1024; // 30MB
});
```

### 2. Política de Cookies Optimizada

```csharp
// Configurar política de cookies para reducir tamaño
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});
```

### 3. Middleware para Límites de Formulario

```csharp
// Configurar límites de formularios para peticiones grandes
app.Use(async (context, next) => {
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 30 * 1024 * 1024; // 30MB
    await next();
});
```

### 4. Simplificación del Enfoque de Sesión

Hemos reemplazado el enfoque basado en AJAX por un enfoque más simple basado en redirecciones directas:

```html
<!-- Enfoque simplificado en _SetEmpresaPartial.cshtml -->
<script>
    // Solución simplificada para evitar problemas con cabeceras HTTP demasiado grandes
    // No usamos AJAX - usamos un redirect directo al controlador de sesión
    console.log("[SetEmpresa] Redirigiendo a controlador de sesión");
    
    // Guardamos data mínima en sessionStorage
    sessionStorage.setItem('currentPath', window.location.pathname);
    
    // Redirigimos directamente al endpoint de configuración de empresa
    window.location.href = '/Session/SetEmpresa?id=1&t=@DateTime.Now.Ticks';
</script>
```

### 5. Controlador de Sesión Optimizado

Hemos rediseñado `SessionController` para:

- Minimizar datos en `TempData` y sesión
- Implementar verificación de existencia de empresas más eficiente
- Mejorar el manejo de errores y logging
- Usar un enfoque minimalista para establecer la sesión

## Pruebas Recomendadas

1. **Borrar caché y cookies**: En el navegador, borrar completamente el historial, caché y cookies antes de probar.
2. **Monitorear respuestas HTTP**: Usar las herramientas de desarrollo del navegador (F12) para verificar las cabeceras de respuesta.
3. **Verificar tamaño de sesión**: Visitar `/Session/Current` para monitorear los datos de sesión.
4. **Comprobar funcionalidad básica**: Verificar que la aplicación mantenga la empresa seleccionada durante la navegación.

## Solución Alternativa de Emergencia

Si el error persiste, puede configurar temporalmente un servidor proxy inverso como Nginx con límites de cabecera más permisivos:

```nginx
# Ejemplo de configuración de Nginx
http {
    large_client_header_buffers 8 64k;
    client_header_buffer_size 64k;
    ...
}
```

## Prevención a Futuro

1. **Monitoreo regular**: Configurar alertas para códigos de error HTTP 431
2. **Optimización de sesión**: Minimizar datos almacenados en sesión
3. **Almacenamiento alternativo**: Considerar usar bases de datos para datos de sesión en lugar de cookies
4. **Revisión de middleware**: Evaluar periódicamente la pipeline de middleware y su impacto en las cabeceras HTTP