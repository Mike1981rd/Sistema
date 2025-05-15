# Diagnóstico del problema con Select2 de Categorías

## Cambios realizados:

1. **URL con window.location.origin**: 
   - Cambié de `/Categoria/Buscar` a `window.location.origin + '/Categoria/Buscar'`
   - Agregué logs de consola para depuración

2. **Condición para mostrar "Crear categoría"**:
   - Cambié de mostrar siempre a mostrar solo cuando no hay resultados
   - Agregué marca `_isNew` para identificar elementos nuevos

3. **Clase del select**:
   - Cambié de `form-control` a `form-select` para consistencia con Bootstrap 5

4. **Tema de Select2**:
   - Agregué el tema Bootstrap 5 en los estilos
   - Tema 'bootstrap-5' ya estaba configurado en el JS

5. **Scripts de depuración**:
   - Creé `/wwwroot/js/productos/debug.js` para diagnóstico
   - Agregué logs para verificar:
     - Si jQuery está cargado
     - Si Select2 está cargado
     - Si Bootstrap está cargado
     - Si el elemento existe en el DOM
     - Si Select2 se inicializa correctamente

## Posibles problemas:

1. **Orden de carga**: Los scripts podrían estar cargando en orden incorrecto
2. **Timing**: El DOM podría no estar listo cuando se ejecuta el script
3. **Endpoint**: El endpoint `/Categoria/Buscar` podría no existir o no responder correctamente
4. **Permisos**: Podría haber un problema de autorización en el endpoint

## Próximos pasos para verificar:

1. Abrir la consola del navegador y revisar los logs
2. Verificar si hay errores 404 o 500 en las llamadas AJAX
3. Verificar que el endpoint `/Categoria/Buscar` existe y responde correctamente
4. Probar con una URL completa en lugar de relativa