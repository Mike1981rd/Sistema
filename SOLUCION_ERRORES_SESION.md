# Solución Problemas de Sesión y Empresa en la Aplicación

## Resumen del Problema

La aplicación presentaba errores 404 recurrentes debido a problemas relacionados con la gestión de sesiones y la asignación de empresas. Los principales problemas identificados fueron:

1. Fallos en el manejo de sesiones al establecer la empresa actual
2. Posibles problemas con cachés de navegador y solicitudes asíncronas
3. Falta de manejo de errores y diagnóstico en situaciones de fallo
4. Problemas con entidades (como Clientes) sin referencia a EmpresaId

## Soluciones Implementadas

### 1. Mejora en API/EmpresasController

Se ha mejorado significativamente el controlador de API para empresas:

- Incorporación de manejo de errores robusto con try-catch en todos los endpoints
- Logging extensivo para facilitar el diagnóstico de problemas
- Verificación que la empresa existe antes de establecerla en sesión
- Implementación de failover automático cuando no hay empresa en sesión
- Prevención de referencias a empresas inexistentes

### 2. Actualización de _SetEmpresaPartial.cshtml

La vista parcial que gestiona la configuración de empresas ha sido mejorada:

- Implementación de mecanismos de caché-busting con timestamps
- Uso de AJAX con parámetros contra el caché en llamadas a la API
- Almacenamiento de información de diagnóstico en sessionStorage
- Mejora del sistema anti-bucles de redirección
- Mensajes de error más informativos para el usuario

### 3. Página de Diagnóstico Mejorada

Se ha actualizado Session/Current para proporcionar mejor información de diagnóstico:

- Visualización de información client-side y server-side
- Herramientas para verificar y establecer empresas manualmente
- Mecanismo para limpiar la sesión y reiniciar el estado
- Visualización detallada de errores de redirección o API
- Integración con la información de sessionStorage para diagnóstico

### 4. Mejoras en SessionController

- Manejo de errores mejorado en el método SetEmpresa
- Reinicio del contador de redirecciones al establecer empresa
- Logging mejorado para facilitar el diagnóstico
- Uso de timestamps para evitar problemas de caché

### 5. Solución para Clientes sin EmpresaId

- Script SQL para actualizar clientes existentes sin EmpresaId
- Modificación del ClientesController para asignar EmpresaId en Create y Edit
- Filtrado correcto en Index para mostrar solo clientes de la empresa actual

## Instrucciones para Aplicar la Solución

1. **Ejecutar script SQL para clientes sin EmpresaId**:
   ```bash
   psql -U postgres -d SistemaContable -f fix_clientes.sql
   ```

2. **Verificar la empresa en sesión**:
   1. Acceder a `/Session/Current` para ver el estado actual
   2. Si no hay empresa en sesión, usar los botones para establecer una
   3. En caso de problemas, usar el botón "Limpiar Sesión" y reintentar

3. **Verificar las APIs de empresa**:
   - `/api/empresas/primera` - Debe retornar la primera empresa
   - `/api/empresas/current` - Debe retornar la empresa actual o establecer una

## Diagnóstico de Problemas Persistentes

Si aún se presentan problemas:

1. Verificar logs de consola del navegador (F12) buscando errores en llamadas a API
2. Revisar logs del servidor para identificar excepciones en el manejo de sesiones
3. Comprobar que existan empresas en la base de datos (puede crear una desde Home/Debug)
4. Validar la configuración de cookies y almacenamiento de sesión del navegador
5. En último caso, usar `/Home/Index?clear=true` para limpiar la sesión completamente

## Notas Técnicas Adicionales

- La gestión de sesión en ASP.NET Core requiere que el middleware de sesión esté correctamente configurado en `Program.cs`
- La relación entre Clientes y Empresas es crítica para el funcionamiento de filtros
- El uso de JavaScript para establecer y verificar la empresa mejora la experiencia pero puede ser susceptible a problemas de caché
- El sistema implementa un mecanismo de failover para detectar y recuperarse automáticamente de problemas de sesión