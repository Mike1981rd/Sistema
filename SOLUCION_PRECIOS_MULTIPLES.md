# Solución de Problemas en Sistema de Precios Múltiples

## Fecha: Enero 2025

## Problemas Identificados

1. **Los precios base no se guardaban correctamente**
   - Aunque los datos se enviaban desde el frontend, no se persistían en la base de datos

2. **Los impuestos del segundo nivel de precio no se guardaban completos**
   - Solo se guardaba el primer impuesto seleccionado
   - Los impuestos adicionales se perdían

## Soluciones Implementadas

### 1. Corrección del Guardado de Impuestos (Backend)

**Archivo:** `/Controllers/API/ProductosController.cs`

**Problema:** En el método `ActualizarImpuestosPrecio`, se eliminaban y agregaban los impuestos pero no se ejecutaba `SaveChangesAsync()`, por lo que los cambios podían perderse.

**Solución:** Agregar `await _context.SaveChangesAsync()` al final del método:

```csharp
private async Task ActualizarImpuestosPrecio(int precioId, List<int> impuestoIds, int empresaId)
{
    // ... código existente ...
    
    // Guardar cambios de impuestos inmediatamente
    await _context.SaveChangesAsync();
    Console.WriteLine($"[DEBUG] Cambios de impuestos guardados para precio {precioId}");
}
```

### 2. Mejora en la Inicialización de Select2 (Frontend)

**Archivo:** `/wwwroot/js/productos/gestionar-producto.js`

**Problema:** Al cargar precios existentes, el Select2 podía no estar inicializado cuando se intentaban cargar los impuestos.

**Solución:** Asegurar que Select2 esté inicializado antes de cargar impuestos:

```javascript
function cargarPrecioEnFila($row, precio) {
    // ... código existente ...
    
    // Asegurar que el Select2 esté inicializado antes de cargar impuestos
    const $impuestoSelect = $row.find('.select2-impuestos');
    if ($impuestoSelect.length && !$impuestoSelect.data('select2')) {
        console.log('[DEBUG] Inicializando Select2 antes de cargar impuestos');
        const index = $row.closest('.price-row').index();
        if (typeof window.initializePriceRow === 'function') {
            window.initializePriceRow($row[0], index);
        }
    }
    
    // ... resto del código ...
}
```

### 3. Corrección de la Función agregarFilaPrecio

**Problema:** La función tenía código duplicado y no retornaba correctamente la fila creada en el fallback.

**Solución:** Limpiar el código y asegurar que siempre retorne el objeto jQuery de la fila creada.

## Script de Verificación

Se creó un script SQL para verificar el correcto guardado de precios e impuestos:

**Archivo:** `/Scripts/VerificarPreciosProductos.sql`

Este script permite:
- Ver los precios de los últimos productos
- Verificar los impuestos asociados a cada precio
- Contar la cantidad de impuestos por precio
- Ver detalles completos del producto más reciente

## Cómo Probar las Correcciones

1. **Crear/Editar un producto con múltiples precios:**
   - Agregar varios niveles de precio
   - Asignar diferentes impuestos a cada nivel
   - Guardar el producto

2. **Verificar en la base de datos:**
   - Ejecutar el script `VerificarPreciosProductos.sql`
   - Confirmar que cada precio tiene su PrecioBase correcto
   - Confirmar que todos los impuestos seleccionados se guardaron

3. **Recargar la página de edición:**
   - Los precios deben cargar con sus valores correctos
   - Los impuestos deben aparecer seleccionados correctamente

## Notas Importantes

1. El sistema mantiene logs detallados en la consola del navegador para debugging
2. Cada precio puede tener su propio conjunto de impuestos
3. El primer precio siempre se marca como principal (`EsPrincipal = true`)
4. Los cambios de impuestos ahora se guardan inmediatamente para evitar pérdida de datos

### 4. Corrección de Carga de Precios Base en Edición (UPDATE)

**Problema Adicional Identificado:** Los precios base se guardaban en la base de datos pero no se mostraban al editar el producto.

**Causa:** Discrepancia entre el formato de las propiedades del API (PascalCase) y lo que esperaba el JavaScript (camelCase).

**Solución:** Modificar la función `cargarPrecioEnFila` para soportar ambos formatos:

```javascript
function cargarPrecioEnFila($row, precio) {
    // Soportar tanto PascalCase (API) como camelCase (JavaScript)
    const nombreNivel = precio.NombreNivel || precio.nombreNivel || 'Precio Base';
    const precioBase = precio.PrecioBase || precio.precioBase || 0;
    const precioTotal = precio.PrecioTotal || precio.precioTotal || 0;
    const impuestoIds = precio.ImpuestoIds || precio.impuestoIds || [];
    
    // ... resto del código ...
}
```

**Scripts de Debug Adicionales:**
- `/Scripts/DebugPreciosCargas.sql` - Para verificar exactamente qué datos están en la BD

### 5. Corrección de Cálculos y Guardado de Impuestos Múltiples (UPDATE 2)

**Problemas Adicionales Identificados:**
1. Los cálculos no funcionaban en el primer nivel de precios
2. El segundo nivel solo guardaba un impuesto en lugar de múltiples

**Causas:**
1. Las funciones de cálculo usaban `impuestosDataGlobal` local en lugar de `window.impuestosDataGlobal`
2. Diferencias en la inicialización entre primer y segundo nivel
3. Problema en la actualización de nombres de campos

**Soluciones Implementadas:**

1. **Corregir Funciones de Cálculo:**
```javascript
// Cambiar de impuestosDataGlobal a window.impuestosDataGlobal
const percentage = (window.impuestosDataGlobal && window.impuestosDataGlobal[value]) || 0;
```

2. **Asegurar Inicialización Uniforme:**
```javascript
function initializeFirstRow() {
    const firstRow = priceRowsContainer.querySelector('.price-row');
    if (firstRow) {
        initializePriceRow(firstRow, 0);
    }
}
```

3. **Mejorar Actualización de Nombres:**
```javascript
function updateInputNames() {
    priceRowsContainer.querySelectorAll('.price-row').forEach((row, index) => {
        row.querySelectorAll('[data-original-name]').forEach(input => {
            const newName = `Precios[${index}].${originalName}`;
            input.name = newName;
        });
    });
}
```

**Scripts de Debug Adicionales:**
- `/Scripts/DebugPreciosCargas.sql` - Para verificar exactamente qué datos están en la BD
- `/Scripts/DebugImpuestosGuardado.sql` - Para verificar específicamente el guardado de impuestos múltiples

## Estado Final

✅ Los precios base se guardan correctamente
✅ Múltiples impuestos se guardan para todos los niveles de precio
✅ La carga de precios existentes funciona correctamente
✅ El Select2 se inicializa correctamente en filas dinámicas
✅ Los precios base se muestran correctamente al editar productos
✅ Los cálculos funcionan correctamente en todos los niveles de precio
✅ El guardado de múltiples impuestos funciona en todos los niveles